using RPG.Attributes;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer healthBarFill;
    Health health;

    float maxWidth;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        maxWidth = healthBarFill.size.x;
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
        float fillFraction = Mathf.Clamp01(percentage / 100f);
        healthBarFill.size = new Vector2(maxWidth * fillFraction, healthBarFill.size.y);

        if (percentage <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
