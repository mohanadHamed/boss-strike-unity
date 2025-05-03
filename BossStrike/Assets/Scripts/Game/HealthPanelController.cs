using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthPanelController : MonoBehaviour
{
    [SerializeField]
    private PlayerNumber _playerNumber;

    [SerializeField]
    private RawImage _characterImage;

    [SerializeField]
    private TextMeshProUGUI _playerNameText;

    [SerializeField]
    private Image[] _heartImages;

    private void Start()
    {
        // Set the player name text
        _playerNameText.text = SaveSystem.Load().GetPlayerName(_playerNumber);
        // Set the character image
        _characterImage.texture = GameManager.Instance.GetTextureForPlayer(_playerNumber);
        // Set the heart images based on player lives
        UpdateHeartImages(GameplayManager.Instance.Player1Lives);
    }

    private void UpdateHeartImages(int player1Lives)
    {
        for (int i = 0; i < _heartImages.Length; i++)
        {
            if (i < player1Lives)
            {
                _heartImages[i].enabled = true;
            }
            else
            {
                _heartImages[i].enabled = false;
            }
        }
    }
}
