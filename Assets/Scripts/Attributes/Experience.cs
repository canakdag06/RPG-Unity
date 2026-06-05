using UnityEngine;


namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0f;

        public void GainEXP(float exp)
        {
            experiencePoints += exp;
        }
    }
}
