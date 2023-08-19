using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CollectionContainer", menuName = "Collcetion/CollectionContainer", order = 0)]

public class CollectionContainer : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<string, CreatureData> dataTable;
    public SerializableDictionary<string, CreatureData> DataTable => dataTable;


    [Button("파일 자동 탐색")]
    private void AutoSetupByName(string nameTag)
    {
#if UNITY_EDITOR
        var findTable = new SerializableDictionary<string, CreatureData>();
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"{nameTag} t:CreatureData", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var fileName = System.IO.Path.GetFileName(assetPath);
            var file = (CreatureData)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(CreatureData));
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
