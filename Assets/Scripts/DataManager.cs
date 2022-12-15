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
    private SaveData m_data;

    public PlayerData CurPlayer
    {
        get => m_curPlr;
    }
    public int Highscore
    {
        get => m_data.Get(m_curPlr.Name).Score;
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            m_curPlr = new PlayerData("", 0);

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
            m_data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            m_data = new SaveData(new List<PlayerData>());
        }
        m_data.PrintAll();
    }
    private void SaveData()
    {
        string json = JsonUtility.ToJson(m_data);
        File.WriteAllText(FilePath, json);
    }

    public PlayerData[] GetHighscores()
    {
        PlayerData[] highscores = new PlayerData[m_data.players.Count];
        m_data.players.CopyTo(highscores);
        return highscores;
    }

    public void SetCurPlayer(string name)
    {
        m_curPlr = new PlayerData(name, 0);
        m_data.Get(ref m_curPlr);
    }
    public void PlayerScored(int points)
    {
        m_curPlr.Score += points;
    }
    public void GameOver()
    {
        m_data.Update(m_curPlr);
        m_curPlr.Score = 0;
        SaveData();
    }
}
[Serializable]
public struct SaveData
{
    public List<PlayerData> players;

    private static Int32 Comparator(PlayerData a, PlayerData b)
    {
        if (a < b)
            return 1;
        else if (a > b)
            return -1;
        else
            return 0;
    }

    public void Add(PlayerData plr)
    {
        players.Add(plr);
        players.Sort(Comparator);
    }
    public void Get(ref PlayerData plr)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] == plr)
            {
                plr = players[i];
                return;
            }
        }
    }
    public PlayerData Get(string name)
    {
        for (int i = 0; i < players.Count; ++i)
            if (players[i].Name == name)
                return players[i];

        return new PlayerData(name, 0);
    }
    public void Update(PlayerData plr)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] == plr)
            {
                if (plr > players[i])
                {
                    players[i] = plr;
                    players.Sort(Comparator);
                }
                return;
            }
        }

        Add(plr);
    }
    public void PrintAll()
    {
        string str = "";

        foreach (PlayerData plr in players)
            str += plr + '\n';

        Debug.Log(str);
    }

    public SaveData(List<PlayerData> players) => this.players = players;
}

[Serializable]
public struct PlayerData
{
    public string Name;
    public int Score;

    public static implicit operator string(PlayerData data) => $"{data.Name} - {data.Score}";
    public override string ToString() => (string)this;

    public static bool operator< (PlayerData a, PlayerData b) => a.Score < b.Score;
    public static bool operator> (PlayerData a, PlayerData b) => a.Score > b.Score;

    public static bool operator== (PlayerData a, PlayerData b) => a.Name == b.Name;
    public static bool operator!= (PlayerData a, PlayerData b) => a.Name != b.Name;

    public override bool Equals(object obj) => base.Equals(obj);
    public bool Equals(PlayerData other) => Name == other.Name && Score == other.Score;
    public override int GetHashCode() => Name.GetHashCode();

    public PlayerData(string name, int score)
    {
        Name = name;
        Score = score;
    }
}