using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

class PrefabBrushSettings : ScriptableObject
{
    public const string SettingPath = "Assets/Editor/PrefabBrush/PrefabBrushSettings.asset";

    [SerializeField]
    private float stepDistance = 10f;

    [SerializeField]
    private Color drawBrushColor = Color.green;
    [SerializeField]
    private Color drawLineColor = Color.cyan;
    [SerializeField]
    private Color drawNormalColor = Color.red;

    [SerializeField]
    private float drawBrushSize = 10f;
    [SerializeField]
    private float drawStroke = 10f;
    [SerializeField]
    private float drawNormalHeightOffset = 10f;
    [SerializeField]
    private int drawDetail = 50;

    internal static PrefabBrushSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<PrefabBrushSettings>(SettingPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<PrefabBrushSettings>();

            settings.drawBrushColor.a = 0.5f;
            settings.drawLineColor.a = 0.75f;
            settings.drawNormalColor.a = 0.75f;

            AssetDatabase.CreateAsset(settings, SettingPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }
}

static class PrefabBrushSettingsIMGUIRegister
{
    [SettingsProvider]
    public static SettingsProvider CreateSettingProvider()
    {
        var path = "Preferences/PrefabBrush/Setting";

        var provider = new SettingsProvider(path, SettingsScope.User)
        {
            label = "PrefabBrush Setting",

            guiHandler = (searchContext) =>
            {
                var settings = PrefabBrushSettings.GetSerializedSettings();

                EditorGUILayout.LabelField("Prefab Brush의 옵션을 변경 할 수 있습니다.");

                EditorGUILayout.Space(10);

                EditorGUILayout.Slider(settings.FindProperty("stepDistance"), 0.01f, 100f, new GUIContent("Step Distance"));

                EditorGUILayout.Space(10);

                EditorGUILayout.PropertyField(settings.FindProperty("drawBrushColor"), new GUIContent("Draw Brush Color"));
                EditorGUILayout.PropertyField(settings.FindProperty("drawLineColor"), new GUIContent("Draw Line Color"));
                EditorGUILayout.PropertyField(settings.FindProperty("drawNormalColor"), new GUIContent("Draw Normal Color"));

                EditorGUILayout.Space(10);

                EditorGUILayout.Slider(settings.FindProperty("drawBrushSize"), 1f, 100f, new GUIContent("Draw Brush Size"));
                EditorGUILayout.Slider(settings.FindProperty("drawStroke"), 1f, 100f, new GUIContent("Draw Stroke"));
                EditorGUILayout.Slider(settings.FindProperty("drawNormalHeightOffset"), 1f, 100f, new GUIContent("Draw Normal Height Offset"));
                EditorGUILayout.IntSlider(settings.FindProperty("drawDetail"), 1, 100, new GUIContent("Draw Detail"));

                EditorGUILayout.Space(10);

                settings.ApplyModifiedProperties();
            },

            // Populate the search keywords to enable smart search filtering and label highlighting:
            keywords = new HashSet<string>(new[] { "PrefabBrush" })
        };

        return provider;
    }
}


