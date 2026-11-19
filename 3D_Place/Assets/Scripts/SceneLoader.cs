using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;          // 넘어갈 씬 이름
    public AudioClip clickSfx;        // E키 누를 때 재생할 효과음
    public CanvasGroup fadeGroup;     // 화면 전환용 검은 패널 CanvasGroup
    public float fadeDuration = 0.5f; // 어두워지는 시간(초)
    public bool requireKeyE = true;   // true면 구역 안에서 E키를 눌러야 이동

    private bool playerInZone = false;
    private bool isLoading = false;

    private void Update()
    {
        if (!requireKeyE || isLoading || !playerInZone) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Load();
        }
    }

    public void Load()
    {
        if (isLoading) return;
        isLoading = true;
        StartCoroutine(LoadWithFade());
    }

    private IEnumerator LoadWithFade()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && clickSfx != null)
        {
            audio.PlayOneShot(clickSfx);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}