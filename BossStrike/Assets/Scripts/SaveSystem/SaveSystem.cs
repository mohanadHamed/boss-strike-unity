using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public static class SaveSystem
{
    static string path => Path.Combine(Application.persistentDataPath, "boss_strike_save.json");

    public static void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
    }

    public static GameSaveData Load()
    {
        if (!File.Exists(path)) return new GameSaveData();
        return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path));
    }

    public static void SaveLeaderboardEntry(LeaderboardEntry entry)
    {
        var data = Load();
        for (int i = 0; i < data.LeaderboardEntries.Length; i++)
        {
            if (data.LeaderboardEntries[i] == null || data.LeaderboardEntries[i].Score < entry.Score)
            {
                data.LeaderboardEntries[i] = entry;
                break;
            }
        }
        Save(data);
    }

    public static LeaderboardEntry[] LoadLeaderboardEntries()
    {
        var data = Load();
        return data.LeaderboardEntries;
    }

    public static void SavePlayer1Name(string name)
    {
        var data = Load();
        data.Player1Name = name;
        Save(data);
    }

    public static void SavePlayer2Name(string name)
    {
        var data = Load();
        data.Player2Name = name;
        Save(data);
    }
}




