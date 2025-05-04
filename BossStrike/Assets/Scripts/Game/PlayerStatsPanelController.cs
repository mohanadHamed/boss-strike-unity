using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsPanelController : MonoBehaviour
{
    [SerializeField]
    private PlayerNumber _playerNumber;

    [SerializeField]
    private RawImage _characterImage;

    [SerializeField]
    private TextMeshProUGUI _playerNameText;

    [SerializeField]
    private TextMeshProUGUI _playerScoreText;

    [SerializeField]
    private Image[] _heartImages;

    public void UpdateHeartImages(int player1Lives)
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
    public void UpdatePlayerScoreText()
    {
        _playerScoreText.text = $"Score: {GameplayManager.Instance.GetPlayerScore(_playerNumber)}";
    }

    private void Start()
    {
        _playerNameText.text = SaveSystem.Load().GetPlayerName(_playerNumber);
        _characterImage.texture = GameManager.Instance.GetTextureForPlayer(_playerNumber);
        UpdateHeartImages(GameplayManager.Instance.Player1Lives);

        UpdatePlayerScoreText();
    }
}
