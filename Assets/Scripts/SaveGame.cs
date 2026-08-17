using System;
using System.IO;
using UnityEngine;

//save data into text
public class SaveGame : MonoBehaviour
{
    private string fileName = "SaveData.txt";
    private string absolutePath = "/Config.txt";

    [SerializeField]
    private GameData rawData;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath + absolutePath);
        ReadFile();
    }

    private void ReadFile()
    {

        if (File.Exists(Application.persistentDataPath + absolutePath))
        {
            string content = File.ReadAllText(Application.persistentDataPath + absolutePath);
            rawData.ConvertRawData(content);
        }
        else
        {
            Debug.LogError("File not found at: " + Application.persistentDataPath + absolutePath);
        }
    }


    public void SaveProgress(float _score)
    {
        string data = DateTime.Now.ToString() + "  " + _score.ToString(); 

        SaveTextToFile(data, fileName);
    }

    public void SaveTextToFile(string content, string fileName)
    {
        // 1. Define a safe path across different platforms
        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            // 2. Write the text (overwrites the file if it already exists)
            File.AppendAllText(path, content + System.Environment.NewLine);
            Debug.Log($"File successfully saved to: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save file: {e.Message}");
        }
    }
}
