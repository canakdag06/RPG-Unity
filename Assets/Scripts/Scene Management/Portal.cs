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
            yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);

            Portal otherPortal = FindOtherPortal();
            UpdatePlayer(otherPortal);
            print("Scene loaded");
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