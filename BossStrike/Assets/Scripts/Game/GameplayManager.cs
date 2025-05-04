using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public int Player1Score { get; set; }
    public int Player2Score { get; set; }

    public int Player1Lives { get; private set; }
    public int Player2Lives { get; private set; }
    public int BossLives { get; private set; }

    public GameObject BossInstance => _bossInstance;
    public GameObject RocketPrefab => _rocketPrefab;

    public bool IsGameOver => (Player1Lives <= 0 && Player2Lives <= 0) || BossLives <= 0;

    public AudioClip FlameAttackAudio;
    public AudioClip EagleStrikeFlyAudio;
    public AudioClip EagleStrikeSlamAudio;
    public AudioClip LaunchRocketAudio;
    public AudioClip ExplodeAudio;
    public AudioClip PlayerHitAudio;
    public AudioClip PlayerScreamAudio;
    public AudioClip BossHitAudio;

    [SerializeField]
    private PlayerStatsPanelController _player1HealthPanelController;
    [SerializeField]
    private PlayerStatsPanelController _player2HealthPanelController;

    [SerializeField]
    private GameObject _bossInstance;

    [SerializeField]
    private GameObject _rocketPrefab;

    [SerializeField]
    private GameoverPanelController _gameoverPanelController;


    public int GetLivesForPlayer(PlayerNumber playerNumber)
    {
        return playerNumber == PlayerNumber.PlayerOne ? Player1Lives : Player2Lives;
    }

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
        _player1HealthPanelController.UpdatePlayerScoreText();
        _player2HealthPanelController.UpdatePlayerScoreText();
    }

    public void IncreasePlayerScore(PlayerNumber playerNumber, int score)
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            Player1Score += score;
            _player1HealthPanelController.UpdatePlayerScoreText();
        }
        else
        {
            Player2Score += score;
            _player2HealthPanelController.UpdatePlayerScoreText();
        }
    }

    public void DecreasePlayerLives(PlayerController playerController)
    {
        if (playerController.PlayerNumber == PlayerNumber.PlayerOne)
        {
            Player1Lives--;
            _player1HealthPanelController.UpdateHeartImages(Player1Lives);
            if (Player1Lives <= 0)
            {
                // Handle player 1 defeat
                Debug.Log("Player 1 defeated!");

            }
        }
        else
        {
            Player2Lives--;
            _player2HealthPanelController.UpdateHeartImages(Player2Lives);
            if (Player1Lives <= 0)
            {
                // Handle player 1 defeat
                Debug.Log("Player 1 defeated!");

            }
        }

        CheckGameOver();
    }

    public IEnumerator DecreaseBossLives()
    {
        BossLives--;
        Debug.Log($"Boss lives = {BossLives}");
        if(BossLives <= 0)
        {
            _bossInstance.SetActive(false);
        }

        yield return new WaitForSeconds(1.0f);
        CheckGameOver();
    }

    public int GetPlayerScore(PlayerNumber playerNumber)
    {
        return playerNumber == PlayerNumber.PlayerOne ? Player1Score : Player2Score;
    }

    public void CheckGameOver()
    {
        if (!IsGameOver) return;

        // Show game over panel
        _gameoverPanelController.SetGameOverTitle(BossLives <= 0 ? "You win" : "You lose");
        SaveSystem.SaveLeaderboardEntry(new LeaderboardEntry
        {
            PlayerName = SaveSystem.Load().GetPlayerName(PlayerNumber.PlayerOne),
            Score = Player1Score
        });
        SaveSystem.SaveLeaderboardEntry(new LeaderboardEntry
        {
            PlayerName = SaveSystem.Load().GetPlayerName(PlayerNumber.PlayerTwo),
            Score = Player2Score
        });

        _gameoverPanelController.ShowLeaderboardEntries();

        _gameoverPanelController.gameObject.SetActive(true);

        Time.timeScale = 0; // Pause the game
    }

    public void PlaySfxAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource == null || audioClip == null) return;
        
        if (!SaveSystem.Load().SoundEffectsEnabled) return;
        
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
