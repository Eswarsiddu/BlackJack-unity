using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class FileData{
    public bool haptic;
    public bool sound;
    public int coins;
    public FileData(Settings settings,PlayerData playerdata)
	{
        haptic = settings.haptic;
        sound = settings.sound;
        coins = playerdata.coins;
	}
    public FileData(bool haptic, bool sound, int coins)
	{
        this.haptic = haptic;
        this.sound = sound;
        this.coins = coins;
	}
}

public static class SaveSystem
{
    private static string HAPTIC = "haptic";
    private static string SOUND = "sound";
    private static string COINS = "coins";

    private static void Savefile(Settings settings,PlayerData playerdata)
	{
        FileData data = new FileData(settings, playerdata);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.siddu";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void SaveData()
	{
        Settings settings = Resources.Load<Settings>(Constants.SETTINGS_PATH);
        PlayerData playerdata = Resources.Load<PlayerData>(Constants.PLAYER_DATA_PATH);
        PlayerPrefs.SetInt(COINS, playerdata.coins);
        PlayerPrefs.SetInt(SOUND, settings.sound ? 1 : 0);
        PlayerPrefs.SetInt(HAPTIC, settings.haptic ? 1 : 0);
    }

    public static void LoadData()
	{
        
        Settings settings = Resources.Load<Settings>(Constants.SETTINGS_PATH);
        PlayerData playerdata = Resources.Load<PlayerData>(Constants.PLAYER_DATA_PATH);

        FileData data = new FileData(
            haptic: PlayerPrefs.GetInt(HAPTIC, 1) == 1 ? true : false,
            sound: PlayerPrefs.GetInt(SOUND, 1) == 1 ? true : false,
            coins: PlayerPrefs.GetInt(COINS,Constants.DEFAULT_COINS)
            );
         /*
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as FileData;
            stream.Close();
        }
        else
        {
            data = new FileData(haptic: true, sound: true, coins: Constants.DEFAULT_COINS);
        }*/
        settings.SetHaptic(data.haptic);
        settings.SetSound(data.sound);
        playerdata.SetCoins(data.coins);
    }
}
