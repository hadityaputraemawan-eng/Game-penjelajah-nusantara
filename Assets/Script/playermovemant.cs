using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 10f;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2; // Set 2 untuk Double Jump, Set 1 untuk Lompat Biasa
    private int jumpsRemaining;

    // Status karakter
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded = false;

    // Komponen
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Cek apakah menyentuh tanah
        isGrounded = CheckGround();

        // Reset jumlah lompatan saat menyentuh tanah
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpsRemaining = maxJumps;
        }

        FlipSprite();

        // Logika Melompat (Bisa loncat selama sisa lompatan masih ada)
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpsRemaining--; // Kurangi jatah loncat
        }

        // Update animasi
        if (animator != null)
        {
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (animator != null)
        {
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
        }
    }

    private bool CheckGround()
    {
        if (coll == null) return false;

        // Tembakkan BoxCast sedikit lebih fleksibel di bawah kaki
        Vector2 raycastOrigin = new Vector2(coll.bounds.center.x, coll.bounds.min.y);
        Vector2 boxSize = new Vector2(coll.bounds.size.x * 0.8f, 0.05f);

        RaycastHit2D hit = Physics2D.BoxCast(raycastOrigin, boxSize, 0f, Vector2.down, 0.15f);

        return hit.collider != null && hit.collider.gameObject != gameObject;
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
}