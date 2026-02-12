using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private const string ForwardSpeed = "forwardSpeed"; 

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            agent.SetDestination(hit.point);
        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat(ForwardSpeed, speed);
    }
}
