using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 12f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;

    // Status karakter
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded = false;

    // Komponen
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Deteksi tanah sederhana: Cek apakah ada collider di sekitar objek groundCheck
        if (groundCheck != null)
        {
            // Ambil semua collider di sekitar titik groundCheck
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius);
            
            isGrounded = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                // Jika menabrak objek lain yang BUKAN karakter Supri sendiri
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        FlipSprite();

        // LOGIKA LOMPAT
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
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

    // Menggambar lingkarannya di tab Scene biar kelihatan jelas
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}