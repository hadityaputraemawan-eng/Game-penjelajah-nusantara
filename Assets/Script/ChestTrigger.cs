using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questionPanel; // UI Panel Pertanyaan
    [SerializeField] private GameObject wrongAnswerNotice; // UI Teks "Jawaban Salah!" (Opsional)

    private bool playerIsClose = false;

    private void Start()
    {
        // Sembunyikan UI saat awal game
        if (questionPanel != null) questionPanel.SetActive(false);
        if (wrongAnswerNotice != null) wrongAnswerNotice.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek jika yang menyentuh peti adalah Player
        if (other.CompareTag("Player") || other.GetComponent<PlayerMovement>() != null)
        {
            playerIsClose = true;
            ShowQuestion();
        }
    }

    private void ShowQuestion()
    {
        if (questionPanel != null)
        {
            questionPanel.SetActive(true);
            Time.timeScale = 0f; // Pause pergerakan game saat menjawab
        }
    }

    // --- FUNGSI DENGAN TOMBOL JAWABAN ---

    // Panggil fungsi ini di Button Jawaban yang BENAR
    public void OnCorrectAnswer()
    {
        Debug.Log("Jawaban BENAR!");
        
        // Unpause game
        Time.timeScale = 1f;

        // Sembunyikan Panel UI Pertanyaan
        if (questionPanel != null) questionPanel.SetActive(false);

        // Hancurkan / Hilangkan Peti dari Game (atau ganti sprite jadi peti terbuka)
        Destroy(gameObject);
    }

    // Panggil fungsi ini di Button Jawaban yang SALAH
    public void OnWrongAnswer()
    {
        Debug.Log("Jawaban SALAH!");

        // Tampilkan teks pemberitahuan salah jika ada
        if (wrongAnswerNotice != null)
        {
            wrongAnswerNotice.SetActive(true);
        }
    }
}