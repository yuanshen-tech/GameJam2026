using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private AudioClip openClick;
    [SerializeField] private AudioClip resumeClick;
    [SerializeField] private AudioClip quitClick;
    [SerializeField] private Button continueButton;

    private bool isPaused;
    private float previousTimeScale;
    private CursorLockMode previousCursorLock;
    private bool previousCursorVisibility;
    private GameObject previousSelection;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (isPaused)
            return;

        previousTimeScale = Time.timeScale;
        previousCursorLock = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;
        previousSelection = EventSystem.current == null ? null : EventSystem.current.currentSelectedGameObject;
        isPaused = true;
        UiClickAudio.Play(openClick);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(true);
        continueButton.Select();
    }

    public void Resume()
    {
        if (isPaused)
            UiClickAudio.Play(resumeClick);
        RestoreGameplay();
    }

    private void RestoreGameplay()
    {
        if (!isPaused)
            return;

        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = previousTimeScale;
        Cursor.lockState = previousCursorLock;
        Cursor.visible = previousCursorVisibility;
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(previousSelection != null && previousSelection.activeInHierarchy ? previousSelection : null);
    }

    public void Quit()
    {
        UiClickAudio.QuitAfterClick(quitClick);
    }

    private void OnDisable()
    {
        RestoreGameplay();
    }
}
