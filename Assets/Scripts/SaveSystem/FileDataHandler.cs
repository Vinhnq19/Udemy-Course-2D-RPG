using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool encryptData;
    private string codeWord = "QuangVinh";

    public FileDataHandler(string dataDirPath, string fileName, bool encryptData)
    {
        this.encryptData = encryptData;
        fullPath = Path.Combine(dataDirPath, fileName); // Combine directory path and file name to get full path
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            //1. Create Directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //2. Convert GameData to JSON
            string dataToSave = JsonUtility.ToJson(gameData, true);
            //if encryption is enabled, encrypt the data
            if (encryptData)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }
            //3. Write JSON to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                //4. Write data to file
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(encryptData)
                dataToLoad = EncryptDecrypt(dataToLoad);
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading data from file: " + fullPath + "\n" + e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifiedData;
    }
}
