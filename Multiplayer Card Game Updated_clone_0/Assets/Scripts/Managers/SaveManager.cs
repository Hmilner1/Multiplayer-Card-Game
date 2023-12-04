using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static void SavePlayerInfo(PlayerInfoManager info)
    {
        BinaryFormatter SaveFormatter = new BinaryFormatter();
        string SavePath = Application.persistentDataPath + "/PlayerInfo.data";
        FileStream fileStream = new FileStream(SavePath, FileMode.Create);

        PlayerInfo settings = new PlayerInfo(info);


        SaveFormatter.Serialize(fileStream, settings);
        fileStream.Close();
    }

    public static PlayerInfo LoadPlayerInfo()
    {
        string SavePath = Application.persistentDataPath + "/PlayerInfo.data";
        if (File.Exists(SavePath))
        {
            BinaryFormatter SaveFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(SavePath, FileMode.Open);

            PlayerInfo Settings = SaveFormatter.Deserialize(fileStream) as PlayerInfo;
            fileStream.Close();
            return Settings;
        }
        else
        {
            Debug.Log("No player save found");
            return null;
        }
    }
}
