using System.Collections;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeOut(2f));
        }

        public IEnumerator FadeOutIn()
        {
            yield return FadeOut(1f);
            yield return FadeIn(1f);
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }

}