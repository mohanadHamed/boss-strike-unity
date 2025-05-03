using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _titleText;

    [SerializeField]
    private TextMeshProUGUI[] _playerNameTexts;

    [SerializeField]
    private TextMeshProUGUI[] _playerScoreTexts;


    public void SetGameOverTitle(string title)
    {
        _titleText.text = title;
    }

    public void ShowLeaderboardEntries()
    {
        var leaderboardEntries = SaveSystem.LoadLeaderboardEntries();
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            _playerNameTexts[i].text = leaderboardEntries[i].PlayerName;
            _playerScoreTexts[i].text = leaderboardEntries[i].Score.ToString();
        }
    }

    public void RestartButtonClick()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitButtonClick()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("MainMenu");
    }
}
