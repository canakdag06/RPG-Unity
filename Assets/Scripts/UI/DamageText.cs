using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] float duration = 1f;
        [SerializeField] float moveDistance = 1.5f;
        [SerializeField] Ease moveEase = Ease.OutCubic;
        [SerializeField] Ease fadeEase = Ease.InQuad;

        private TextMeshPro tmp;

        void Awake()
        {
            tmp = GetComponentInChildren<TextMeshPro>();
        }

        public void SetText(float damageAmount)
        {
            if (tmp != null)
                tmp.text = Mathf.RoundToInt(damageAmount).ToString();
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void PopUpAnimation()
        {
            transform
                .DOMove(transform.position + Vector3.up * moveDistance, duration)
                .SetEase(moveEase);

            tmp.material.DOFade(0f, duration)
                .SetEase(fadeEase)
                .OnComplete(() => Destroy(gameObject));
        }

        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
