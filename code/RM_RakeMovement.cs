// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// RM_RakeMovement
using System.Collections;
using UnityEngine;

public class RM_RakeMovement : MonoBehaviour
{
	[SerializeField]
	private bool m_IsWalking;

	[SerializeField]
	private float m_WalkSpeed;

	[SerializeField]
	private float m_RunSpeed;

	public bool isGrounded;

	public MP_RPSController rpsController;

	public float groundMovementSpeed = 6f;

	public float groundRunSpeed = 10f;

	public float flySpeed = 3f;

	public float speed = 6f;

	public float jumpSpeed = 8f;

	public float gravity = 20f;

	public Vector3 moveDirection = Vector3.zero;

	public Animator weaponAnim;

	public Animator cameraAnim;

	public float m_StickToGroundForce;

	public bool BCanJump;

	public AudioSource audioSource;

	private Vector2 m_Input;

	public AudioClip[] FootstepSound;

	private bool StepDown;

	private float StepSpeed;

	public CharacterController m_CharacterController;

	public bool isGround()
	{
		if (Physics.SphereCast(base.transform.position, GetComponent<CharacterController>().radius, Vector3.down, out var _, GetComponent<CharacterController>().height / 2f))
		{
			return true;
		}
		return false;
	}

	private void Update()
	{
		weaponAnim.SetBool("Grounded", m_CharacterController.isGrounded);
		rpsController.verticalInput = m_Input.y;
		rpsController.horizontalInput = m_Input.x;
		rpsController.grounded = m_CharacterController.isGrounded;
		rpsController.speed = m_Input.sqrMagnitude + m_Input.y * Input.GetAxis("Run");
	}

	private void FixedUpdate()
	{
		GetInput(out var num);
		if (m_Input.sqrMagnitude > 1f)
		{
			m_Input.Normalize();
		}
		float value = m_Input.sqrMagnitude + Input.GetAxis("Run");
		Vector3 vector = base.transform.forward * m_Input.y + base.transform.right * m_Input.x;
		Physics.SphereCast(base.transform.position, m_CharacterController.radius, Vector3.down, out var hitInfo, m_CharacterController.height / 2f);
		vector = Vector3.ProjectOnPlane(vector, hitInfo.normal).normalized;
		moveDirection.x = vector.x * num;
		moveDirection.z = vector.z * num;
		cameraAnim.SetBool("Grounded", m_CharacterController.isGrounded);
		cameraAnim.SetFloat("Speed", value);
		weaponAnim.SetFloat("Speed", value);
		weaponAnim.SetBool("Grounded", m_CharacterController.isGrounded);
		if (m_CharacterController.isGrounded)
		{
			if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && !StepDown)
			{
				GetComponent<NetworkView>().RPC("StartPlay", RPCMode.All, StepSpeed);
			}
			if (Input.GetButton("Run") && m_Input.y >= 1f)
			{
				StepSpeed = 0.3f;
			}
			else
			{
				StepSpeed = 0.8f;
			}
			moveDirection.y = 0f - m_StickToGroundForce;
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		else
		{
			moveDirection += Physics.gravity * gravity * Time.fixedDeltaTime;
		}
		m_CharacterController.Move(moveDirection * Time.fixedDeltaTime);
		if (m_CharacterController.collisionFlags == CollisionFlags.Sides && Input.GetButton("Jump"))
		{
			moveDirection.y = jumpSpeed;
		}
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

	private void GetInput(out float speed)
	{
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
		speed = ((!m_IsWalking) ? m_RunSpeed : m_WalkSpeed);
		m_Input = new Vector2(axis, axis2);
		if (m_Input.sqrMagnitude > 1f)
		{
			m_Input.Normalize();
		}
	}
}
