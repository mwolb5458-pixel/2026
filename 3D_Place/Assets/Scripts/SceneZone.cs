using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneZone : MonoBehaviour
{
    public GameObject prompt;       // PRESS E 글자
    public string sceneName;        // 넘어갈 씬 이름
    public AudioClip clickSfx;      // E키 누를 때 재생할 효과음
    public CanvasGroup fadeGroup;   // 화면 전환용 검은 패널 CanvasGroup
    public float fadeDuration = 0.5f;

    private bool playerNear;
    private bool isTransitioning;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        if (prompt != null) prompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;
        if (!other.CompareTag("Player")) return;

        playerNear = true;
        if (prompt != null) prompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNear = false;
        if (prompt != null) prompt.SetActive(false);
    }

    private void Update()
    {
        if (isTransitioning || !playerNear) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(TransitionRoutine());
        }
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;
        if (prompt != null) prompt.SetActive(false);

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && clickSfx != null)
        {
            audioSource.PlayOneShot(clickSfx);
        }

        if (fadeGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            fadeGroup.alpha = 1f;
        }
        else
        {
            yield return new WaitForSeconds(fadeDuration);
        }

        SceneManager.LoadScene(sceneName);
    }
}