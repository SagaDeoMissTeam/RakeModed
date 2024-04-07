// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Enemy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Enemy : MonoBehaviour
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

	public enum ENEMY_STATE
	{
		PATROL,
		CHASE,
		ATTACK,
		ESCAPE,
		CAMERA_BREAK,
		RUN
	}

	public ENEMY_TYPE Type;

	public int EnemyID;

	public int Health = 100;

	public int AttackDamage = 10;

	public float RecoveryDelay = 1f;

	protected Transform ThisTransform;

	public Vector3 runPoint;

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

	public ENEMY_STATE ActiveState;

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

	public GameObject rakeDeadPrefab;

	protected virtual void Start()
	{
		Health = 4500;
		UI_Health = Object.FindObjectOfType<MP_RakeHealthUI>();
		animatorProperties = new AnimatorProperties();
		animatorProperties.animator = GetComponent<Animator>();
		lookPoint = GameObject.Find("RakeLookPoint").transform;
		if (Network.isServer)
		{
			GetEscapePoints();
			GetPatrolPoints();
			Agent = GetComponent<NavMeshAgent>();
			ThisTransform = base.transform;
			ChangeState(ActiveState);
		}
	}

	private void Awake()
	{
		SFX = GetComponent<AudioSource>();
		soundProp = new SoundProperties();
	}

	private void GetEscapePoints()
	{
		GameObject gameObject = GameObject.Find("EscapePointsHolder");
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			escapePoints.Add(componentsInChildren[i]);
		}
		escapePoints.RemoveAt(0);
	}

	private void GetPatrolPoints()
	{
		GameObject gameObject = GameObject.Find("PatrolPointsHolder");
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			patrolPoints.Add(componentsInChildren[i]);
		}
		patrolPoints.RemoveAt(0);
		MonoBehaviour.print("Patrol points has been find! " + componentsInChildren.Length);
	}

	public void ChangeState(ENEMY_STATE State)
	{
		if (Network.isServer)
		{
			StopAllCoroutines();
			ActiveState = State;
			switch (ActiveState)
			{
			case ENEMY_STATE.ATTACK:
				StartCoroutine(AI_Attack());
				SendMessage("Attack", SendMessageOptions.DontRequireReceiver);
				break;
			case ENEMY_STATE.CHASE:
				StartCoroutine(AI_Chase());
				SendMessage("Chase", SendMessageOptions.DontRequireReceiver);
				break;
			case ENEMY_STATE.PATROL:
				StartCoroutine(AI_Patrol());
				SendMessage("Patrol", SendMessageOptions.DontRequireReceiver);
				break;
			case ENEMY_STATE.ESCAPE:
				StartCoroutine(AI_Escape());
				SendMessage("Escape", SendMessageOptions.DontRequireReceiver);
				break;
			case ENEMY_STATE.CAMERA_BREAK:
				StartCoroutine(AI_CameraBreak());
				break;
			case ENEMY_STATE.RUN:
				StartCoroutine(AI_GoTo());
				break;
			}
		}
	}

	private IEnumerator AI_Escape()
	{
		Agent.Stop();
		soundProp.stepSpeed = 0.2f;
		soundProp.StepDown = false;
		if (ActiveState == ENEMY_STATE.ESCAPE)
		{
			Vector3 randomPosition = Random.onUnitSphere * EscapeDistance;
			randomPosition += ThisTransform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomPosition, out hit, PatrolDistance, 1);
			animatorProperties.speed = 2f;
			Agent.Resume();
			Agent.speed = runSpeed;
			Vector3 pos = escapePoints[Random.Range(0, escapePoints.Count)].position;
			Agent.SetDestination(pos);
			float ArrivalDistance = 2f;
			float TimeOut = 180f;
			float ElapsedTime = 0f;
			while (Vector3.Distance(ThisTransform.position, pos) > ArrivalDistance)
			{
				ElapsedTime += Time.deltaTime;
				yield return null;
			}
			ChangeState(ENEMY_STATE.PATROL);
		}
	}

	private IEnumerator AI_Patrol()
	{
		PC = null;
		Agent.Stop();
		soundProp.stepSpeed = 0.7f;
		soundProp.StepDown = false;
		while (ActiveState == ENEMY_STATE.PATROL)
		{
			Vector3 randomPosition = Random.insideUnitSphere * PatrolDistance;
			randomPosition += ThisTransform.position;
			animatorProperties.speed = 1f;
			NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, PatrolDistance, 1);
			Agent.speed = walkSpeed;
			Agent.Resume();
			Vector3 pos = patrolPoints[Random.Range(0, patrolPoints.Count)].position;
			Agent.SetDestination(pos);
			float ArrivalDistance = 2f;
			float TimeOut = 5f;
			float ElapsedTime = 0f;
			while (Vector3.Distance(ThisTransform.position, pos) > ArrivalDistance)
			{
				yield return null;
			}
		}
	}

	private IEnumerator AI_Chase()
	{
		Agent.Stop();
		soundProp.stepSpeed = 0.2f;
		soundProp.StepDown = false;
		while (ActiveState == ENEMY_STATE.CHASE)
		{
			animatorProperties.speed = 2f;
			Agent.speed = runSpeed;
			Agent.Resume();
			Agent.SetDestination(PlayerTransform.position);
			float DistanceFromPlayer = Vector3.Distance(ThisTransform.position, PlayerTransform.position);
			if (DistanceFromPlayer < AttackDistance)
			{
				ChangeState(ENEMY_STATE.ATTACK);
				break;
			}
			if (DistanceFromPlayer > ChaseDistance)
			{
				ChangeState(ENEMY_STATE.PATROL);
				PC = null;
				break;
			}
			yield return null;
		}
	}

	private IEnumerator AI_Attack()
	{
		Agent.Stop();
		PC.GetComponent<NetworkView>().RPC("Camera_state", RPCMode.All, false);
		PC.GetComponent<NetworkView>().RPC("FreezePlayer", RPCMode.All, false);
		PC.GetComponent<NetworkView>().RPC("HideCurrentWeapon", RPCMode.All);
		GetComponent<NetworkView>().RPC("PlayPlayerAttack", RPCMode.All, Random.Range(0, PlayerAttackRoar.Count));
		animatorProperties.speed = 0f;
		Agent.speed = runSpeed;
		Agent.Stop();
		animatorProperties.bAttack = true;
		float delay = 0f;
		while (delay < 1f)
		{
			PC.GetComponent<NetworkView>().RPC("Focus", RPCMode.All, lookPoint.position);
			delay += Time.deltaTime;
			yield return null;
		}
		GetComponent<NetworkView>().RPC("PlayPlayerKick", RPCMode.All, Random.Range(0, PlayerAttackKicks.Count));
		yield return new WaitForSeconds(0.1f);
		PC.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 10, "Kick");
		PC.GetComponent<NetworkView>().RPC("Camera_state", RPCMode.All, true);
		PC.GetComponent<NetworkView>().RPC("FreezePlayer", RPCMode.All, true);
		animatorProperties.bAttack = false;
		MonoBehaviour.print("attack");
		PC = null;
		ChangeState(ENEMY_STATE.ESCAPE);
	}

	private IEnumerator AI_CameraBreak()
	{
		Agent.Stop();
		Vector3 pos = MC.transform.TransformPoint(Vector3.forward * 2f);
		NavMeshHit hit;
        NavMesh.SamplePosition(pos, out hit, 5f, 1);
		Vector3 dest = hit.position;
		Agent.Resume();
		Agent.SetDestination(dest);
		animatorProperties.speed = 2f;
		Agent.speed = runSpeed;
		soundProp.stepSpeed = 0.2f;
		soundProp.StepDown = false;
		float ArrivalDistance = 2f;
		float DistanceFromCamera = Vector3.Distance(ThisTransform.position, dest);
		MonoBehaviour.print(DistanceFromCamera);
		animatorProperties.speed = 2f;
		Agent.speed = runSpeed;
		MonoBehaviour.print("inst");
		while (DistanceFromCamera > 1.5f)
		{
			DistanceFromCamera = Vector3.Distance(ThisTransform.position, dest);
			MonoBehaviour.print("run");
			yield return null;
		}
		GetComponent<NetworkView>().RPC("PlayCameraBreak", RPCMode.All);
		Agent.Stop();
		animatorProperties.speed = 0f;
		animatorProperties.bAttack = true;
		Vector3 viewPos = MC.transform.position;
		viewPos.y = ThisTransform.transform.position.y;
		float delay = 0f;
		while (delay < 2f)
		{
			delay += Time.deltaTime;
			base.transform.LookAt(viewPos);
			yield return null;
		}
		MonoBehaviour.print("attacl anim");
		MonoBehaviour.print("set down");
		animatorProperties.bAttack = false;
		MC.GetComponent<NetworkView>().RPC("SetUnsetupState", RPCMode.All);
		ChangeState(ENEMY_STATE.ESCAPE);
	}

	private IEnumerator AI_GoTo()
	{
		Agent.Stop();
		soundProp.stepSpeed = 0.2f;
		soundProp.StepDown = false;
		Agent.SetDestination(runPoint);
		Agent.Resume();
		animatorProperties.speed = 2f;
		Agent.speed = runSpeed;
		while (Vector3.Distance(ThisTransform.position, runPoint) > 2f)
		{
			yield return null;
		}
		ChangeState(ENEMY_STATE.PATROL);
	}

	public void OnObjectEnter(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<MP_PlayerController>())
		{
			if (PC == null)
			{
				PC = info.gameObject.GetComponent<MP_PlayerController>();
				PlayerTransform = PC.transform;
				ChangeState(ENEMY_STATE.CHASE);
			}
			else if (ActiveState != ENEMY_STATE.ATTACK && ActiveState != ENEMY_STATE.CHASE)
			{
				PC = info.gameObject.GetComponent<MP_PlayerController>();
				PlayerTransform = PC.transform;
				ChangeState(ENEMY_STATE.CHASE);
			}
		}
		if ((bool)info.gameObject.GetComponent<MP_RecordCamera_obj>() && PC == null && info.gameObject.GetComponent<MP_RecordCamera_obj>().cameraState == MP_RecordCamera_obj.Camera_state.Setup)
		{
			MC = info.gameObject.GetComponent<MP_RecordCamera_obj>();
			ChangeState(ENEMY_STATE.CAMERA_BREAK);
		}
	}

	public void OnObjectStay(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<MP_PlayerController>() && PC == null && ActiveState == ENEMY_STATE.PATROL)
		{
			PC = info.gameObject.GetComponent<MP_PlayerController>();
			PlayerTransform = PC.transform;
			ChangeState(ENEMY_STATE.CHASE);
		}
	}

	private void Update()
	{
		UI_Health.health = Health;
		animatorProperties.animator.SetFloat("Speed", animatorProperties.speed);
		animatorProperties.animator.SetBool("bPlayerAttack", animatorProperties.bAttack);
		if (Network.isServer && !soundProp.StepDown && animatorProperties.speed > 0f)
		{
			GetComponent<NetworkView>().RPC("PlayFootStep", RPCMode.All, soundProp.stepSpeed);
		}
	}

	[RPC]
	private void PlayPlayerAttack(int id)
	{
		SFX.PlayOneShot(PlayerAttackRoar[id]);
	}

	[RPC]
	private void PlayPlayerKick(int id)
	{
		SFX.PlayOneShot(PlayerAttackKicks[id]);
	}

	[RPC]
	private void TakeDamage(int damageTaken)
	{
		if (ActiveState != ENEMY_STATE.ATTACK && ActiveState != ENEMY_STATE.ESCAPE && ActiveState != ENEMY_STATE.CHASE)
		{
			ChangeState(ENEMY_STATE.ESCAPE);
		}
		Health -= damageTaken;
		if (Health <= 0 && Network.isServer)
		{
			MP_LevelManager.instance.FinishGame();
			Object.Instantiate(rakeDeadPrefab, base.transform.position, base.transform.rotation);
			Network.Destroy(base.gameObject);
		}
	}

	[RPC]
	private void PlayCameraBreak()
	{
		SFX.PlayOneShot(cameraAttack);
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
