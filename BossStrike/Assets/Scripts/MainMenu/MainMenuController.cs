using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectCharacterPanel;
    [SerializeField]
    private GameObject _settingsPanel;

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
        _selectCharacterPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }
}
