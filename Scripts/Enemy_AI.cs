// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Enemy_AI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : Unit_obj
{
	public Animator animator;

	public NavMeshAgent nav;

	public List<Transform> patrolPoints = new List<Transform>();

	public List<Transform> escapePoints = new List<Transform>();

	public Vector3 currentPatrolPoint;

	public Vector3 currentHountPoint;

	public Vector3 currentEscapePoint;

	public Vector3 currentPartialPoint;

	public Vector3 currentBreakPoint;

	public Vector3 viewRot;

	public Transform cameraBreak;

	public int currentStage;

	public AudioClip walkSound;

	public AudioClip[] playerAttackRoars;

	public AudioClip[] playerAttackKicks;

	public AudioClip[] painRoar;

	public AudioClip cameraAttackRoar;

	public AudioClip[] idleSounds;

	public bool StepDown;

	public AudioSource audioSource;

	public bool stateSound;

	public bool bDanger;

	public bool bHaunt;

	public bool bVisible;

	public bool bPlayerHount;

	public bool bKillingProccess;

	public bool bPlayerKilling;

	public bool bCameraBreak;

	public bool bHungry;

	public int hungryCounter;

	public float patrolWaitTime;

	public float hauntWaitTime;

	public float dangerWaitTime;

	public float breakWaitTime;

	public float playerHountWaitTime;

	private float stopDistance = 0.5f;

	public Transform target;

	public Transform playerTarget;

	public float walkSpeed;

	public float runSpeed;

	public float hauntSpeed;

	public Transform focusPoint;

	public float Health;

	public int PlayerAttackType;

	public GameObject deadPrefab;

	private void Start()
	{
		hauntSpeed = 3f;
		runSpeed = 16f;
		nav.acceleration = 100f;
		audioSource.maxDistance = 20f;
		Health = 4500f;
		NotificationsManager.Instance.AddListener(this, "Event_PlayerFire");
	}

	public void Event_PlayerFire(Component sender)
	{
		if (!bDanger && !bPlayerHount)
		{
			Debug.Log("PlayerFire");
			float num = Vector3.Distance(base.transform.position, sender.GetComponent<Transform>().position);
			if (num < 300f)
			{
				SetDangerState();
			}
		}
	}

	public Vector3 FindClosetPoint()
	{
		int index = 0;
		float num = Vector3.Distance(base.transform.position, patrolPoints[0].position);
		for (int i = 0; i < patrolPoints.Count; i++)
		{
			if (Vector3.Distance(base.transform.position, patrolPoints[i].position) < num)
			{
				num = Vector3.Distance(base.transform.position, patrolPoints[i].position);
				index = i;
			}
		}
		return patrolPoints[index].position;
	}

	public Vector3 BuildEscapePath()
	{
		Debug.Log("RebuildEsacpePath");
		float num = 300f;
		Vector3 sourcePosition = Random.insideUnitSphere * num;
		sourcePosition += base.transform.position;
		NavMeshHit hit;
        NavMesh.SamplePosition(sourcePosition, out hit, num * 2f, 1 << NavMesh.GetNavMeshLayerFromName("Default"));
		return hit.position;
	}

	public override void TakeDamage(int amount, string damageType)
	{
		Health -= amount;
		NotificationsManager.Instance.PostNotification(this, "OnDamageTaken");
		if (damageType == "SniperShot")
		{
			audioSource.PlayOneShot(painRoar[Random.Range(0, painRoar.Length)]);
			float num = Random.Range(0, 100);
			if (num < 30f)
			{
				SetDangerState();
			}
		}
		if (damageType == "TrapDamage")
		{
			float num2 = Random.Range(0, 100);
			if (num2 < 30f)
			{
				SetDangerState();
			}
		}
		if (Health <= 0f && !bPlayerKilling)
		{
			Object.Instantiate(deadPrefab, base.transform.position, base.transform.rotation);
			Object.Destroy(base.gameObject);
		}
	}

	public void OnDamageTaken()
	{
	}

	private void Update()
	{
		if (target != null)
		{
			if (!target.GetComponent<Animal_AI>())
			{
				bHaunt = false;
			}
		}
		else
		{
			bHaunt = false;
		}
		if (!bDanger)
		{
			if (!bHaunt && !bPlayerHount)
			{
				if (bCameraBreak)
				{
					CameraBreak();
				}
				else
				{
					Patrol();
				}
			}
			if (bPlayerHount)
			{
				if (!playerTarget.GetComponent<FPSController>().bHide)
				{
					PlayerHount();
				}
				else
				{
					bPlayerHount = false;
				}
			}
			if (bHaunt && !bPlayerHount && bHungry)
			{
				Haunting();
			}
		}
		else if (!bPlayerKilling)
		{
			Escape();
		}
	}

	public void SetDangerState()
	{
		if (!bPlayerKilling)
		{
			nav.Resume();
			stateSound = false;
			currentEscapePoint = BuildEscapePath();
			bDanger = true;
			animator.SetInteger("WalkType", 1);
			animator.SetFloat("Speed", 2f);
		}
	}

	public void RefreshStats()
	{
		if (hungryCounter >= 100)
		{
			hungryCounter = 0;
			bHungry = true;
		}
	}

	public void SetPlayerHountState()
	{
		nav.Resume();
		stateSound = false;
		animator.SetInteger("WalkType", 1);
		bPlayerHount = true;
		bHaunt = false;
	}

	public void SetPatrolState()
	{
		stateSound = false;
		animator.SetBool("bCrouch", value: false);
		nav.Resume();
		animator.SetInteger("WalkType", 1);
		target = null;
	}

	public void SetBreakState()
	{
		stateSound = false;
		nav.Resume();
		bCameraBreak = true;
	}

	private void PlayerHount()
	{
		if (currentStage == 0)
		{
			currentHountPoint = playerTarget.position;
			if (Vector3.Distance(currentHountPoint, base.transform.position) < 7f)
			{
				animator.SetFloat("Speed", 0f);
				animator.SetBool("bAttack", value: false);
				nav.Stop();
				playerHountWaitTime += Time.deltaTime;
				if (playerHountWaitTime > 10f)
				{
					playerHountWaitTime = 0f;
					bPlayerHount = false;
					nav.Resume();
				}
				if (Vector3.Distance(currentHountPoint, base.transform.position) < 3f)
				{
					nav.Resume();
					bPlayerHount = false;
					bDanger = true;
					playerHountWaitTime = 0f;
				}
			}
			else
			{
				nav.Resume();
				nav.SetDestination(currentHountPoint);
				animator.SetBool("bAttack", value: false);
				nav.speed = walkSpeed;
				animator.SetFloat("Speed", 1f);
			}
		}
		if (currentStage == 1)
		{
			Vector3 from = base.transform.position - playerTarget.transform.position;
			Vector3 forward = playerTarget.forward;
			float num = Vector3.Angle(from, forward);
			if (num < 60f)
			{
				bVisible = true;
			}
			else if (!bVisible)
			{
				bVisible = false;
			}
			if (bVisible)
			{
				if (PlayerAttackType == 0)
				{
					int[] array = new int[2] { 1, 2 };
					PlayerAttackType = array[Random.Range(0, array.Length)];
				}
				if (PlayerAttackType == 1)
				{
					currentHountPoint = playerTarget.position;
					if (Vector3.Distance(currentHountPoint, base.transform.position) < 5f)
					{
						nav.Stop();
						animator.SetFloat("Speed", 0f);
						bPlayerKilling = true;
						playerTarget.GetComponent<PlayerMovement>().FreezePlayer();
						playerTarget.GetComponent<CameraMouseLook>().bControl = false;
						playerTarget.GetComponent<FPSController>().HidePlayerWeapon();
						Vector3 position = playerTarget.position;
						position.y = base.transform.position.y;
						base.transform.LookAt(position);
						playerTarget.GetComponent<CameraMouseLook>().Focus(focusPoint.transform);
						animator.SetBool("bCrouch", value: false);
						if (!stateSound)
						{
							stateSound = true;
							audioSource.PlayOneShot(playerAttackRoars[Random.Range(0, playerAttackRoars.Length)]);
						}
						animator.SetBool("bPlayerAttack", value: true);
						playerHountWaitTime += Time.deltaTime;
						if (playerHountWaitTime > 1.2f)
						{
							audioSource.PlayOneShot(playerAttackKicks[Random.Range(0, playerAttackKicks.Length)]);
							bPlayerKilling = false;
							playerTarget.GetComponent<FPSController>().TakeDamage(10, "RakeKick");
							playerTarget.GetComponent<PlayerMovement>().UnFreezePlayer();
							playerTarget.GetComponent<CameraMouseLook>().bControl = true;
							stateSound = false;
							SetDangerState();
							nav.Resume();
							bDanger = true;
							playerHountWaitTime = 0f;
							bPlayerHount = false;
							bVisible = false;
							animator.SetBool("bAttack", value: false);
							animator.SetBool("bPlayerAttack", value: false);
						}
					}
					else
					{
						nav.Resume();
						nav.SetDestination(currentHountPoint);
						animator.SetBool("bCrouch", value: false);
						animator.SetBool("bAttack", value: false);
						nav.speed = runSpeed;
						PlayFootstep(0.2f);
						animator.SetFloat("Speed", 2f);
					}
				}
				if (PlayerAttackType == 2)
				{
					currentHountPoint = playerTarget.position;
					if (Vector3.Distance(currentHountPoint, base.transform.position) < 5f)
					{
						nav.Stop();
						animator.SetFloat("Speed", 0f);
						bPlayerKilling = true;
						playerTarget.GetComponent<PlayerMovement>().FreezePlayer();
						playerTarget.GetComponent<CameraMouseLook>().bControl = false;
						playerTarget.GetComponent<FPSController>().HidePlayerWeapon();
						Vector3 position2 = playerTarget.position;
						position2.y = base.transform.position.y;
						base.transform.LookAt(position2);
						playerTarget.GetComponent<CameraMouseLook>().Focus(focusPoint.transform);
						animator.SetBool("bCrouch", value: false);
						if (!stateSound)
						{
							stateSound = true;
							audioSource.PlayOneShot(playerAttackRoars[Random.Range(0, playerAttackRoars.Length)]);
						}
						animator.SetBool("bPlayerAppercut", value: true);
						playerHountWaitTime += Time.deltaTime;
						if (playerHountWaitTime > 1.2f)
						{
							audioSource.PlayOneShot(playerAttackKicks[Random.Range(0, playerAttackKicks.Length)]);
							bPlayerKilling = false;
							playerTarget.GetComponent<FPSController>().TakeDamage(20, "RakeUppercut");
							playerTarget.GetComponent<PlayerMovement>().UnFreezePlayer();
							playerTarget.GetComponent<CameraMouseLook>().bControl = true;
							stateSound = false;
							PlayerAttackType = 0;
							SetDangerState();
							nav.Resume();
							bDanger = true;
							playerHountWaitTime = 0f;
							bPlayerHount = false;
							bVisible = false;
							animator.SetBool("bAttack", value: false);
							animator.SetBool("bPlayerAppercut", value: false);
						}
					}
					else
					{
						nav.Resume();
						nav.SetDestination(currentHountPoint);
						animator.SetBool("bCrouch", value: false);
						animator.SetBool("bAttack", value: false);
						nav.speed = runSpeed;
						PlayFootstep(0.2f);
						animator.SetFloat("Speed", 2f);
					}
				}
			}
			else
			{
				currentHountPoint = playerTarget.position;
				if (Vector3.Distance(currentHountPoint, base.transform.position) < 3f)
				{
					nav.Stop();
					animator.SetFloat("Speed", 0f);
					bPlayerKilling = true;
					playerTarget.GetComponent<PlayerMovement>().FreezePlayer();
					playerTarget.GetComponent<CameraMouseLook>().bControl = false;
					playerTarget.GetComponent<FPSController>().HidePlayerWeapon();
					Vector3 position3 = playerTarget.position;
					position3.y = base.transform.position.y;
					base.transform.LookAt(position3);
					playerTarget.GetComponent<CameraMouseLook>().Focus(focusPoint.transform);
					animator.SetBool("bCrouch", value: false);
					if (!stateSound)
					{
						stateSound = true;
						audioSource.PlayOneShot(playerAttackRoars[Random.Range(0, playerAttackRoars.Length)]);
					}
					animator.SetBool("bPlayerAttack", value: true);
					playerHountWaitTime += Time.deltaTime;
					if (playerHountWaitTime > 1.2f)
					{
						audioSource.PlayOneShot(playerAttackKicks[Random.Range(0, playerAttackKicks.Length)]);
						bPlayerKilling = false;
						playerTarget.GetComponent<FPSController>().TakeDamage(10, "RakeUppercut");
						playerTarget.GetComponent<PlayerMovement>().UnFreezePlayer();
						playerTarget.GetComponent<CameraMouseLook>().bControl = true;
						stateSound = false;
						PlayerAttackType = 0;
						SetDangerState();
						nav.Resume();
						bDanger = true;
						playerHountWaitTime = 0f;
						bPlayerHount = false;
						bVisible = false;
						animator.SetBool("bAttack", value: false);
						animator.SetBool("bPlayerAttack", value: false);
					}
				}
				else
				{
					nav.Resume();
					nav.SetDestination(currentHountPoint);
					animator.SetBool("bAttack", value: false);
					nav.speed = hauntSpeed;
					animator.SetBool("bCrouch", value: true);
					animator.SetFloat("Speed", 0f);
				}
			}
		}
		if (currentStage != 2)
		{
			return;
		}
		currentHountPoint = playerTarget.position;
		if (Vector3.Distance(currentHountPoint, base.transform.position) < 3f)
		{
			nav.Stop();
			animator.SetFloat("Speed", 0f);
			animator.SetBool("bCrouch", value: false);
			animator.SetBool("bAttack", value: true);
			playerHountWaitTime += Time.deltaTime;
			if (playerHountWaitTime > 5f)
			{
				nav.Resume();
				bDanger = true;
				bVisible = false;
				playerHountWaitTime = 0f;
				bPlayerHount = false;
				animator.SetBool("bAttack", value: false);
			}
		}
		else
		{
			nav.Resume();
			nav.SetDestination(currentHountPoint);
			animator.SetBool("bCrouch", value: false);
			animator.SetBool("bAttack", value: false);
			nav.speed = runSpeed;
			animator.SetFloat("Speed", 2f);
		}
	}

	private void Escape()
	{
		if (Vector3.Distance(currentEscapePoint, base.transform.position) < stopDistance)
		{
			animator.SetFloat("Speed", 0f);
			dangerWaitTime += Time.deltaTime;
			if (dangerWaitTime > 2f)
			{
				dangerWaitTime = 0f;
				bPlayerHount = false;
				bVisible = false;
				bDanger = false;
				animator.SetFloat("Speed", 1f);
			}
			return;
		}
		if (nav.path.status == NavMeshPathStatus.PathPartial || nav.path.status == NavMeshPathStatus.PathInvalid)
		{
			animator.SetFloat("Speed", 0f);
			currentEscapePoint = BuildEscapePath();
			nav.Stop();
		}
		if (!nav.pathPending)
		{
			nav.Resume();
			nav.SetDestination(currentEscapePoint);
			PlayFootstep(0.2f);
			nav.speed = runSpeed;
			animator.SetFloat("Speed", 2f);
		}
		else
		{
			nav.Stop();
			animator.SetFloat("Speed", 0f);
		}
	}

	private void Patrol()
	{
		if (playerTarget != null && Vector3.Distance(playerTarget.position, base.transform.position) < 60f)
		{
			SetPlayerHountState();
		}
		if (currentPatrolPoint == Vector3.zero)
		{
			nav.Resume();
			currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)].position;
		}
		if (Vector3.Distance(currentPatrolPoint, base.transform.position) < stopDistance)
		{
			animator.SetFloat("Speed", 0f);
			patrolWaitTime += Time.deltaTime;
			if (!stateSound)
			{
				audioSource.PlayOneShot(idleSounds[Random.Range(0, idleSounds.Length)]);
				stateSound = true;
			}
			if (patrolWaitTime > 3f)
			{
				currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)].position;
				hungryCounter += 50;
				RefreshStats();
				if (nav.pathPending)
				{
					patrolWaitTime = 0f;
					stateSound = false;
				}
			}
		}
		else
		{
			if (nav.path.status == NavMeshPathStatus.PathPartial || nav.path.status == NavMeshPathStatus.PathInvalid)
			{
				animator.SetFloat("Speed", 0f);
				currentPatrolPoint = FindClosetPoint();
			}
			animator.SetBool("bAttack", value: false);
			nav.speed = walkSpeed;
			if (!nav.pathPending)
			{
				PlayFootstep(0.7f);
				nav.SetDestination(currentPatrolPoint);
				animator.SetFloat("Speed", 1f);
			}
			else
			{
				animator.SetFloat("Speed", 0f);
			}
		}
	}

	private void Haunting()
	{
		currentHountPoint = target.position;
		if (Vector3.Distance(currentHountPoint, base.transform.position) < 3f)
		{
			if (target.GetComponent<Animal_AI>().isAlive && !target.GetComponent<Animal_AI>().bTraped)
			{
				if (!bKillingProccess)
				{
					AnimalKilling();
				}
			}
			else
			{
				animator.SetBool("bAnimalKilling", value: false);
				bHaunt = false;
			}
		}
		else if (target.GetComponent<Animal_AI>().bDanger)
		{
			animator.SetBool("bAttack", value: false);
			nav.speed = runSpeed;
			animator.SetBool("bCrouch", value: false);
			animator.SetInteger("WalkType", 1);
			animator.SetFloat("Speed", 2f);
			nav.SetDestination(currentHountPoint);
		}
		else
		{
			animator.SetInteger("WalkType", 0);
			animator.SetBool("bAttack", value: false);
			nav.speed = hauntSpeed;
			animator.SetBool("bCrouch", value: true);
			animator.SetFloat("Speed", 0f);
			nav.SetDestination(currentHountPoint);
		}
	}

	public void CameraBreak()
	{
		if (Vector3.Distance(currentBreakPoint, base.transform.position) < 3f)
		{
			animator.SetBool("bCameraBreak", value: true);
			animator.SetFloat("Speed", 0f);
			breakWaitTime += Time.deltaTime;
			if (!stateSound)
			{
				stateSound = true;
				audioSource.PlayOneShot(cameraAttackRoar);
			}
			Vector3 vector = viewRot - base.transform.position;
			Quaternion quaternion = Quaternion.LookRotation(viewRot);
			viewRot.y = base.transform.position.y;
			base.transform.LookAt(viewRot);
			if (breakWaitTime > 1.9f)
			{
				breakWaitTime = 0f;
				animator.SetBool("bCameraBreak", value: false);
				if (cameraBreak != null)
				{
					cameraBreak.GetComponent<RecordCamera_obj>().SetUnsetupState();
					bCameraBreak = false;
				}
			}
		}
		else
		{
			PlayFootstep(0.2f);
			animator.SetBool("bCameraBreak", value: false);
			nav.speed = runSpeed;
			animator.SetBool("bCrouch", value: false);
			animator.SetInteger("WalkType", 1);
			animator.SetFloat("Speed", 2f);
			nav.SetDestination(currentBreakPoint);
		}
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

	public void AnimalKilling()
	{
		nav.Stop();
		bKillingProccess = true;
		animator.SetFloat("Speed", 0f);
		animator.SetBool("bCrouch", value: false);
		animator.SetBool("bAnimalKilling", value: true);
		target.GetComponent<Animal_AI>().bKillingProccess = true;
		StartCoroutine(kill());
	}

	private IEnumerator kill()
	{
		yield return new WaitForSeconds(2f);
		if (target != null)
		{
			target.GetComponent<Animal_AI>().TakeDamage(250, "RakeAnimalKick");
		}
		bHungry = false;
		animator.SetBool("bAnimalKilling", value: false);
		bKillingProccess = false;
		animator.SetBool("bCrouch", value: false);
		SetPatrolState();
		currentPatrolPoint = Vector3.zero;
	}
}
