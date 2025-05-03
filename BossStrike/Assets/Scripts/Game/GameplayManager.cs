using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public int Player1Score { get; set; }
    public int Player2Score { get; set; }

    public int Player1Lives { get; private set; }
    public int Player2Lives { get; private set; }
    public int BossLives { get; private set; }

    public bool IsGameOver => (Player1Lives <= 0 && Player2Lives <= 0) || BossLives <= 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Initialize()
    {
        // Initialize player lives and scores
        Player1Lives = 3;
        Player2Lives = 3;
        BossLives = 3;
        Player1Score = 0;
        Player2Score = 0;
    }

    public void UpdatePlayerScore(PlayerNumber playerNumber, int score)
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            Player1Score += score;
        }
        else
        {
            Player2Score += score;
        }
    }

    public void DecreasePlayerLives(PlayerNumber playerNumber)
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            Player1Lives--;
        }
        else
        {
            Player2Lives--;
        }
    }


}
