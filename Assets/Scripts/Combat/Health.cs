using UnityEngine;


namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private const string dieTrigger = "die";

        private bool isDead = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0f);

            if (health == 0f && !isDead)
            {
                GetComponent<Animator>().SetTrigger(dieTrigger);
                isDead = true;
            }
        }

    }
}