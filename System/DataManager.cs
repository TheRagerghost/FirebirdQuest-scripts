using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public List<string> key_values = new();

    [Header("System")]
    public string saveFolder = "Firebird Quest";
    public string filename = "save";
    string savePath;
    string filePath;

    public UnityEvent OnKeysUpdated;

    void Start()
    {
        savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + saveFolder;
        filePath = savePath + "/" + filename + ".txt";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKey(string k)
    {
        if (!key_values.Contains(k)) key_values.Add(k);
        OnKeysUpdated.Invoke();
    }

    public void SaveGame()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllLines(filePath, key_values);
    }

    public void LoadGame()
    {
        List<string> temp = new();

        if (File.Exists(filePath))
        {
            string[] fileData = File.ReadAllLines(filePath);
            temp.AddRange(fileData);
        }

        key_values = temp;
    }

    public bool ContainsAllKeys(List<string> col)
    {
        foreach (string key in col)
        {
            if (!key_values.Contains(key)) return false;
        }
        return true;
    }
}
