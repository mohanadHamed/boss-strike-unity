using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectCharacterPanel;
    [SerializeField]
    private GameObject _settingsPanel;

    [SerializeField]
    private RawImage _player1SelectedCharacterImage;
    [SerializeField]
    private RawImage _player2SelectedCharacterImage;

    [SerializeField]
    private TextMeshProUGUI _player1NameText;
    [SerializeField]
    private TextMeshProUGUI _player2NameText;

    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _selectCharacterButton;
    [SerializeField] 
    private Button _settingsButton;
    
    [SerializeField]
    private SceneLoader _sceneLoader;

    private void Start()
    {
        InitializeUi();
    }

    private void InitializeUi()
    {
        // Set the player names
        _player1NameText.text = SaveSystem.Load().Player1Name;
        _player2NameText.text = SaveSystem.Load().Player2Name;

        // Initialize the selected character images
        _player1SelectedCharacterImage.texture = GameManager.Instance.PlayerCharacterTextureMap[GameManager.Instance.Player1Character];
        _player2SelectedCharacterImage.texture = GameManager.Instance.PlayerCharacterTextureMap[GameManager.Instance.Player2Character];
        // Set the initial state of the panels
        _selectCharacterPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    public void PlayButtonClick()
    {
        DisableAllButtons();
        _sceneLoader.LoadTargetScene("Gameplay");
    }

    public void SelecCharacterClick()
    {
        _selectCharacterPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void SettingsButtonClick()
    {
        _selectCharacterPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void BackButtonClick()
    {
        InitializeUi();
    }

    private void DisableAllButtons()
    {
        _playButton.interactable = false;
        _selectCharacterButton.interactable = false;
        _settingsButton.interactable = false;
    }
}
