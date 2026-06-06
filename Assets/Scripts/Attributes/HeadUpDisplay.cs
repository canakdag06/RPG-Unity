using RPG.Attributes;
using RPG.Stats;
using TMPro;
using UnityEngine;

public class HeadUpDisplay : MonoBehaviour
{
    Health health;
    Experience experience;
    BaseStats stats;

    int level;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI levelText;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        health = player.GetComponent<Health>();
        experience = player.GetComponent<Experience>();
        stats = player.GetComponent<BaseStats>();

    }

    private void OnEnable()
    {
        health.OnHealthChanged += UpdateHealthDisplay;
        experience.OnExpChanged += UpdateExpDisplay;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateHealthDisplay;
        experience.OnExpChanged -= UpdateExpDisplay;
    }

    private void UpdateHealthDisplay(float percentage)
    {
        healthText.text = $"HP: {percentage:0}%";
    }

    private void UpdateExpDisplay(float expPoints)
    {
        expText.text = $"EXP: {expPoints:0}";

        level = stats.CalculateLevel();
        levelText.text = $"Level: {level}";
    }

    //private void UpdateLevelDisplay(int level)
    //{
    //    levelText.text = $"Level: {level}";
    //}
}
