using GameDevTV.Inventories;
using RPG.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (pickup.CanBePickedUp() && Mouse.current.rightButton.isPressed)
            {
                callingController.GetComponent<Mover>().StartMoving(transform.position);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }

            return CursorType.CannotPickup;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && pickup.CanBePickedUp())
            {
                pickup.PickupItem();
            }
        }
    }
}
