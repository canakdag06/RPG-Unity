using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] float duration;
        [SerializeField] float moveDistance;
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
            float punchDuration = 0.25f;

            DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector3.one * 0.4f, punchDuration, vibrato: 1, elasticity: 0.5f))
                .Append(transform.DOScale(0.8f, duration - punchDuration).SetEase(Ease.InQuad));

            transform.DOLocalMoveY(transform.localPosition.y + moveDistance, duration)
                .SetEase(moveEase);

            tmp.DOFade(0f, duration)
                .SetEase(fadeEase)
                .OnComplete(() => Destroy(gameObject));
        }

        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
