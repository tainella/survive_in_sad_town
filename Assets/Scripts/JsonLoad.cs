using System.Diagnostics;
using System.IO;
using UnityEngine;

public class JsonLoad
{
    public PlayerInfo playerInfo;

    private string path = "";

    private void SetPaths()
    {
        path = "Assets/Scripts" + Path.AltDirectorySeparatorChar + "load_scene.json";
    }

    public void CreateDefaultPlayerData()
    {
        SetPaths();
        playerInfo = new PlayerInfo(3, -90.4f, 4f, 76f, 0.0f, 90.0f, 0.0f, false, false, false, false, false, false);
        SaveData();
    }

    public void SaveData()
    {
        string savePath = path;

        string json = JsonUtility.ToJson(playerInfo);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadData()
    {
        SetPaths();
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        PlayerInfo data = JsonUtility.FromJson<PlayerInfo>(json);
        playerInfo = data;
    }
}
