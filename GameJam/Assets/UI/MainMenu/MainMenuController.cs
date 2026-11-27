using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip startClick;
    [SerializeField] private AudioClip backClick;
    [SerializeField] private AudioClip enterClick;
    [SerializeField] private AudioClip quitClick;
    [SerializeField] private string gameplayScenePath;
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject titleBackdrop;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private GameObject loadError;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowTitle();
    }

    private void Update()
    {
        if (instructionsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            ReturnToTitle();
    }

    public void ReturnToTitle()
    {
        UiClickAudio.Play(backClick);
        ShowTitle();
    }

    public void ShowTitle()
    {
        instructionsPanel.SetActive(false);
        titlePanel.SetActive(true);
        titleBackdrop.SetActive(true);
        startButton.Select();
    }

    public void ShowInstructions()
    {
        UiClickAudio.Play(startClick);
        titlePanel.SetActive(false);
        titleBackdrop.SetActive(false);
        instructionsPanel.SetActive(true);
        loadError.SetActive(false);
        enterButton.Select();
    }

    public void EnterGame()
    {
        UiClickAudio.Play(enterClick);
        if (!CanLoadGameplayScene())
        {
            loadError.SetActive(true);
            return;
        }

        CorridorIntroController.RequestForScene(gameplayScenePath);
        SceneManager.LoadSceneAsync(gameplayScenePath, LoadSceneMode.Single);
        enterButton.interactable = false;
    }

    // Application.CanStreamedLevelBeLoaded 在编辑器里对任何场景都返回 false，
    // 改为直接对照 Build Settings 里的场景列表（编辑器与打包后行为一致）。
    private bool CanLoadGameplayScene()
    {
        if (string.IsNullOrEmpty(gameplayScenePath))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (path == gameplayScenePath)
                return true;
            if (System.IO.Path.GetFileNameWithoutExtension(path) == gameplayScenePath)
                return true;
        }
        return false;
    }

    public void Quit()
    {
        UiClickAudio.QuitAfterClick(quitClick);
    }
}
