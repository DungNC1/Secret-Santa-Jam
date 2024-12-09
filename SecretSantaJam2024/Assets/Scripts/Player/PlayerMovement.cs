using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    public GameObject ghostTrailPrefab;
    public float moveSpeed = 5f;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerMana playerMana;
    private Animator animator;
    private Vector2 moveInput;
    private bool isDodging = false;
    private float nextDodgeTime;
    private float originalMovespeed;

    private bool facingLeft = false;

    private void Awake()
    {
        playerMana = GetComponent<PlayerMana>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMovespeed = moveSpeed;
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

        animator.SetFloat("IsMoving", moveInput.magnitude);
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
        playerMana.UseMana(5);

        while (Time.time < startTime + dodgeDuration)
        {
            rb.MovePosition(rb.position + moveInput * dodgeSpeed * Time.fixedDeltaTime);
            CreateGhostTrail();
            yield return null; // Wait for the next frame
        }

        isDodging = false;
    }

    private void CreateGhostTrail()
    {
        GameObject ghostTrail = Instantiate(ghostTrailPrefab, transform.position, transform.rotation);
        SpriteRenderer ghostSpriteRenderer = ghostTrail.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();

        ghostSpriteRenderer.sprite = playerSpriteRenderer.sprite;
        ghostSpriteRenderer.color = playerSpriteRenderer.color;
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
    
    public IEnumerator MovementBoost(float speed)
    {
        moveSpeed = speed;
        yield return new WaitForSeconds(10);
        moveSpeed = originalMovespeed;
    }
}
