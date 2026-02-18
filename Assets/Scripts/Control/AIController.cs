using UnityEngine;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;


        private const string playerTag = "Player";

        private void Update()
        {
            GameObject player = GameObject.FindWithTag(playerTag);
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if(distance < chaseDistance)
            {
                Debug.Log("GET HIM!",this);
            }
        }
    }
}