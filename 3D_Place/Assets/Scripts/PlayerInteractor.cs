using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerInteractor : MonoBehaviour
{
    public FirstPersonMover mover;     // Player의 FirstPersonMover
 
    private Exhibit nearby;            // 지금 가까이 있는 작품
    private GameObject openPanel;      // 지금 열려 있는 패널
    private void OnTriggerEnter(Collider other)
    {
        Exhibit e = other.GetComponent<Exhibit>();
        if (e == null) return;
        nearby = e;
        nearby.ShowPrompt(true);
    }
 
    private void OnTriggerExit(Collider other)
    {
        Exhibit e = other.GetComponent<Exhibit>();
        if (e == null || e != nearby) return;
        nearby.ShowPrompt(false);
        nearby = null;
    }
    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;
        if (openPanel != null)                  // 열려 있으면 닫기
        {
            PlayExhibitSfx(nearby);             // 닫을 때 효과음 (싫으면 이 줄 빼기)
            openPanel.SetActive(false);
            openPanel = null;
            mover.canMove = true;
            return;
        }
        if (nearby == null) return;             // 가까운 작품이 없으면 무시
        openPanel = nearby.panel;
        openPanel.SetActive(true);
        mover.canMove = false;
        PlayExhibitSfx(nearby);                 // 열 때 효과음
    }
    private void PlayExhibitSfx(Exhibit target)
    {
        if (target == null || target.sfxClip == null) return;

        AudioSource audio = target.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.PlayOneShot(target.sfxClip);
        }
    }

}
