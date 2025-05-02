using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string PlayerName;
    public int Score;
}

[System.Serializable]
public class GameSaveData
{
    public string Player1Name = "Player 1";
    public string Player2Name = "Player 2";
    public bool SoundMusicEnabled = true;
    public bool SoundEffectsEnabled = true;

    public LeaderboardEntry[] LeaderboardEntries = new LeaderboardEntry[10];
}
