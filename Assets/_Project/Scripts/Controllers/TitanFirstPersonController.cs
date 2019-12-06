using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class TitanFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
            public KeyCode RunKey = KeyCode.LeftShift;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;
            private bool m_Running;

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
                if (input == Vector2.zero) return;
                if (input.x > 0 || input.x < 0)
                {
                    //strafe
                    CurrentTargetSpeed = StrafeSpeed;
                }
                if (input.y < 0)
                {
                    //backwards
                    CurrentTargetSpeed = BackwardSpeed;
                }
                if (input.y > 0)
                {
                    //forwards
                    //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                    CurrentTargetSpeed = ForwardSpeed;
                }
            }

            public void UpdateInput()
            {
                if (Input.GetKey(RunKey))
                {
                    m_Running = true;
                }
                else
                {
                    m_Running = false;
                }
            }


            public bool Running
            {
                get { return m_Running; }
            }
        }


        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float wallCheckDistance = 0.05f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float wallCheckRadius = 0.05f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float stickToWallHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
            [Range(0, 4)] public float gravityModifier;
        }

        public Camera cam;
        public Transform cameraHolderTransform;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();
        [Header("SFX")]
        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private AudioSource m_BackGroundAudioSource;
        [Space(10)]
        [SerializeField] private AudioClip m_JumpBackGroundSFX;
        [SerializeField] private AudioClip m_LandingSFX;
        [SerializeField] private AudioClip m_WalkingSFX;


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation, m_targetCameraHolderAngle;
        private Vector3 m_GroundContactNormal, m_WallContactNormal;
        private bool m_PreviouslyGrounded, m_IsGrounded;

        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }
        public bool Running
        {
            get
            {
#if !MOBILE_INPUT
                return movementSettings.Running;
#else
	            return false;
#endif
            }
        }
        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init(transform, cam.transform);
        }


        private void Update()
        {
            RotateView();
            movementSettings.UpdateInput();
        }


        private void FixedUpdate()
        {
            GroundCheck();
            Vector2 input = GetInput();
            float downVelocity = 0;

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                //Target Speed
                float TargetSpeed = movementSettings.CurrentTargetSpeed;
                if (m_IsGrounded)
                {
                    if (movementSettings.Running)
                    {
                        TargetSpeed *= movementSettings.RunMultiplier;
                    }
                }

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;


                desiredMove = desiredMove * TargetSpeed;

                if (m_RigidBody.velocity.sqrMagnitude <
                    (TargetSpeed * TargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                }
            }


            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                // if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                // {
                //     m_RigidBody.Sleep();
                // }
                if (input.x > 0 || input.y > 0)
                {
                    PlayWalkingSound();
                }
            }
            else //The Player is Airborn.
            {

                downVelocity = Physics.gravity.y * Time.fixedDeltaTime * advancedSettings.gravityModifier;
                m_RigidBody.drag = 1f;

                if (m_PreviouslyGrounded)
                {
                    StickToGroundHelper();
                }
                //Add gravity effect
                m_RigidBody.velocity += Vector3.up * downVelocity;
            }
        }

        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }


        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height / 2f) - m_Capsule.radius * (1.0f - advancedSettings.shellOffset)) +
                                   advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }
        private Vector2 GetInput()
        {

            Vector2 input = new Vector2
            {
                x = CrossPlatformInputManager.GetAxis("Horizontal"),
                y = CrossPlatformInputManager.GetAxis("Vertical")
            };
            movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation(transform, cam.transform);

            if (m_IsGrounded || advancedSettings.airControl)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
            }
        }

        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height / 2f) - m_Capsule.radius * (1.0f - advancedSettings.shellOffset)) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && m_IsGrounded)
            {
                PlayLandingSFX();
            }
            PlayBackGroundSFX();
        }

        private void PlayBackGroundSFX()
        {
            if (!m_IsGrounded && m_PreviouslyGrounded)
            {
                m_BackGroundAudioSource.clip = m_JumpBackGroundSFX;
                m_BackGroundAudioSource.Play();
            }
            else if (m_IsGrounded)
            {
                m_BackGroundAudioSource.Stop();
            }
        }

        private void PlayWalkingSound()
        {
            if (m_AudioSource.clip == m_WalkingSFX)
                return;
            m_AudioSource.clip = m_WalkingSFX;
            m_BackGroundAudioSource.Play();
        }

        private void PlayLandingSFX()
        {
            m_AudioSource.clip = m_LandingSFX;
            m_AudioSource.Play();
        }
    }
}
