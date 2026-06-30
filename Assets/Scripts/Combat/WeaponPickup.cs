using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Combat
{

    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] private float healthToRestore = 50f;
        private float respawnDelay = 2f;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Mouse.current.rightButton.isPressed)
            {
                callingController.GetComponent<Mover>().StartMoving(transform.position);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PickUp(other.gameObject);

                //PickUp(other.GetComponent<Fighter>());
                //other.GetComponent<Fighter>().EquipWeapon(weapon);
                //StartCoroutine(HideForSeconds(respawnDelay));
            }
        }

        private void PickUp(GameObject subject)
        {
            if(weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }

            if(healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }

            StartCoroutine(HideForSeconds(respawnDelay));
        }

        private IEnumerator HideForSeconds(float delay)
        {
            SetVisibility(false);
            yield return new WaitForSeconds(delay);
            SetVisibility(true);
        }

        private void SetVisibility(bool isVisible)
        {
            GetComponent<Collider>().enabled = isVisible;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isVisible);
            }
        }
    }
}
