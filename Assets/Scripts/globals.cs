using GameJolt.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public struct mission_status
{
    public int mission_id;
    public int score;
    public float time;
    public int rank;
};

[Serializable]
class GameData
{
    public int keys;
    public int stars;
    public int wages;
    public int rate;
    public String name;
    public List<bool> rentsPaid;
    public List<int> mission_ids;
    public List<int> score;
    public List<float> time;
    public List<int> rank;
    //public Dictionary<int, mission_status> mission_stats;
}


public class globals {

    public static bool hasLoaded = false;
    public static bool isPaused = false;
    public static bool hasInit = false;
    public static Sprite[,] sprites;
    public static int keys = 0, stars = 0, wages = 0, rate = 1;
    public static Scene lastScene;
    public static String name;
    public static bool hasWarned = false;
    public static bool isMobile;
    public static bool isSwapping = false;
    public static List<bool> rentsPaid;
    public static int currentMonth;
    public static int currentRent;

    public static bool isSignedInGoogle = false;

    public static Dictionary<int, mission_status> mission_stats;

    public static void init()
    {
        if (hasInit)
            return;
        mission_stats = new Dictionary<int, mission_status>();
        currentMonth = 0;
        rentsPaid = new List<bool>();
        rentsPaid.Add(false);
        hasInit = true;
        //Application.targetFrameRate = 60;

        //Load Data if Exists
        if (isMobile)
        {


            Debug.Log("Atempting to loading");
            if (File.Exists(Application.persistentDataPath + "/playerProgress.dat"))
            {
                Debug.Log("Loading Game Data");
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerProgress.dat", FileMode.Open);
                GameData data = (GameData)bf.Deserialize(file);
                file.Close();

                keys = data.keys;
                wages = data.wages;
                rate = data.rate;
                name = data.name;
                for( int i = 0; i < data.mission_ids.Count; i++ )
                {
                    mission_status currentMission;
                    currentMission.mission_id = i;
                    currentMission.score = data.score[i];
                    currentMission.time = 0;
                    Debug.Log("Loading rank[" + i + "]:" + data.rank[i]);
                    currentMission.rank = data.rank[i];
                    mission_stats.Add(i, currentMission);
                }
                //mission_stats = data.mission_stats;
                hasInit = true;
                //hasWarned = true;
            }
            else
                Debug.Log("Could not open file");

            
        }
        else
        {
            GameJolt.API.Objects.User user = GameJoltAPI.Instance.CurrentUser;
            Debug.Log("User: " + user.Name);
            GameData data = new GameData();
            GameJolt.API.DataStore.Get("keysss", false, dataString => OnLoadKeys(dataString));
            GameJolt.API.DataStore.Get("wages", false, dataString => OnLoadWages(dataString));
            GameJolt.API.DataStore.Get("rate", false, dataString => OnLoadRate(dataString));
            //GameJolt.API.DataStore.Get("stars", false, dataString => OnLoadStars(dataString));
            GameJolt.API.DataStore.Get("mStatss", false, dataString => OnLoadStats(dataString));
        }
    }

    public static void SaveLocalData()
    {
        GameData data = new GameData();
        data.keys = keys;
        data.wages = wages;
        data.rate = rate;
        data.name = name;
        data.mission_ids = new List<int>();
        data.score = new List<int>();
        data.time = new List<float>();
        data.rank = new List<int>();
        for (int i = 0; i < mission_stats.Count; i++)
        {
            data.mission_ids.Add(i);
            data.score.Add(mission_stats[i].score);
            data.time.Add(0);
            Debug.Log("Saving rank[" + i + "]: " + mission_stats[i].rank);
            data.rank.Add(mission_stats[i].rank);
        }
        //data.mission_stats = mission_stats;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerProgress.dat");
        Debug.Log("Saving Game Data");
        bf.Serialize(file, data);
        file.Close();
    }

    public static void OnLoadKeys(string dataString)
    {
        if (dataString != null)
        {
            Debug.Log("Should get " + int.Parse(dataString));
            keys = int.Parse(dataString);
            Debug.Log("Keys: " + keys.ToString());
        }
    }

    public static void OnLoadWages(string dataString)
    {
        if (dataString != null)
        {
            Debug.Log("Should get " + int.Parse(dataString));
            wages = int.Parse(dataString);
            Debug.Log("Wages: " + wages.ToString());
        }
    }

    public static void OnLoadRate(string dataString)
    {
        if (dataString != null)
        {
            Debug.Log("Should get " + int.Parse(dataString));
            rate = int.Parse(dataString);
            Debug.Log("Rate: " + rate.ToString());
        }
    }

    public static void OnLoadStars(string dataString)
    {
        if (dataString != null)
            stars = int.Parse(dataString);
    }

    public static void OnLoadStats(string dataString)
    {
        if (dataString != null)
        {
            for (int i = 0; i < dataString.Length; i++)
            {
                Debug.Log("Loading stats for mission: " + i);
                mission_status currentMission;
                currentMission.mission_id = i;
                currentMission.score = 0;
                currentMission.time = 0;
                currentMission.rank = int.Parse(dataString[i].ToString());
                mission_stats[i] = currentMission;
            }
        }
    }

