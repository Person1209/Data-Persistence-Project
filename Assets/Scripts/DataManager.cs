using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    private static string FilePath;

    private PlayerData m_curPlr;
    private PlayerData m_bestPlr;

    public PlayerData CurPlayer
    {
        get => m_curPlr;
        private set => m_curPlr = value;
    }
    public PlayerData BestPlayer
    {
        get => m_bestPlr;
        set => m_bestPlr = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            FilePath = Application.persistentDataPath + "/savefile.json";
            LoadData();
        }    
        else
        {
            Destroy(gameObject);
        }
    }
    private void LoadData()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            m_bestPlr = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            m_bestPlr = new PlayerData("Nobody", 0);
        }
    }
    private void SaveData()
    {
        string json = JsonUtility.ToJson(m_bestPlr);
        File.WriteAllText(FilePath, json);
    }

    public void UpdateBestPlayer()
    {
        if (CurPlayer.Score > BestPlayer.Score)
        {
            m_bestPlr = m_curPlr;
            SaveData();
        }
    }

    public void SetCurPlayer(string name)
    {
        m_curPlr = new PlayerData(name, 0);
    }
    public void PlayerScored(int points)
    {
        m_curPlr.Score += points;
    }
    public void GameOver()
    {
        UpdateBestPlayer();
        m_curPlr.Score = 0;
    }
}
[Serializable]
public struct PlayerData
{
    public string Name;
    public int Score;

    public static implicit operator string(PlayerData data)
    {
        return $"Name: {data.Name} - Score: {data.Score}";
    }

    public PlayerData(string name, int score)
    {
        Name = name;
        Score = score;
    }
}