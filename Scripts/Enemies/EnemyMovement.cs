using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    EnemyStats enemy;

    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed*Time.deltaTime);
    }
}
