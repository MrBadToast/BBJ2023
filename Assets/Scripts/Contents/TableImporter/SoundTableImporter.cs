#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "SoundImporter", menuName = "TableImporter/SoundTableImporter", order = 0)]
public class SoundTableImporter : ExcelImporter
{
    [SerializeField]
    [FolderPath]
    private string audioPrefabFolerPath;

    [SerializeField]
    [FolderPath]
    private string audioClipFolerPath;

    [Button("Audio Prefab 최신화")]
    private void CreateAudioData()
    {
        for (var i = 0; i < loadDataContainer.Count; ++i)
        {
            var dataDic = loadDataContainer[i];
            var audioPlayerFileName = $"{dataDic["Type"]}Player_{dataDic["ContainerName"]}_{dataDic["Key"]}";
            var audioPlayerPath = $"{audioPrefabFolerPath}/{dataDic["Type"]}/{audioPlayerFileName}.prefab";

            var audioPlayer = (GameObject)AssetDatabase.LoadAssetAtPath(audioPlayerPath, typeof(GameObject));

            var audioClipPath = $"{audioClipFolerPath}/{dataDic["Type"]}/{dataDic["FileName"]}";
            var audioClip = (AudioClip)AssetDatabase.LoadAssetAtPath(audioClipPath, typeof(AudioClip));

            //이미 파일이 존재하지 않는 경우, 생성합니다.
            if (audioPlayer == null)
            {
                audioPlayer = new GameObject(audioPlayerFileName);
                switch (dataDic["Type"])
                {
                    case "BGM":
                        audioPlayer.AddComponent<BGMPlayer>();
                        break;
                    case "SFX":
                        audioPlayer.AddComponent<SFXPlayer>();
                        break;
                }
                PrefabUtility.SaveAsPrefabAsset(audioPlayer, audioPlayerPath);
            }

            switch (dataDic["Type"])
            {
                case "BGM":
                    {
                        var bgmPlayer = audioPlayer.GetComponent<BGMPlayer>();
                        bgmPlayer.SetAudioClip(audioClip);
                        bgmPlayer.SetVolume(float.Parse(dataDic["Volume"]));
                        EditorUtility.SetDirty(bgmPlayer);
                    }
                    break;
                case "SFX":
                    {
                        var sfxPlayer = audioPlayer.GetComponent<SFXPlayer>();
                        sfxPlayer.SetAudioClip(audioClip);
                        sfxPlayer.SetVolume(float.Parse(dataDic["Volume"]));
                        sfxPlayer.isLoop = bool.Parse(dataDic["IsLoop"]);
                        EditorUtility.SetDirty(sfxPlayer);
                    }
                    break;
            }

            EditorUtility.SetDirty(audioPlayer);
        }

        AssetDatabase.Refresh();
        EditorApplication.RepaintProjectWindow();
        EditorApplication.RepaintHierarchyWindow();
    }
}
#endif