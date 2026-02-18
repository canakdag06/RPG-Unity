using RPG.Combat;
using UnityEngine;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        private Fighter fighter;
        private GameObject player;

        private const string playerTag = "Player";

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
        }

        private void Start()
        {
            player = GameObject.FindWithTag(playerTag);
        }


        private void Update()
        {
            if (IsInAttackRange() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }
        }

        private bool IsInAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}