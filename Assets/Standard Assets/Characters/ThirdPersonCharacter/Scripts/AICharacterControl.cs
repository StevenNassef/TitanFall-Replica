using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public int detectionRadius = 5;

        public Patrol patrolScript;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            if (target != null)
                CheckPlayer();
            
            
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void CheckPlayer()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, target.position - transform.position, out hitInfo, detectionRadius))
        {
                //target = hitInfo.collider.gameObject.transform;
                Debug.Log("Patrol should be disabled");
                agent.SetDestination(target.position);
                patrolScript.enabled = false;
                if (agent.remainingDistance > agent.stoppingDistance){
                    character.Move(agent.desiredVelocity, false, false);
                }
                else
                    character.Move(Vector3.zero, false, false);
                return;
        }
        patrolScript.enabled = true;
        character.Move(agent.desiredVelocity, false, false);
    }
    }
}
