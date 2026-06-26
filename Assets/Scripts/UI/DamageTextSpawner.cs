using RPG.Attributes;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        Health health;

        void Start()
        {
            health = GetComponentInParent<Health>();
            health.OnTakeDamage += Spawn;
        }

        void OnDestroy()
        {
            if (health != null)
                health.OnTakeDamage -= Spawn;
        }

        void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate(damageTextPrefab, transform);
            instance.SetText(damageAmount);
            instance.PopUpAnimation();
        }
    }
}
