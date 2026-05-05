using System.Collections;
using UnityEngine;

namespace RPG.Combat
{

    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        private float respawnDelay = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnDelay));
                //Destroy(gameObject);
            }
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
