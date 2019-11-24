using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        // [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_JumpStrafSpeedFactor;
        [SerializeField] private float m_WallRunningSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private int m_NumberOfJumps = 1;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private float m_GravityMultiplierWhileWallRun;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private AudioClip m_WallRunningSound;           // the sound played when character is wall running.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private float m_RemainingJumps;
        // private bool m_WallRunning;
        private bool m_WallRun;
        private bool m_WallRunLock; //Make sure the player has left the wall before allowing another wallrun
        private bool sprintKey;
        private PlayerMovementState currentMovementState;
        private PlayerMovementState previousMovementState;
        private float m_currentSpeed;
        private float m_previousSpeed;
        private ControllerColliderHit m_currentWallCollider;
        private AudioSource m_AudioSource;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            currentMovementState = PlayerMovementState.Walking;
            previousMovementState = PlayerMovementState.Walking;
            m_RemainingJumps = m_NumberOfJumps;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump && m_RemainingJumps > 0)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                currentMovementState = PlayerMovementState.Walking;
                previousMovementState = PlayerMovementState.Walking;
                m_RemainingJumps = m_NumberOfJumps;
            }
            if (!m_CharacterController.isGrounded && currentMovementState != PlayerMovementState.Jumping && m_PreviouslyGrounded)
            {
                Debug.Log("Wierd");
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            // Debug.Log(hitInfo.normal);



            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;
                m_MoveDir.y = -m_StickToGroundForce;

            }
            else
            {


                if (currentMovementState == PlayerMovementState.WallRunning)
                {
                    Vector3 newMove = Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime + m_MoveDir;
                    m_MoveDir = newMove.y > 0 ? newMove : m_MoveDir;
                }
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
            }

            if (m_RemainingJumps > 0)
            {
                if (m_Jump)
                {

                    if (currentMovementState == PlayerMovementState.WallRunning)
                    {
                        Vector3 jumpVector = ((Vector3.up + m_currentWallCollider.normal).normalized )* m_JumpSpeed;
                        m_MoveDir += jumpVector;
                        Debug.Log(jumpVector + " " + m_MoveDir);
                    }
                    else
                    {
                        m_MoveDir.y = m_JumpSpeed;
                    }
                    PlayJumpSound();
                    m_Jump = false;
                    SwitchMovementState(PlayerMovementState.Jumping);
                    m_RemainingJumps--;
                }
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            if (m_WallRunLock && m_CollisionFlags != CollisionFlags.Sides)
            {
                m_WallRunLock = false;
            }

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                float newSpeed = speed;
                switch (currentMovementState)
                {
                    case PlayerMovementState.Walking:
                        newSpeed = 1;
                        break;

                    case PlayerMovementState.Running:
                        newSpeed = m_RunstepLenghten;
                        break;

                    default:
                        newSpeed = 1;
                        break;
                }
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * newSpeed)) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                float newSpeed = speed;
                switch (currentMovementState)
                {
                    case PlayerMovementState.Walking:
                        newSpeed = 1;
                        break;

                    case PlayerMovementState.Running:
                        newSpeed = m_RunstepLenghten;
                        break;

                    default:
                        newSpeed = 1;
                        break;
                }
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * newSpeed));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");



#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            sprintKey = Input.GetKey(KeyCode.LeftShift);




#endif
            // set the desired speed to be walking or running
            // speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            float newSpeed = 0;
            bool Kick = false;
            switch (currentMovementState)
            {
                case PlayerMovementState.Walking:
                    if (sprintKey)
                    {
                        SwitchMovementState(PlayerMovementState.Running);
                        newSpeed = m_RunSpeed;
                        Kick = true;
                    }
                    else
                    {
                        newSpeed = m_WalkSpeed;
                    }
                    break;

                case PlayerMovementState.Running:
                    if (!sprintKey)
                    {
                        SwitchMovementState(PlayerMovementState.Walking);
                        newSpeed = m_WalkSpeed;
                        Kick = true;
                    }
                    else
                    {
                        newSpeed = m_RunSpeed;
                    }
                    break;

                case PlayerMovementState.Jumping:
                    newSpeed = m_currentSpeed;
                    break;

                case PlayerMovementState.WallRunning:
                    if (!sprintKey || m_CollisionFlags != CollisionFlags.Sides)
                    {
                        newSpeed = m_previousSpeed;
                        SwitchMovementState(previousMovementState);
                    }
                    else
                    {
                        newSpeed = m_WallRunningSpeed;
                    }
                    break;

                default:
                    newSpeed = m_WalkSpeed;
                    break;
            }
            speed = SwitchSpeed(newSpeed);
            // Debug.Log(speed);
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (Kick && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                if (currentMovementState == PlayerMovementState.Walking && previousMovementState == PlayerMovementState.Running)
                {
                    StartCoroutine(m_FovKick.FOVKickDown());
                }
                else if (currentMovementState == PlayerMovementState.Running && previousMovementState == PlayerMovementState.Walking)
                {
                    StartCoroutine(m_FovKick.FOVKickUp());
                }
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            //Check if we have hit a wall on the sides.
            if (!m_WallRunLock && m_CollisionFlags == CollisionFlags.Sides && sprintKey && currentMovementState == PlayerMovementState.Jumping && hit.collider.CompareTag("Wall"))
            {
                m_WallRunLock = true;
                m_currentWallCollider = hit;
                m_RemainingJumps = m_NumberOfJumps;
                SwitchMovementState(PlayerMovementState.WallRunning);
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        private void SwitchMovementState(PlayerMovementState newState)
        {
            previousMovementState = currentMovementState;
            currentMovementState = newState;
        }

        private float SwitchSpeed(float newSpeed)
        {
            if (m_currentSpeed != newSpeed)
            {
                m_previousSpeed = m_currentSpeed;
                m_currentSpeed = newSpeed;
            }
            return m_currentSpeed;
        }
    }

}

public enum PlayerMovementState
{
    Walking, Running, Jumping, WallRunning
}
