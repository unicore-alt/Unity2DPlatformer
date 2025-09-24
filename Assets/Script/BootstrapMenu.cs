using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootstrapMenu : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [Header("Scene Names")] 
    [SerializeField] private string _gameSceneName = "Level 1";

    private void OnEnable() => Subscribe();

    private void OnDisable() => Unsubscribe();
    
    private void Subscribe()
    {
        if (_playButton != null) _playButton.onClick.AddListener(PlayGame);
        if (_settingsButton != null) _settingsButton.onClick.AddListener(OpenSettings);
        if (_quitButton != null) _quitButton.onClick.AddListener(ExitGame);
    }
    
    private void Unsubscribe()
    {
        if (_playButton != null) _playButton.onClick.RemoveListener(PlayGame);
        if (_settingsButton != null) _settingsButton.onClick.RemoveListener(OpenSettings);
        if (_quitButton != null) _quitButton.onClick.RemoveListener(ExitGame);
    }

    private void Start()
    {
        _playButton.onClick.AddListener(PlayGame);
        _settingsButton.onClick.AddListener(OpenSettings);
        _quitButton.onClick.AddListener(ExitGame);
    }
    private void PlayGame() => SceneManager.LoadSceneAsync(_gameSceneName);

    private static void OpenSettings() => Debug.Log("Settings");

    private static void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
