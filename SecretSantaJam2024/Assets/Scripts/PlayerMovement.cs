using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    public float moveSpeed = 5f;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRender;
    private Vector2 moveInput;
    private bool isDodging = false;
    private float nextDodgeTime;

    private bool facingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        AdjustPlayerFacingDirection();

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

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            Vector2 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale =scale;
            FacingLeft = true;
        }
        else
        {
            Vector2 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
            FacingLeft = false;
        }
    }
}
