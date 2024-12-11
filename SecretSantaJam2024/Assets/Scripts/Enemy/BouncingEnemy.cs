using Unity.VisualScripting;
using UnityEngine;

public class BouncingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveDirection = new Vector2(1f, 0.25f);
    [SerializeField] private GameObject rightCheck, leftCheck, roofCheck, groundCheck;
    [SerializeField] private Vector2 rightCheckSize, leftCheckSize, roofCheckSize, groundCheckSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool goingUp = true;

    private Knockback knockback;
    private bool touchedGround, touchedRoof, touchedRight, touchedLeft;
    private Rigidbody2D enemyRB;

    private void Start()
    {
        knockback = GetComponent<Knockback>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HitLogic();
    }

    private void FixedUpdate()
    {
        if(knockback.gettingKnockedBack)
        {
            return;
        }

        enemyRB.velocity = moveDirection * moveSpeed;
    }

    private void HitLogic()
    {
        touchedRight = HitDetector(rightCheck, rightCheckSize, groundLayer);
        touchedLeft = HitDetector(leftCheck, leftCheckSize, groundLayer);
        touchedRoof = HitDetector(roofCheck, roofCheckSize, groundLayer);
        touchedGround = HitDetector(groundCheck, groundCheckSize, groundLayer);

        if (touchedRight || touchedLeft)
        {
            FlipHorizontal();
        }
        if (touchedRoof && goingUp)
        {
            ChangeYDirection();
        }
        if (touchedGround && !goingUp)
        {
            ChangeYDirection();
        }
    }

    private bool HitDetector(GameObject checkObject, Vector2 size, LayerMask layer)
    {
        return Physics2D.OverlapBox(checkObject.transform.position, size, 0f, layer);
    }

    private void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    private void FlipHorizontal()
    {
        moveDirection.x = -moveDirection.x;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheck.transform.position, roofCheckSize);
        Gizmos.DrawWireCube(rightCheck.transform.position, rightCheckSize);
        Gizmos.DrawWireCube(leftCheck.transform.position, leftCheckSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, transform);
        }
    }
}
