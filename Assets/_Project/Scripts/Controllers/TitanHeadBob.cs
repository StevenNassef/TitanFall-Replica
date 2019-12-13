using System;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class TitanHeadBob : MonoBehaviour
    {
        public Camera fpsCamera;
        public CurveControlledBob motionBob = new CurveControlledBob();
        public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
        public TitanFirstPersonController titanFirstPersonController;
        public float StrideInterval;
        [Range(0f, 1f)] public float RunningStrideLengthen;

       // private CameraRefocus m_CameraRefocus;
        private bool m_PreviouslyGrounded;
        private bool m_PreviouslyWallRunning;
        private Vector3 m_OriginalCameraPosition;


        private void Start()
        {
            motionBob.Setup(fpsCamera, StrideInterval);
            m_OriginalCameraPosition = fpsCamera.transform.localPosition;
       //     m_CameraRefocus = new CameraRefocus(Camera, transform.root.transform, Camera.transform.localPosition);
        }


        private void Update()
        {
          //  m_CameraRefocus.GetFocusPoint();
            Vector3 newCameraPosition;
            if (titanFirstPersonController.Velocity.magnitude > 0.1f && titanFirstPersonController.Grounded)
            {
                fpsCamera.transform.localPosition = motionBob.DoHeadBob(titanFirstPersonController.Velocity.magnitude*(titanFirstPersonController.Running ? RunningStrideLengthen : 1f));
                newCameraPosition = fpsCamera.transform.localPosition;
                newCameraPosition.y = fpsCamera.transform.localPosition.y - jumpAndLandingBob.Offset();
            }
            else
            {
                newCameraPosition = fpsCamera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
            }
            fpsCamera.transform.localPosition = newCameraPosition;

            if ((!m_PreviouslyGrounded && titanFirstPersonController.Grounded))
            {
                StartCoroutine(jumpAndLandingBob.DoBobCycle());
            }

            m_PreviouslyGrounded = titanFirstPersonController.Grounded;
          //  m_CameraRefocus.SetFocusPoint();
        }
    }
}
