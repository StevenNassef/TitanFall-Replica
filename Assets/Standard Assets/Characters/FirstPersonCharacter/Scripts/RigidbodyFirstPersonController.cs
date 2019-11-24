using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
            public float CrouchMultiplier = 0.5f;   // Speed when sprinting

            public int MaxNumberOfJumps = 2;
            public int RemainingJumps;
            public KeyCode RunKey = KeyCode.LeftShift;
            public KeyCode CrouchKey = KeyCode.C;
            public float JumpForce = 30f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
            private bool m_Crouching;
            private bool LockCrouchKey;
#endif

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

                if (Input.GetKey(CrouchKey))
                {
                    if (!LockCrouchKey)
                    {
                        m_Crouching = !m_Crouching;
                        LockCrouchKey = true;
                    }
                }
                else
                {
                    LockCrouchKey = false;
                }

                // Cancel Crouching if the player pressed the crouch button
                if (m_Running)
                {
                    m_Crouching = false;
                }
            }


            public bool Running
            {
                get { return m_Running; }
            }
            public bool Crouching
            {
                get { return m_Crouching; }
            }

            public void ResetCrouch()
            {
                m_Crouching = false;
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
            [Range(0, 1)] public float gravityModifier;
        }

        public enum PlayerMovementState
        {
            Walking, Running, Jumping, WallRunning
        }


        public Camera cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal, m_WallContactNormal;
        private bool m_Jump, m_WallRun, m_PreviouslyGrounded, m_Jumping, m_WallRunning, m_IsGrounded, m_PreviouslyWalled, m_IsWalled;
        private PlayerMovementState currentMovementState;
        private PlayerMovementState previousMovementState;
        private float downVelocity = 0;


        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
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

            if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
            {
                m_Jump = true;
            }

            movementSettings.UpdateInput();
        }


        private void FixedUpdate()
        {
            GroundCheck();
            WallCheck();
            Vector2 input = GetInput();

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
                    else if (movementSettings.Crouching)
                    {
                        TargetSpeed *= movementSettings.CrouchMultiplier;
                    }
                }

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                if (m_WallRunning)
                {
                    desiredMove = Vector3.ProjectOnPlane(desiredMove, m_WallContactNormal).normalized;
                }
                else
                {
                    desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;
                }


                desiredMove = desiredMove * TargetSpeed;

                if (m_RigidBody.velocity.sqrMagnitude <
                    (TargetSpeed * TargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                }
            }
            if (movementSettings.RemainingJumps > 0)
            {
                if (m_Jump)
                {
                    movementSettings.ResetCrouch();
                    
                    Vector3 jumpDirection = Vector3.up;
                    Debug.Log(m_WallRunning);
                    if (m_WallRunning)
                    {
                        jumpDirection = (m_WallContactNormal + Vector3.up).normalized;
                        m_WallRunning = false;
                        Debug.Log("WallJump");
                    }
                    // m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);

                    m_RigidBody.AddForce(jumpDirection * movementSettings.JumpForce, ForceMode.Impulse);
                    m_Jumping = true;
                    downVelocity = 0;
                    movementSettings.RemainingJumps--;
                }
            }

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else //The Player is Airborn.
            {
                downVelocity += Physics.gravity.y * Time.fixedDeltaTime * advancedSettings.gravityModifier;
                m_RigidBody.drag = 2.5f;

                if (m_IsWalled)
                {
                    if (!m_WallRunning && movementSettings.Running && (Mathf.Abs(input.y) > 0.1f))
                    {
                        m_WallRunning = true;
                        movementSettings.RemainingJumps = 1;
                    }

                }
                else
                {
                    m_WallRunning = false;
                }

                if (m_PreviouslyGrounded && !(m_Jumping || m_WallRunning))
                {
                    StickToGroundHelper();
                }
                else if (m_WallRunning)
                {
                    downVelocity = 0;
                    StickToWallHelper();
                    //Check if there is any of the wall run conditions are broken
                    if ((Mathf.Abs(input.y) < 0.1f || !movementSettings.Running))
                    {
                        m_WallRunning = false;
                    }
                }

                //Add gravity effect
                m_RigidBody.velocity += Vector3.up * downVelocity;
            }
            m_Jump = false;
            m_WallRun = false;
        }

        private void SwitchMovementState(PlayerMovementState newState)
        {
            previousMovementState = currentMovementState;
            currentMovementState = newState;
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

        private void StickToWallHelper()
        {
            m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, m_WallContactNormal);
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
                m_Jumping = false;
                movementSettings.RemainingJumps = movementSettings.MaxNumberOfJumps;
                downVelocity = 0;
            }
        }

        private void WallCheck()
        {
            m_PreviouslyWalled = m_IsWalled;
            RaycastHit hitInfo;
            // Vector3 direction = transform.position - (m_WallContactNormal);
            // if (!m_PreviouslyWalled)
            // {
            // }

            Vector3[] directions = {
                transform.forward,
                -transform.forward,
                transform.right,
                -transform.right
            };


            Vector3 HeightFactor = Vector3.up * m_Capsule.height * 0.1f;
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 direction = directions[i];
                Debug.DrawRay(transform.position, (direction) * (1 + m_Capsule.radius + advancedSettings.wallCheckDistance), Color.red, Time.fixedDeltaTime);
                if (Physics.CapsuleCast(transform.position + HeightFactor, transform.position - HeightFactor, m_Capsule.radius - advancedSettings.wallCheckRadius,
                    direction, out hitInfo, advancedSettings.wallCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_IsWalled = true;
                    m_WallContactNormal = hitInfo.normal;
                    break;
                }
                else
                {
                    m_IsWalled = false;
                    m_WallContactNormal = Vector3.up;
                }
            }
        }

        private void UpdateRotationWhileWallRunning(Transform inputTransform)
        {
            Vector3 newUpVector = (m_WallContactNormal + Vector3.up).normalized;
            inputTransform.rotation = Quaternion.LookRotation(inputTransform.forward, newUpVector);
        }
    }
}
