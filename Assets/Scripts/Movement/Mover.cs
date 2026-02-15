using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        private Fighter fighter;

        private const string ForwardSpeed = "forwardSpeed";

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
        }


        void Update()
        {
            UpdateAnimator();
        }
        public void StartMoving(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            fighter.CancelAttack();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 position)
        {
            agent.destination = position;
            agent.isStopped = false;

        }

        public void Stop()
        {
            agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(ForwardSpeed, speed);
        }
    }
}