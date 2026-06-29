using RPG.Attributes;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer healthBarFill;
    [SerializeField] SpriteRenderer healthBarLazyFill;
    [SerializeField] float lazyFillSpeed = 2f;
    [SerializeField] float lazySnapThreshold = 0.001f;

    Health health;
    Transform mainCameraTransform;

    float maxWidth;
    float targetFillFraction;
    bool isLerping;
    bool isDying;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        mainCameraTransform = Camera.main.transform;
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
        targetFillFraction = Mathf.Clamp01(percentage / 100f);
        healthBarFill.size = new Vector2(maxWidth * targetFillFraction, healthBarFill.size.y);

        if (percentage <= 0f)
        {
            isDying = true;
        }

        isLerping = true;
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCameraTransform);

        if (!isLerping) return;

        float currentFraction = healthBarLazyFill.size.x / maxWidth;

        if (Mathf.Abs(targetFillFraction - currentFraction) <= lazySnapThreshold)
        {
            healthBarLazyFill.size = new Vector2(maxWidth * targetFillFraction, healthBarLazyFill.size.y);
            isLerping = false;

            if (isDying)
            {
                gameObject.SetActive(false);
            }

            return;
        }

        float newFraction = Mathf.Lerp(currentFraction, targetFillFraction, Time.deltaTime * lazyFillSpeed);
        healthBarLazyFill.size = new Vector2(maxWidth * newFraction, healthBarLazyFill.size.y);
    }
}
