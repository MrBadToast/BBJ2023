using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "BiomeContainer", menuName = "Biome/BiomeContainer", order = 0)]
public class BiomeContainer : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<string, BiomeData> dataTable;

    public BiomeData GetBiome(string key)
    {
        if (dataTable.ContainsKey(key))
        {
            return dataTable[key];
        }

        return null;
    }


    public BiomeData GetBiome(StatusInfo statusInfo)
    {
        foreach (var item in dataTable)
        {
            var isContains = item.Value.ContainStatus(statusInfo);

            if (isContains)
            {
                return item.Value;
            }
        }

        return null;
    }

    [Button("파일 자동 탐색")]
    private void AutoSetupByName(string nameTag)
    {
#if UNITY_EDITOR
        var findTable = new SerializableDictionary<string, BiomeData>();
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"{nameTag} t:BiomeData", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var fileName = System.IO.Path.GetFileName(assetPath);
            var file = (BiomeData)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(BiomeData));
            findTable.Add(fileName.Replace(".asset", ""), file);
        }

        dataTable.Clear();

        foreach (var item in findTable)
        {
            var key = item.Key.ToString().Replace($"{nameTag}", "");
            dataTable.Add(key, item.Value);
        }

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.EditorApplication.RepaintProjectWindow();
        UnityEditor.EditorApplication.RepaintHierarchyWindow();
#endif
    }
}
