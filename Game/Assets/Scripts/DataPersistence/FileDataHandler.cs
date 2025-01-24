using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.Rendering;


public class FileDataHandler : Cryptography
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private byte[] key = new byte[16] { 25, 197, 154, 211, 240, 223, 182, 197, 205, 16, 147, 190, 55, 126, 152, 47 };
    byte[] iv = new byte[16] { 101, 56, 27, 187, 183, 147, 214, 82, 84, 133, 193, 167, 118, 147, 253, 18 };

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                byte[] encryptedBytes = Convert.FromBase64String(dataToLoad);
                byte[] decrypted = Decode(encryptedBytes, key, iv);
                // deserialize the data from json to C# object
                loadedData = JsonUtility.FromJson<GameData>(Encoding.UTF8.GetString(decrypted));

            }
            catch (Exception e) 
            {
                Debug.LogError("Error when loading data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data) 
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // get the directory
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the data to json
            string dataToStore = JsonUtility.ToJson(data, true);
            byte[] bytes = Encoding.ASCII.GetBytes(dataToStore);
            byte[] encrypted = Encode(bytes, key, iv);

            // write the serialized data to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(Convert.ToBase64String(encrypted));
                }
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Error when saving data to file: " + fullPath + "\n" + e);
        }
    }

}
