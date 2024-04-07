// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Animal_AI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_AI : Unit_obj
{
	public enum Animal_state
	{
		idle,
		walk,
		eating,
		walking,
		escape
	}

	public Animator animator;

	public AnimalHead animalHead;

	public Animal_state ActiveState;

	public float dangerDistance;

	public NavMeshAgent nav;

	public int Health = 1000;

	public bool isAlive = true;

	public float walkSpeed = 2f;

	public float runSpeed = 7f;

	public Transform camera;

	public bool bKillingProccess;

	public bool bHungry;

	public bool bDanger;

	public bool bWalk;

	public bool bTakeDamage;

	public bool bTraped;

	public int hungryCounter;

	public float staminaCounter;

	public GameObject ragdoll;

	public Vector3 currentDist;

	private float stopDistance = 0.5f;

	public Transform enemy;

	public float takeDamageWaitTime;

	public float walkingWaitTime;

	public float walkingWaitTimeMax;

	public float eatingWaitTime;

	public float eatingWaitTimeMax;

	public float dangerWaitTime;

	public bool bEnemyInRadius;

	public Transform point;

	public List<Transform> escapePoints = new List<Transform>();

	public List<Transform> walkPoints = new List<Transform>();

	public List<Transform> foodPoints = new List<Transform>();

	public GameObject bloodSpot;

	public Transform detectionTrigger;

	public Vector3 currentWalkingPoint;

	public Vector3 currentEatingPoint;

	public Vector3 currentEscapePoint;

	public int eatingPointID;

	public AudioClip walkSound;

	public AudioClip[] howlSounds;

	public bool StepDown;

	public AudioSource audioSource;

	public bool stateSound;

	public float walkSoundDelay;

	public float runSoundDelay;

	private void OnTriggerEnter(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<Enemy_AI>())
		{
			enemy = info.gameObject.transform;
			bEnemyInRadius = true;
		}
	}

	private void OnTriggerExit(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<Enemy_AI>())
		{
			bEnemyInRadius = false;
		}
	}

	private void Start()
	{
		NotificationsManager.Instance.AddListener(this, "Event_PlayerFire");
		SetWalkingState();
	}

	public void Event_PlayerFire(Component sender)
	{
		if (!bDanger)
		{
			Debug.Log("PlayerFire");
			float num = Vector3.Distance(base.transform.position, sender.GetComponent<Transform>().position);
			if (num < 300f)
			{
				SetDangerState();
			}
		}
	}

	public void Event_Trapped()
	{
		SetTrapedState();
	}

	public void SetCamera(bool state)
	{
		if (camera != null && (bool)camera.GetComponent<RecordCamera_obj>())
		{
			camera.gameObject.SetActive(state);
		}
	}

	public void SetHungryState()
	{
		ResetAllDestination();
		RefreshPoints();
		nav.Resume();
		bHungry = true;
		nav.speed = walkSpeed;
		if (currentEatingPoint == Vector3.zero)
		{
			int num = Random.Range(0, foodPoints.Count);
			eatingPointID = num;
			currentEatingPoint = foodPoints[eatingPointID].position;
		}
	}

	public void SetDangerState()
	{
		audioSource.PlayOneShot(howlSounds[Random.Range(0, howlSounds.Length)]);
		ResetAllDestination();
		nav.Resume();
		bDanger = true;
		animator.SetBool("bEat", value: false);
		animator.SetBool("bWalk", value: false);
		animator.SetFloat("Speed", 2f);
		nav.speed = runSpeed;
		if (currentEscapePoint == Vector3.zero)
		{
			currentEscapePoint = escapePoints[Random.Range(0, escapePoints.Count)].position;
		}
	}

	public void SetWalkingState()
	{
		nav.ResetPath();
		ResetAllDestination();
		nav.Resume();
		bWalk = true;
		animator.SetFloat("Speed", 1f);
		nav.speed = walkSpeed;
		if (currentWalkingPoint == Vector3.zero)
		{
			currentWalkingPoint = walkPoints[Random.Range(0, walkPoints.Count)].position;
		}
	}

	public void SetTrapedState()
	{
		bTraped = true;
		bHungry = false;
		SetCamera(state: true);
	}

	private void Update()
	{
		if (isAlive)
		{
			if (bTraped)
			{
				Idle();
			}
			else if (bDanger)
			{
				Escape();
			}
			else
			{
				if (bEnemyInRadius && enemy != null)
				{
					if (Vector3.Distance(enemy.position, base.transform.position) < dangerDistance)
					{
						SetDangerState();
					}
					if (animalHead.bSeen())
					{
						SetDangerState();
					}
				}
				if (bHungry)
				{
					Eating();
				}
				else
				{
					Walking();
				}
			}
		}
		if (bKillingProccess)
		{
			Killing();
		}
	}

	public void Killing()
	{
		nav.Stop();
		bHungry = false;
		bWalk = false;
		animator.SetFloat("Speed", 0f);
		bDanger = false;
	}

	private void Idle()
	{
		Stop();
		animator.SetFloat("Speed", 0f);
		animator.SetBool("bEat", value: false);
	}

	private void Walking()
	{
		if (Vector3.Distance(currentWalkingPoint, base.transform.position) < stopDistance)
		{
			animator.SetFloat("Speed", 0f);
			animator.SetBool("bWalk", value: true);
			ActiveState = Animal_state.walking;
			walkingWaitTime += Time.deltaTime;
			if (walkingWaitTime > walkingWaitTimeMax)
			{
				animator.SetBool("bWalk", value: false);
				int[] array = new int[2] { 0, 1 };
				animator.SetInteger("WalkType", Random.Range(0, array.Length));
				walkingWaitTime = 0f;
				animator.SetFloat("Speed", 1f);
				Vector3 sourcePosition = Random.insideUnitSphere * 5f;
				sourcePosition += currentWalkingPoint;
				NavMesh.SamplePosition(sourcePosition, out var hit, 5f, 1);
				int[] array2 = new int[4] { 10, 15, 20, 25 };
				hungryCounter += array2[Random.Range(0, array2.Length)];
				UpdateAnimalState();
				currentWalkingPoint = hit.position;
				int[] array3 = new int[2] { 3, 3 };
				walkingWaitTimeMax = array3[Random.Range(0, array3.Length)];
			}
		}
		else if (!nav.pathPending)
		{
			ActiveState = Animal_state.walk;
			PlayFootstep(walkSoundDelay);
			nav.SetDestination(currentWalkingPoint);
			animator.SetFloat("Speed", 1f);
		}
		else
		{
			animator.SetFloat("Speed", 0f);
		}
	}

	private void Eating()
	{
		if (Vector3.Distance(currentEatingPoint, base.transform.position) < 10f && Vector3.Distance(currentEatingPoint, base.transform.position) > stopDistance && foodPoints[eatingPointID].GetComponent<FoodPoint>().isUse)
		{
			SetHungryState();
		}
		if (Vector3.Distance(currentEatingPoint, base.transform.position) < stopDistance)
		{
			foodPoints[eatingPointID].GetComponent<FoodPoint>().isUse = true;
			animator.SetFloat("Speed", 0f);
			animator.SetBool("bEat", value: true);
			eatingWaitTime += Time.deltaTime;
			ActiveState = Animal_state.eating;
			if (eatingWaitTime > eatingWaitTimeMax)
			{
				foodPoints[eatingPointID].GetComponent<FoodPoint>().isUse = false;
				foodPoints[eatingPointID].GetComponent<FoodPoint>().Eat();
				eatingWaitTime = 0f;
				hungryCounter = 0;
				bHungry = false;
				currentWalkingPoint = Vector3.zero;
				animator.SetBool("bEat", value: false);
				animator.SetFloat("Speed", 1f);
				SetWalkingState();
			}
		}
		else
		{
			PlayFootstep(walkSoundDelay);
			ActiveState = Animal_state.walk;
			animator.SetBool("bEat", value: false);
			nav.SetDestination(currentEatingPoint);
			animator.SetFloat("Speed", 1f);
		}
	}

	private void Escape()
	{
		if (Vector3.Distance(currentEscapePoint, base.transform.position) < stopDistance)
		{
			animator.SetFloat("Speed", 0f);
			animator.SetBool("bWalk", value: false);
			animator.SetBool("bEat", value: false);
			if (!bEnemyInRadius)
			{
				dangerWaitTime += Time.deltaTime;
				ActiveState = Animal_state.idle;
				if (dangerWaitTime > 5f)
				{
					dangerWaitTime = 0f;
					currentEatingPoint = Vector3.zero;
					currentWalkingPoint = Vector3.zero;
					bDanger = false;
					if (bWalk)
					{
						SetWalkingState();
					}
					if (bHungry)
					{
						SetHungryState();
					}
				}
			}
			else
			{
				SetDangerState();
				dangerWaitTime = 0f;
			}
		}
		else
		{
			PlayFootstep(runSoundDelay);
			ActiveState = Animal_state.escape;
			animator.SetBool("bEat", value: false);
			nav.SetDestination(currentEscapePoint);
			animator.SetFloat("Speed", 2f);
		}
	}

	public void ResetAllDestination()
	{
		currentEatingPoint = Vector3.zero;
		currentEscapePoint = Vector3.zero;
		currentWalkingPoint = Vector3.zero;
	}

	private void RefreshPoints()
	{
		for (int i = 0; i < foodPoints.Count; i++)
		{
			if (foodPoints[i] == null)
			{
				foodPoints.RemoveAt(i);
			}
		}
	}

	private void Stop()
	{
		nav.Stop();
	}

	private bool bReached()
	{
		if (Vector3.Distance(base.transform.position, currentDist) < stopDistance)
		{
			return true;
		}
		return false;
	}

	public void AfterTrapEffect()
	{
		SetDangerState();
		bTraped = false;
	}

	private void PlayFootstep(float time)
	{
		if (!StepDown)
		{
			StartCoroutine(FootStep(time));
		}
	}

	private IEnumerator FootStep(float stepDelay)
	{
		StepDown = true;
		audioSource.PlayOneShot(walkSound);
		yield return new WaitForSeconds(stepDelay);
		StepDown = false;
	}

	public void UpdateAnimalState()
	{
		if ((float)hungryCounter > 100f)
		{
			SetHungryState();
		}
	}

	public override void TakeDamage(int amount, string damageType)
	{
		Health -= amount;
		if (damageType == "SniperShot")
		{
			SetDangerState();
		}
		if (damageType == "TrapDamage")
		{
			SetDangerState();
		}
		if (Health <= 0)
		{
			isAlive = false;
			animator.SetBool("bDead", value: true);
			Stop();
			Vector3 position = base.transform.position;
			position.y += 1f;
			Ray ray = new Ray(position, Vector3.down);
			if (Physics.Raycast(ray, out var hitInfo, 100f))
			{
				Object.Instantiate(bloodSpot, hitInfo.point + hitInfo.normal * 0.2f, Quaternion.LookRotation(-hitInfo.normal));
			}
			NotificationsManager.Instance.RemoveListener(this, "Event_PlayerFire");
			animator.SetFloat("Speed", 0f);
			animator.SetBool("bEat", value: false);
			Object.Instantiate(ragdoll, base.transform.position, Quaternion.identity);
			if (camera != null && (bool)camera.GetComponent<RecordCamera_obj>())
			{
				CameraManager.instance.RemoveCamera(camera.GetComponent<RecordCamera_obj>().setupCamera.transform);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
