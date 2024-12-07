using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDodging = false;
    private float nextDodgeTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isDodging)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(moveX, moveY).normalized;

            if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDodgeTime)
            {
                StartCoroutine(Dodge());
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator Dodge()
    {
        isDodging = true;
        nextDodgeTime = Time.time + dodgeCooldown;
        float startTime = Time.time;

        while (Time.time < startTime + dodgeDuration)
        {
            rb.MovePosition(rb.position + moveInput * dodgeSpeed * Time.fixedDeltaTime);
            yield return null; // Wait for the next frame
        }

        isDodging = false;
    }
}
