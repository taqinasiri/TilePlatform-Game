using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D rigidBody2D;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rigidBody2D.velocity = new Vector2(moveSpeed,0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyX();
    }

    private void FlipEnemyX()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rigidBody2D.velocity.x)),1f);
    }
}