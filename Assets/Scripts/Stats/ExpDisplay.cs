using RPG.Stats;
using TMPro;
using UnityEngine;

public class ExpDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI expText;
    Experience exp;

    private void Awake()
    {
        exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
    }

    private void OnEnable()
    {
        exp.OnExpChanged += UpdateExpDisplay;
    }

    private void OnDisable()
    {
        exp.OnExpChanged -= UpdateExpDisplay;
    }

    private void UpdateExpDisplay(float expPoints)
    {
        expText.text = $"EXP: {expPoints:0}%";
    }
}
