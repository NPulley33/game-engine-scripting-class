using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorSaver : MonoBehaviour
{
    [SerializeField] TMP_InputField NumberInput;    
    [SerializeField] TMP_InputField NameInput;    
    [SerializeField] TMP_InputField RewardInput;    
    [SerializeField] TMP_InputField EnemyInput;

    public LevelSaveData saveData;

    private void Awake()
    {

    }

    public void SaveData()
    {
        saveData.LevelName = NameInput.text;
        saveData.LevelNumber = Int32.Parse(NumberInput.text);
        saveData.Reward = Int32.Parse(RewardInput.text);
        saveData.NumOfEnemies = Int32.Parse(EnemyInput.text);

        string path;
        if (saveData.LevelNumber < 10) path = $"Assets/Resources/Levels/Level_0{saveData.LevelNumber}";
        else path = $"Assets/Resources/Levels/Level_{saveData.LevelNumber}";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(JsonUtility.ToJson(saveData));
        writer.Close();

        AssetDatabase.ImportAsset(path);

    }

}
