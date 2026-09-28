using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variabel gerakan (diberi [SerializeField] agar bisa diatur lewat Inspector)
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 5f;

    // Status karakter
    private float horizontalInput;
    private bool isFacingRight = true; // Ubah ke false jika sprite bawaanmu menghadap ke kiri
    private bool isGrounded = false;

    // Komponen
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        // Mengambil komponen secara otomatis saat game mulai
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Ambil input horizontal
        horizontalInput = Input.GetAxis("Horizontal");

        // 2. Balik arah sprite berdasarkan input
        FlipSprite();

        // 3. Logika Melompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;

            if (animator != null)
            {
                animator.SetBool("isJumping", !isGrounded);
            }
        }
    }

    private void FixedUpdate()
    {
        // Pergerakan karakter (menggunakan FixedUpdate untuk fisika)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Update animasi kecepatan
        if (animator != null)
        {
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
        }
    }

    void FlipSprite()
    {
        // Membalik skala X jika arah gerak bertolak belakang dengan arah hadap
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Deteksi pendaratan (opsional: tambahkan tag "Ground" di Unity)
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;

            if (animator != null)
            {
                animator.SetBool("isJumping", !isGrounded);
            }
        }
    }
}