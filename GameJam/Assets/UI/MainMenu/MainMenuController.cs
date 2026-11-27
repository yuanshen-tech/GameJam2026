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
        if (!Application.CanStreamedLevelBeLoaded(gameplayScenePath))
        {
            loadError.SetActive(true);
            return;
        }

        CorridorIntroController.RequestForScene(gameplayScenePath);
        SceneManager.LoadSceneAsync(gameplayScenePath, LoadSceneMode.Single);
        enterButton.interactable = false;
    }

    public void Quit()
    {
        UiClickAudio.QuitAfterClick(quitClick);
    }
}
