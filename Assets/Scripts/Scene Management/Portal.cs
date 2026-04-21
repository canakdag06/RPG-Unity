using RPG.Saving;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationID
        {
            A, B, C, D, E
        }

        [SerializeField] private int sceneIndexToLoad = 0;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationID destination;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeWaitTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneIndexToLoad < 0)
            {
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            Fader fader = FindAnyObjectByType<Fader>();
            SavingWrapper wrapper = FindAnyObjectByType<SavingWrapper>();

            yield return fader.FadeOut(fadeOutTime);
            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);
            wrapper.Load();
            Portal otherPortal = FindOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }


        private Portal FindOtherPortal()
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this || portal.destination != destination) continue;

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
    }
}