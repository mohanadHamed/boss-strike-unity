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
}
