using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Label = UnityEngine.UI.Text;

[DefaultExecutionOrder(200)]
public sealed class Success : EndingTrigger<Success>
{
    public int success = 0;
    protected override bool Reached => success >= 6;
}

// The two counters share one canvas. Its active state locks in the first ending.
public abstract class EndingTrigger<T> : MonoBehaviour where T : EndingTrigger<T>
{
    private static EndingTrigger<T> instance;
    public static EndingTrigger<T> Instance=> instance;
    private void Awake()
    {
        instance = this as T;
    }
    [SerializeField] private GameObject screen;
    [SerializeField] private Label body;
    [SerializeField] private AudioClip quitClick;
    [SerializeField] private Button quitButton;
    [SerializeField] private AudioSource speaker;
    [SerializeField] private AudioClip cue;
    [SerializeField, TextArea(3, 6)] private string narration;
    protected abstract bool Reached { get; }

    private bool presenting;
    private float previousSpeed;
    private CursorLockMode previousLock;
    private bool previousCursor;
    private PauseMenuController pause;
    private bool restorePause;

    protected void LateUpdate()
    {
        if (Reached)
            ShowEnding();
    }

    public void ShowEnding()
    {
        if (!Application.isPlaying || !isActiveAndEnabled || screen == null || screen.activeSelf)
            return;

        var intro = FindObjectOfType<CorridorIntroController>();
        if (intro != null)
            intro.Skip();
        pause = FindObjectOfType<PauseMenuController>();
        restorePause = pause != null && pause.enabled;
        if (restorePause)
            pause.enabled = false;

        previousSpeed = Time.timeScale;
        previousLock = Cursor.lockState;
        previousCursor = Cursor.visible;
        presenting = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        body.text = string.Empty;
        quitButton.gameObject.SetActive(false);
        screen.SetActive(true);
        speaker.clip = cue;
        speaker.Play();
        quitButton.onClick.AddListener(Quit);
        StartCoroutine(Reveal());
    }

    private IEnumerator Reveal()
    {
        yield return new WaitForSecondsRealtime(.65f);
        for (int count = 1; count <= narration.Length; count++)
        {
            // Reserve the full paragraph's layout so centered lines stay still as they appear.
            body.text = count == narration.Length ? narration :
                narration.Substring(0, count) + "<color=#00000000>" + narration.Substring(count) + "</color>";
            char c = narration[count - 1];
            float delay = c == '。' ? .42f : c == '，' ? .22f : c == '\n' ? .15f : .085f;
            yield return new WaitForSecondsRealtime(delay);
        }
        quitButton.gameObject.SetActive(true);
        quitButton.Select();
    }

    private void Quit()
    {
        UiClickAudio.QuitAfterClick(quitClick);
    }

    protected void OnDisable()
    {
        if (!presenting)
            return;
        presenting = false;
        StopAllCoroutines();
        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(Quit);
            quitButton.interactable = true;
        }
        if (speaker != null)
            speaker.Stop();
        if (screen != null)
            screen.SetActive(false);
        Time.timeScale = previousSpeed;
        Cursor.lockState = previousLock;
        Cursor.visible = previousCursor;
        if (restorePause && pause != null)
            pause.enabled = true;
    }
}
