using System.Collections;
using GameDevTV.Inventories;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] float pickupDistance = 2f;

        Pickup pickup;
        Coroutine pickupRoutine;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!pickup.CanBePickedUp() || !Mouse.current.rightButton.wasPressedThisFrame)
            {
                return true;
            }

            Transform player = callingController.transform;
            if (IsWithinPickupRange(player))
            {
                pickup.PickupItem();
                return true;
            }

            Mover mover = callingController.GetComponent<Mover>();
            if (!mover.CanMoveTo(transform.position))
            {
                return true;
            }

            if (pickupRoutine != null)
            {
                StopCoroutine(pickupRoutine);
            }

            pickupRoutine = StartCoroutine(PickupWhenInRange(mover, player));
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

        bool IsWithinPickupRange(Transform player)
        {
            return Vector3.Distance(player.position, transform.position) <= pickupDistance;
        }

        IEnumerator PickupWhenInRange(Mover mover, Transform player)
        {
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            mover.StartMoving(transform.position);

            while (pickup != null && pickup.CanBePickedUp())
            {
                if (IsWithinPickupRange(player))
                {
                    pickup.PickupItem();
                    yield break;
                }

                if (HasChangedDestination(agent))
                {
                    yield break;
                }

                yield return null;
            }
        }

        bool HasChangedDestination(NavMeshAgent agent)
        {
            if (agent.pathPending)
            {
                return false;
            }

            return Vector3.Distance(agent.destination, transform.position) > pickupDistance;
        }
    }
}
