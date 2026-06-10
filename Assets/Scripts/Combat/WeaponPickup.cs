using RPG.Control;
using RPG.Movement;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Combat
{

    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        private float respawnDelay = 2f;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Mouse.current.rightButton.isPressed)
            {
                callingController.GetComponent<Mover>().StartMoving(transform.position);
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PickUp(other.GetComponent<Fighter>());
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnDelay));
                //Destroy(gameObject);
            }
        }

        private void PickUp(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
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
