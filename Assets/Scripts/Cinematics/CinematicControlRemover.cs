using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private PlayableDirector director;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }


        private void OnEnable()
        {
            director.played += DisableControl;
            director.stopped += EnableControl;
        }

        private void OnDisable()
        {
            director.played -= DisableControl;
            director.stopped -= EnableControl;
        }


        private void EnableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        private void DisableControl(PlayableDirector director)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
    }
}
