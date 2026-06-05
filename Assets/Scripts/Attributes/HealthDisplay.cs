using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI healthText;
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnHealthChanged += UpdateHealthDisplay;
        }

        private void OnDisable()
        {
            health.OnHealthChanged -= UpdateHealthDisplay;
        }

        private void UpdateHealthDisplay(float percentage)
        {
            healthText.text = $"% {percentage}";
        }
    }
}