    public static bool setStatus( int m_id, int s, float t, int r, int tip )
    {
        if (mission_stats[m_id].rank < 2)
        {
            if (mission_stats[m_id].rank < 1 && r > 0)
            {
                keys++;
                wages += rate;
            }
            if (r == 2)
                wages += tip;
        }



        mission_status currentMission;
        currentMission.mission_id = m_id;
        currentMission.score = s;
        currentMission.time = t;
        currentMission.rank = r;
        mission_stats[currentMission.mission_id] = currentMission;
        return true;
    }

    public static bool setStatusPuzzle(int m_id, int s, float t, int r, int tip)
    {
        if (mission_stats[m_id].rank < 2)
        {
            if (r > 0)
            {
                wages += rate;
            }
            //if (r == 2)
            //keys++;
        }
        else if (r == 2)
        {
            if (mission_stats[m_id].rank == 1)
            {
                //keys++;
                wages += tip;
            }
        }



        mission_status currentMission;
        currentMission.mission_id = m_id;
        currentMission.score = s;
        currentMission.time = t;
        currentMission.rank = r;
        mission_stats[currentMission.mission_id] = currentMission;
        return true;
    }

    public static void loadSprites()
    {
        if (hasLoaded)
            return;
        sprites = new Sprite[9, 9];
        //MANUALLY LOAD INDIVIDUAL SPRITE INTO 2-DIMENSIONAL ARRAY BY FLAVOR THEN SHAPE
        //IE sprites[0,0] WILL CONTAIN Chocolate Raised Donut
        sprites[0, 0] = Resources.Load<Sprite>("Donuts/donut/choco");
        sprites[0, 1] = Resources.Load<Sprite>("Donuts/square/choco");
        sprites[0, 2] = Resources.Load<Sprite>("Donuts/paw/choco");
        sprites[0, 3] = Resources.Load<Sprite>("Donuts/star/choco");
        sprites[0, 4] = Resources.Load<Sprite>("Donuts/bone/choco");
        sprites[5, 0] = Resources.Load<Sprite>("Donuts/donut/stale");
        sprites[0, 5] = Resources.Load<Sprite>("Donuts/donut/stale");
        sprites[5, 1] = Resources.Load<Sprite>("Donuts/square/stale");
        sprites[1, 5] = Resources.Load<Sprite>("Donuts/square/stale");
        sprites[5, 2] = Resources.Load<Sprite>("Donuts/paw/stale");
        sprites[2, 5] = Resources.Load<Sprite>("Donuts/paw/stale");
        sprites[5, 3] = Resources.Load<Sprite>("Donuts/star/stale");
        sprites[3, 5] = Resources.Load<Sprite>("Donuts/star/stale");
        sprites[5, 4] = Resources.Load<Sprite>("Donuts/bone/stale");
        sprites[4, 5] = Resources.Load<Sprite>("Donuts/bone/stale");
        sprites[1, 0] = Resources.Load<Sprite>("Donuts/donut/vanilla");
        sprites[1, 1] = Resources.Load<Sprite>("Donuts/square/vanilla");
        sprites[1, 2] = Resources.Load<Sprite>("Donuts/paw/vanilla");
        sprites[1, 3] = Resources.Load<Sprite>("Donuts/star/vanilla");
        sprites[1, 4] = Resources.Load<Sprite>("Donuts/bone/vanilla");
        sprites[2, 0] = Resources.Load<Sprite>("Donuts/donut/purple");
        sprites[2, 1] = Resources.Load<Sprite>("Donuts/square/purple");
        sprites[2, 2] = Resources.Load<Sprite>("Donuts/paw/purple");
        sprites[2, 3] = Resources.Load<Sprite>("Donuts/star/purple");
        sprites[2, 4] = Resources.Load<Sprite>("Donuts/bone/purple");
        sprites[3, 0] = Resources.Load<Sprite>("Donuts/donut/red");
        sprites[3, 1] = Resources.Load<Sprite>("Donuts/square/red");
        sprites[3, 2] = Resources.Load<Sprite>("Donuts/paw/red");
        sprites[3, 3] = Resources.Load<Sprite>("Donuts/star/red");
        sprites[3, 4] = Resources.Load<Sprite>("Donuts/bone/red");
        sprites[4, 0] = Resources.Load<Sprite>("Donuts/donut/pink");
        sprites[4, 1] = Resources.Load<Sprite>("Donuts/square/pink");
        sprites[4, 2] = Resources.Load<Sprite>("Donuts/paw/pink");
        sprites[4, 3] = Resources.Load<Sprite>("Donuts/star/pink");
        sprites[4, 4] = Resources.Load<Sprite>("Donuts/bone/pink");
        sprites[8, 8] = Resources.Load<Sprite>("Tiles/block");
        hasLoaded = true;
    }

}
