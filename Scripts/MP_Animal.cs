// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Animal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Animal : MonoBehaviour
{
	public enum ENEMY_TYPE
	{
		Drone,
		ToughGuy,
		Boss
	}

	public enum MOVE_TYPE
	{
		Idle,
		Walk,
		Run
	}

	public enum ANIMAL_STATE
	{
		PATROL,
		CHASE,
		ATTACK,
		ESCAPE,
		CAMERA_BREAK
	}

	public ENEMY_TYPE Type;

	public int AnimalID;

	public int Health = 100;

	public int AttackDamage = 10;

	public float RecoveryDelay = 1f;

	protected Transform ThisTransform;

	protected NavMeshAgent Agent;

	[SerializeField]
	protected MP_PlayerController PC;

	[SerializeField]
	protected MP_RecordCamera_obj MC;

	protected Transform PlayerTransform;

	public float PatrolDistance = 10f;

	public float ChaseDistance = 10f;

	public float AttackDistance = 0.2f;

	public float EscapeDistance = 20f;

	public ANIMAL_STATE ActiveState;

	protected List<Transform> escapePoints = new List<Transform>();

	protected List<Transform> patrolPoints = new List<Transform>();

	public AnimatorProperties animatorProperties;

	public SoundProperties soundProp;

	public AudioSource SFX;

	protected float moveSpeed = 3f;

	protected float runSpeed = 13f;

	protected float walkSpeed = 3f;

	protected MP_RakeHealthUI UI_Health;

	public Transform lookPoint;

	public AudioClip footStep;

	public List<AudioClip> PlayerAttackRoar;

	public List<AudioClip> PlayerAttackKicks;

	public List<AudioClip> PainRoar;

	public AudioClip cameraAttack;

	public GameObject ragdoll;

	private void Start()
	{
		SFX = GetComponent<AudioSource>();
		soundProp = new SoundProperties();
		animatorProperties = new AnimatorProperties();
		animatorProperties.animator = GetComponent<Animator>();
		if (Network.isServer)
		{
			Agent = GetComponent<NavMeshAgent>();
			ThisTransform = base.transform;
			ChangeState(ActiveState);
		}
	}

	private void Update()
	{
		animatorProperties.animator.SetFloat("Speed", animatorProperties.speed);
		if (Network.isServer && !soundProp.StepDown && animatorProperties.speed > 0f)
		{
			GetComponent<NetworkView>().RPC("PlayFootStep", RPCMode.All, soundProp.stepSpeed);
		}
	}

	private void GetPatrolPoints()
	{
		GameObject gameObject = GameObject.Find("PatrolPointsHolder-" + AnimalID);
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			patrolPoints.Add(componentsInChildren[i]);
		}
		patrolPoints.RemoveAt(0);
		MonoBehaviour.print("Patrol points has been find! " + componentsInChildren.Length);
	}

	private void GetEscapePoints()
	{
		GameObject gameObject = GameObject.Find("EscapePointsHolder-" + AnimalID);
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			escapePoints.Add(componentsInChildren[i]);
		}
		escapePoints.RemoveAt(0);
	}

	public void ChangeState(ANIMAL_STATE State)
	{
		if (!Network.isServer)
		{
			StopAllCoroutines();
			Agent.Stop();
			return;
		}
		StopAllCoroutines();
		ActiveState = State;
		switch (ActiveState)
		{
		case ANIMAL_STATE.ATTACK:
			break;
		case ANIMAL_STATE.CHASE:
			break;
		case ANIMAL_STATE.PATROL:
			StartCoroutine(AI_Patrol());
			break;
		case ANIMAL_STATE.ESCAPE:
			StartCoroutine(AI_Escape());
			break;
		case ANIMAL_STATE.CAMERA_BREAK:
			break;
		}
	}

	private IEnumerator AI_Patrol()
	{
		Agent.Stop();
		soundProp.stepSpeed = 0.7f;
		soundProp.StepDown = false;
		while (ActiveState == ANIMAL_STATE.PATROL)
		{
			Vector3 randomPosition = Random.insideUnitSphere * PatrolDistance;
			randomPosition += ThisTransform.position;
			animatorProperties.speed = 1f;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomPosition, out hit, PatrolDistance, 1);
			Agent.speed = walkSpeed;
			Agent.Resume();
			Vector3 pos = randomPosition;
			Agent.SetDestination(hit.position);
			float ArrivalDistance = 2f;
			float TimeOut = 5f;
			float ElapsedTime = 0f;
			while (Vector3.Distance(ThisTransform.position, hit.position) > ArrivalDistance)
			{
				yield return null;
			}
		}
	}

	private IEnumerator AI_Escape()
	{
		Agent.Stop();
		soundProp.stepSpeed = 0.2f;
		soundProp.StepDown = false;
		if (ActiveState == ANIMAL_STATE.ESCAPE)
		{
			Vector3 randomPosition = Random.onUnitSphere * 40f;
			randomPosition += ThisTransform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, 40f, 1);
			animatorProperties.speed = 2f;
			Agent.Resume();
			Agent.speed = 8f;
			Agent.SetDestination(hit.position);
			float ArrivalDistance = 2f;
			float TimeOut = 180f;
			float ElapsedTime = 0f;
			while (Vector3.Distance(ThisTransform.position, hit.position) > ArrivalDistance)
			{
				yield return null;
			}
			ChangeState(ANIMAL_STATE.PATROL);
		}
	}

	public void OnObjectEnter(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<MP_FPSController>() && ActiveState != ANIMAL_STATE.ESCAPE)
		{
			ChangeState(ANIMAL_STATE.ESCAPE);
		}
	}

	[RPC]
	public void TakeDamage(int dmg)
	{
		Health -= dmg;
		if (Health <= 0)
		{
			Object.Instantiate(ragdoll, base.transform.position, Quaternion.identity);
			Object.Destroy(base.gameObject);
		}
	}

	[RPC]
	private void PlayFootStep(float stepSpeed)
	{
		StartCoroutine(FootStep(stepSpeed));
	}

	private IEnumerator FootStep(float stepDelay)
	{
		soundProp.StepDown = true;
		SFX.PlayOneShot(footStep);
		yield return new WaitForSeconds(stepDelay);
		soundProp.StepDown = false;
	}
}
