using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    Ray lastRay;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            MoveToCursor();
        }
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
}
