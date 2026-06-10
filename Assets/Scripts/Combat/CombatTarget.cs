using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using UnityEngine.InputSystem;


namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Mouse.current.rightButton.isPressed)
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }

            return true;
        }


    }
}
