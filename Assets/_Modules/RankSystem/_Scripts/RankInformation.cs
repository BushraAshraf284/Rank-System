using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RanksInformation", order = 1)]
public class RankInformation : ScriptableObject
{
    public RankInfo rankInfo;

#if UNITY_EDITOR

    [Button("Restore Data", ButtonSizes.Medium), GUIColor(0.133f, 0.655f, 0.941f, 1)]
    private void RestoreData()
    {
        if (File.Exists(GetPath()))
        {
            string fileContent = File.ReadAllText(GetPath());
            rankInfo = JsonUtility.FromJson<RankInfo>(fileContent);
        }
        else
        {
            Debug.Log("File Doesn't exists");
        }
    }

    [Button("Save Data", ButtonSizes.Medium), GUIColor(0.133f, 0.655f, 0.941f, 1)]
    private void SaveData()
    {
        string data = JsonUtility.ToJson(rankInfo, true);
        Debug.Log("Save Data" + data);
        File.WriteAllText(GetPath(), data);
    }

#endif

    public string GetPath()
    {
        return "Assets/RankInformation.json";
    }
}

[System.Serializable]
public class RankInfo
{
    public List<Rank> rankList;
}


