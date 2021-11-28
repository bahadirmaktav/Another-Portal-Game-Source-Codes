using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataController
{
    public void SaveCompletedLevelCounter(int completedLevelCounter)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesettings.dat");
        GameSettingsData data = new GameSettingsData();
        data.completedLevelCounter = completedLevelCounter;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Level data saved.");
    }
    public int GetCompletedLevelCounter()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesettings.dat", FileMode.Open);
            GameSettingsData data = (GameSettingsData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Level data loaded!");
            return data.completedLevelCounter;
        }
        else
        {
            Debug.LogError("There is no save data!");
            return 1;
        }
    }

    public void SaveUISoundAndMusicLevel(float uiSoundLevel, float musicLevel)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/audiosettings.dat");
        AudioSettingsData data = new AudioSettingsData();
        data.uiSoundLevel = uiSoundLevel;
        data.musicLevel = musicLevel;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("UI Sound Level and Music Level data saved.");
    }
    public float GetUISoundLevel()
    {
        if (File.Exists(Application.persistentDataPath + "/audiosettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/audiosettings.dat", FileMode.Open);
            AudioSettingsData data = (AudioSettingsData)bf.Deserialize(file);
            file.Close();
            Debug.Log("UI Sound Level data loaded!");
            return data.uiSoundLevel;
        }
        else
        {
            Debug.LogError("There is no save data!");
            return 0;
        }
    }
    public float GetMusicLevel()
    {
        if (File.Exists(Application.persistentDataPath + "/audiosettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/audiosettings.dat", FileMode.Open);
            AudioSettingsData data = (AudioSettingsData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Music Level data loaded!");
            return data.musicLevel;
        }
        else
        {
            Debug.LogError("There is no save data!");
            return 0;
        }
    }

    public void ResetAllData()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesettings.dat"))
        {
            File.Delete(Application.persistentDataPath + "/gamesettings.dat");
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save game settings data to delete.");

        if (File.Exists(Application.persistentDataPath + "/audiosettings.dat"))
        {
            File.Delete(Application.persistentDataPath + "/audiosettings.dat");
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save audio settings data to delete.");
    }
    public void InitializeAllData()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesettings.dat");
            GameSettingsData data = new GameSettingsData();
            data.completedLevelCounter = 1;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Data initialized.");
        }
        if (!File.Exists(Application.persistentDataPath + "/audiosettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/audiosettings.dat");
            AudioSettingsData data = new AudioSettingsData();
            data.uiSoundLevel = 0.5f;
            data.musicLevel = 0.5f;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Data initialized.");
        }
    }
}

[Serializable]
class GameSettingsData
{
    public int completedLevelCounter;
}

[Serializable]
class AudioSettingsData
{
    public float uiSoundLevel;
    public float musicLevel;
}
