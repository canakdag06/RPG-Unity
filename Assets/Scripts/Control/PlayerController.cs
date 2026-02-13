using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover mover;

        void Update()
        {
            if (InteractWithCombat()) return;
            if (SetDestination()) return;
            Debug.Log("NOTHING");
        }

        private bool SetDestination()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);

            if (hasHit)
            {
                if (Mouse.current.rightButton.isPressed)
                {
                    mover.StartMoving(hit.point);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Mouse.current.rightButton.isPressed)
                {
                    GetComponent<Fighter>().Attack(target);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
    }
}