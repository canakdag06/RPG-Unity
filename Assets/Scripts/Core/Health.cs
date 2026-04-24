using Newtonsoft.Json.Linq;
using RPG.Saving;
using System;
using UnityEngine;


namespace RPG.Core
{
    public class Health : MonoBehaviour, IJsonSaveable, ISaveable
    {
        [SerializeField] private float health = 100f;

        public event Action OnDie;

        public bool IsDead => isDead;


        private const string dieTrigger = "die";

        private bool isDead = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0f);

            if (health == 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger(dieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();

            OnDie.Invoke();
        }

        //public object CaptureState()
        //{
        //    return health;
        //}

        //public void RestoreState(object state)
        //{
        //    health = (float)state;

        //    if (health <= 0f)
        //    {
        //        Die();
        //    }
        //}

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(health/*.value*/);
        }

        public void RestoreFromJToken(JToken state)
        {
            health/*.value*/ = state.ToObject<float>();
            //UpdateState();
        }
    }
}