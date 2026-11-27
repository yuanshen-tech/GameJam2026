using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public sealed class CorridorIntroController : MonoBehaviour
{
    private static string requestedScenePath;

    [SerializeField] private GameObject overlay;
    [SerializeField] private UnityEngine.UI.Text narrationLabel;
    [SerializeField] private Button skipButton;
    [SerializeField] private AudioClip skipClick;
    [SerializeField] private AudioSource typewriter;
    [SerializeField, TextArea(3, 8)] private string narration;

    private PauseMenuController pauseMenu;
    private bool restorePauseMenu;
    private bool showing;
    private float previousTimeScale;
    private CursorLockMode previousCursorLock;
    private bool previousCursorVisibility;
    private Camera gameplayCamera;
    private CameraClearFlags previousClearFlags;
    private Color previousBackgroundColor;
    private int previousCullingMask;

    public static void RequestForScene(string scenePath)
    {
        requestedScenePath = scenePath;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRequest()
    {
        requestedScenePath = null;
    }

    private void Start()
    {
        if (requestedScenePath != gameObject.scene.path)
            return;

        requestedScenePath = null;
        pauseMenu = FindObjectOfType<PauseMenuController>();
        restorePauseMenu = pauseMenu != null && pauseMenu.enabled;
        if (restorePauseMenu)
            pauseMenu.enabled = false;

        previousTimeScale = Time.timeScale;
        previousCursorLock = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;
        showing = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameplayCamera = Camera.main;
        if (gameplayCamera != null)
        {
            previousClearFlags = gameplayCamera.clearFlags;
            previousBackgroundColor = gameplayCamera.backgroundColor;
            previousCullingMask = gameplayCamera.cullingMask;
            gameplayCamera.clearFlags = CameraClearFlags.SolidColor;
            gameplayCamera.backgroundColor = Color.black;
            gameplayCamera.cullingMask = 0;
        }

        narrationLabel.text = string.Empty;
        overlay.SetActive(true);
        skipButton.Select();
        StartCoroutine(TypeNarration());
    }

    private IEnumerator TypeNarration()
    {
        for (int i = 0; i < narration.Length; i++)
        {
            char character = narration[i];
            narrationLabel.text = narration.Substring(0, i + 1);
            if (!char.IsWhiteSpace(character))
            {
                typewriter.pitch = 0.94f + (i % 5) * 0.032f;
                typewriter.PlayOneShot(typewriter.clip);
            }

            bool sentenceEnd = character == '\u3002' || character == '\n';
            yield return new WaitForSecondsRealtime(sentenceEnd ? 0.24f : 0.072f);
        }

        yield return new WaitForSecondsRealtime(2.1f);
        Skip();
    }

    public void SkipFromButton()
    {
        if (showing)
            UiClickAudio.Play(skipClick);
        Skip();
    }

    public void Skip()
    {
        if (!showing)
            return;

        StopAllCoroutines();
        showing = false;
        typewriter.Stop();
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == skipButton.gameObject)
            EventSystem.current.SetSelectedGameObject(null);
        overlay.SetActive(false);
        if (gameplayCamera != null)
        {
            gameplayCamera.clearFlags = previousClearFlags;
            gameplayCamera.backgroundColor = previousBackgroundColor;
            gameplayCamera.cullingMask = previousCullingMask;
        }
        Time.timeScale = previousTimeScale;
        Cursor.lockState = previousCursorLock;
        Cursor.visible = previousCursorVisibility;
        if (restorePauseMenu && pauseMenu != null)
            pauseMenu.enabled = true;
    }

    private void OnDisable()
    {
        Skip();
    }
}
