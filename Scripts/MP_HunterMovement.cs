// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_HunterMovement
using System.Collections;
using UnityEngine;

public class MP_HunterMovement : MonoBehaviour
{
	public float speed = 6f;

	public float walkSpeed = 6f;

	public float runSpeed = 2f;

	public float jumpSpeed = 8f;

	public float gravity = 20f;

	private Vector3 moveDirection = Vector3.zero;

	public float DirectionDampTime = 0.25f;

	public MP_FPSController fpsController;

	public float animationSpeedParam;

	private bool StepDown;

	public float StepSpeed;

	public AudioSource audioSource;

	public AudioClip[] FootstepSound;

	protected bool bCanMove;

	public void Start()
	{
		FreezePlayer(type: true);
	}

	[RPC]
	public void FreezePlayer(bool type)
	{
		bCanMove = type;
	}

	private void Awake()
	{
	}

	private void Update()
	{
		CharacterController component = GetComponent<CharacterController>();
		if (component.isGrounded)
		{
			float axis = Input.GetAxis("Horizontal");
			float num = Input.GetAxis("Vertical");
			fpsController.horizontalInput = axis;
			fpsController.verticalInput = num;
			Vector2 vector = new Vector2(axis, num);
			if (vector.sqrMagnitude > 1f)
			{
				vector.Normalize();
			}
			fpsController.speed = vector.sqrMagnitude + vector.y * Input.GetAxis("Run");
			if (!bCanMove)
			{
				return;
			}
			if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && !StepDown)
			{
				GetComponent<NetworkView>().RPC("StartPlay", RPCMode.All, StepSpeed);
			}
			if (Input.GetButton("Run") && num >= 1f)
			{
				speed = runSpeed;
				num *= 2f;
				StepSpeed = 0.3f;
			}
			else
			{
				StepSpeed = 0.8f;
				speed = walkSpeed;
			}
			moveDirection = new Vector3(axis, 0f, num);
			moveDirection = base.transform.TransformDirection(moveDirection);
			moveDirection *= speed;
		}
		moveDirection.y -= gravity * Time.deltaTime;
		component.Move(moveDirection * Time.deltaTime);
	}

	[RPC]
	private void StartPlay(float time)
	{
		StartCoroutine(FootStep(time));
	}

	private IEnumerator FootStep(float stepDelay)
	{
		StepDown = true;
		audioSource.PlayOneShot(FootstepSound[0]);
		yield return new WaitForSeconds(stepDelay);
		StepDown = false;
	}
}
