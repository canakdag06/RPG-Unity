using RPG.Attributes;
using TMPro;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] TextMeshPro healthText;
    Health health;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
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
        healthText.text = $"{percentage:0}";
        if(percentage <= 0)
        {
            healthText.gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
