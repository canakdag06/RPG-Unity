using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private const string playerTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) { return; }

            GetComponent<PlayableDirector>().Play();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}