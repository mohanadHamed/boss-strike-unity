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
}
