using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class PrefabBrushWindow : EditorWindow
{
    public struct PlacecdObject
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public GameObject prefab;
    }

    private GUIStyle labelStyle;
    private Vector2 scrollPos = Vector2.zero;

    static System.Type type_HandleUtility;
    static MethodInfo meth_IntersectRayMesh;

    private bool isDraw = true;
    private bool isMouseDown = false;
    private bool isStraightMode = false;

    private bool useVertexNormal = true;

    private int drawDensity = 1;
    private bool useRandomDensity = false;
    private AmountRangeInt randomDensityRange = new AmountRangeInt(1, 10);

    private Vector3 prevPoint = Vector3.zero;
    private List<Vector3> drawPointList = new List<Vector3>();
    private List<Vector3> drawNormalPointList = new List<Vector3>();
    private List<Vector3> drawDetailPointList = new List<Vector3>();

    private bool useCurveLookAt = true;

    private bool useWorldOffset = true;
    private Vector3 worldOffsetPosition;

    private bool useWorldRandomOffsetRange = false;
    private AmountRangeVector3 randomWorldPositionRange = new AmountRangeVector3(Vector3.zero, Vector3.zero);

    private bool useLocalOffset = true;
    private Vector3 localOffsetPosition;
    private Vector3 localOffsetRotation;

    private bool useAxisByLocalOffsetScale = false;
    private Vector3 localOffsetScale;

    private bool useLocalRandomOffsetRange = false;
    private AmountRangeVector3 randomLocalPositionRange = new AmountRangeVector3(Vector3.zero, Vector3.zero);
    private AmountRangeVector3 randomLocalRotationRange = new AmountRangeVector3(Vector3.zero, Vector3.zero);

    private bool useAxisByLocalScaleRange = false;
    private AmountRangeVector3 randomLocalScaleRange = new AmountRangeVector3(Vector3.zero, Vector3.zero);


    private bool showColorOptions = true;
    private bool showDrawOptions = true;
    private bool showOffsetOptions = true;
    private bool showGuideMessages = true;

    private PrefabBrushData prefabBrushData;
    private SerializedObject prefabBrushDataSO;

    private SerializedObject settingDataSO;

    private ReorderableList reorderableList;

    private static Transform brushTransfom;
    private Material brushMaterial;

    private List<PlacecdObject> placecdObjectList = new List<PlacecdObject>();

    [MenuItem("Window/Prefab Brush")]
    private static void Init()
    {
        PrefabBrushWindow window = (PrefabBrushWindow)GetWindow(typeof(PrefabBrushWindow), false, "PrefabBrush", true);
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= UpdateScene;
        SceneView.duringSceneGui += UpdateScene;

        var editorTypes = typeof(Editor).Assembly.GetTypes();

        type_HandleUtility = editorTypes.FirstOrDefault(t => t.Name == "HandleUtility");
        meth_IntersectRayMesh = type_HandleUtility.GetMethod("IntersectRayMesh", (BindingFlags.Static | BindingFlags.NonPublic));

        InitializeSetting();
        CreateBrushObject();

        if (prefabBrushData != null)
        {
            RefreshBrushData();
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= UpdateScene;
        if (brushTransfom != null)
        {
            DestroyImmediate(brushTransfom.gameObject);
        }
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = Color.white;
            labelStyle.richText = true;
        }

        EditorGUILayout.LabelField("<b>Prefab Brush 1.0</b>", labelStyle);
        showGuideMessages = EditorGUILayout.Toggle("Show Guide Message",showGuideMessages);
        EditorGUILayout.Space(10);

        EditorGUIUtility.labelWidth = 200;

        EditorGUI.indentLevel++;

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("Scene 드로잉 기능을 켜고 끕니다.", MessageType.Info, true);
        }

        isDraw = EditorGUILayout.Toggle("Drawable", isDraw);

        EditorGUILayout.Space(10);
        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("랜덤 범위 밀도 값 사용 여부.", MessageType.Info, true);
        }
        useRandomDensity = EditorGUILayout.Toggle("Use Density Range", useRandomDensity);

        EditorGUI.indentLevel++;
        if (useRandomDensity)
        {
            if (showGuideMessages)
            {
                EditorGUILayout.HelpBox("밀도의 랜덤 범위 값", MessageType.Info, true);
            }
            randomDensityRange.min = EditorGUILayout.IntField("Draw Density Min", randomDensityRange.min);
            randomDensityRange.max = EditorGUILayout.IntField("Draw Density Max", randomDensityRange.max);
        }
        else
        {
            if (showGuideMessages)
            {
                EditorGUILayout.HelpBox("밀도의 값", MessageType.Info, true);
            }
            drawDensity = EditorGUILayout.IntField("Draw Density", drawDensity);
        }
        EditorGUI.indentLevel--;

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("오브젝트 위치 간격(거리 단위 : M)", MessageType.Info, true);
        }
        EditorGUILayout.Slider(settingDataSO.FindProperty("stepDistance"), 0.01f, 100f, new GUIContent("Step Distance"));

        EditorGUILayout.Space(10);

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("컬러 옵션", MessageType.Info, true);
        }
        showColorOptions = EditorGUILayout.Foldout(showColorOptions, "Color Options");
        if (showColorOptions)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(settingDataSO.FindProperty("drawBrushColor"), new GUIContent("Draw Brush Color"));
            if (EditorGUI.EndChangeCheck())
            {
                UpdateBrushColor();
            }
            EditorGUILayout.PropertyField(settingDataSO.FindProperty("drawLineColor"), new GUIContent("Draw Line Color"));
            EditorGUILayout.PropertyField(settingDataSO.FindProperty("drawNormalColor"), new GUIContent("Draw Normal Color"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("드로잉 옵션", MessageType.Info, true);
        }
        showDrawOptions = EditorGUILayout.Foldout(showDrawOptions, "Draw Options");
        if (showDrawOptions)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.Slider(settingDataSO.FindProperty("drawBrushSize"), 1f, 100f, new GUIContent("Draw Brush Size"));
            EditorGUILayout.Slider(settingDataSO.FindProperty("drawStroke"), 1f, 100f, new GUIContent("Draw Stroke"));
            EditorGUILayout.Slider(settingDataSO.FindProperty("drawNormalHeightOffset"), 1f, 100f, new GUIContent("Draw Normal Height Offset"));
            EditorGUILayout.IntSlider(settingDataSO.FindProperty("drawDetail"), 1, 100, new GUIContent("Draw Detail"));
            EditorGUI.indentLevel--;
        }

        settingDataSO.ApplyModifiedProperties();
        settingDataSO.Update();

        EditorGUILayout.Space(10);

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("오프셋 옵션", MessageType.Info, true);
        }
        showOffsetOptions = EditorGUILayout.Foldout(showOffsetOptions, "Offset Options");
        if (showOffsetOptions)
        {
            EditorGUI.indentLevel++;

            if (showGuideMessages)
            {
                EditorGUILayout.HelpBox("월드 좌표계 기준 오프셋 사용 여부", MessageType.Info, true);
            }
            useWorldOffset = EditorGUILayout.Toggle("Use WorldOffset", useWorldOffset);
            if (useWorldOffset)
            {
                worldOffsetPosition = EditorGUILayout.Vector3Field("World Offset Position", worldOffsetPosition);
                if (showGuideMessages)
                {
                    EditorGUILayout.HelpBox("랜덤 옵션 사용 여부", MessageType.Info, true);
                }
                useWorldRandomOffsetRange = EditorGUILayout.Toggle("Use World Random", useWorldRandomOffsetRange);
                if (useWorldRandomOffsetRange)
                {
                    EditorGUI.indentLevel++;
                    randomWorldPositionRange.min = EditorGUILayout.Vector3Field("Random World Position Min", randomWorldPositionRange.min);
                    randomWorldPositionRange.max = EditorGUILayout.Vector3Field("Random World Position Max", randomWorldPositionRange.max);
                    EditorGUI.indentLevel--;
                }
            }

            if (showGuideMessages)
            {
                EditorGUILayout.HelpBox("오브젝트 기준 오프셋 사용 여부", MessageType.Info, true);
            }
            useLocalOffset = EditorGUILayout.Toggle("Use LocalOffset", useLocalOffset);
            if (useLocalOffset)
            {
                localOffsetPosition = EditorGUILayout.Vector3Field("Local Offset Position", localOffsetPosition);
                localOffsetRotation = EditorGUILayout.Vector3Field("Local Offset Rotation", localOffsetRotation);
                if (showGuideMessages)
                {
                    EditorGUILayout.HelpBox("크기 조정 시 좌표계 별 분리 여부", MessageType.Info, true);
                }
                useAxisByLocalOffsetScale = EditorGUILayout.Toggle("Use Axis Scale", useAxisByLocalOffsetScale);
                if (useAxisByLocalOffsetScale)
                {
                    if (showGuideMessages)
                    {
                        EditorGUILayout.HelpBox("좌표계 별 크기 조정", MessageType.Info, true);
                    }
                    localOffsetScale = EditorGUILayout.Vector3Field("Local Offset Scale", localOffsetScale);
                }
                else
                {
                    if (showGuideMessages)
                    {
                        EditorGUILayout.HelpBox("전체 크기 조정", MessageType.Info, true);
                    }
                    localOffsetScale.x = EditorGUILayout.FloatField("Local Offset Scale", localOffsetScale.x);
                    localOffsetScale.z = localOffsetScale.y = localOffsetScale.x;
                }
                if (showGuideMessages)
                {
                    EditorGUILayout.HelpBox("랜덤 옵션 사용 여부", MessageType.Info, true);
                }
                useLocalRandomOffsetRange = EditorGUILayout.Toggle("Use Local Random", useLocalRandomOffsetRange);
                if (useLocalRandomOffsetRange)
                {
                    EditorGUI.indentLevel++;
                    randomLocalPositionRange.min = EditorGUILayout.Vector3Field("Random Local Position Min", randomLocalPositionRange.min);
                    randomLocalPositionRange.max = EditorGUILayout.Vector3Field("Random Local Position Max", randomLocalPositionRange.max);

                    EditorGUILayout.Space(10);

                    randomLocalRotationRange.min = EditorGUILayout.Vector3Field("Random Local Rotation Min", randomLocalRotationRange.min);
                    randomLocalRotationRange.max = EditorGUILayout.Vector3Field("Random Local Rotation Max", randomLocalRotationRange.max);

                    EditorGUILayout.Space(10);
                    if (showGuideMessages)
                    {
                        EditorGUILayout.HelpBox("크기 조정 시 좌표계 별 분리 여부", MessageType.Info, true);
                    }
                    useAxisByLocalScaleRange = EditorGUILayout.Toggle("Use Axis Scale", useAxisByLocalScaleRange);
                    if (useAxisByLocalScaleRange)
                    {
                        if (showGuideMessages)
                        {
                            EditorGUILayout.HelpBox("좌표계 별 크기 조정", MessageType.Info, true);
                        }
                        randomLocalScaleRange.min = EditorGUILayout.Vector3Field("Random Local Scale Min", randomLocalScaleRange.min);
                        randomLocalScaleRange.max = EditorGUILayout.Vector3Field("Random Local Scale Max", randomLocalScaleRange.max);
                    }
                    else
                    {
                        if (showGuideMessages)
                        {
                            EditorGUILayout.HelpBox("전체 크기 조정", MessageType.Info, true);
                        }
                        randomLocalScaleRange.min.x = EditorGUILayout.FloatField("Random Local Scale Min", randomLocalScaleRange.min.x);
                        randomLocalScaleRange.min.z = randomLocalScaleRange.min.y = randomLocalScaleRange.min.x;
                        randomLocalScaleRange.max.x = EditorGUILayout.FloatField("Random Local Scale Max", randomLocalScaleRange.max.x);
                        randomLocalScaleRange.max.z = randomLocalScaleRange.max.y = randomLocalScaleRange.max.x;
                    }


                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);
        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("드로잉 방향에 따라 오브젝트 회전 여부", MessageType.Info, true);
        }
        useCurveLookAt = EditorGUILayout.Toggle("Use Curve LookAt", useCurveLookAt);

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("3D 모델의 Normal 방향에 맞추어 회전 여부", MessageType.Info, true);
        }
        useVertexNormal = EditorGUILayout.Toggle("Use Vertex Normal", useVertexNormal);

        EditorGUILayout.Space(10);

        if (showGuideMessages)
        {
            EditorGUILayout.HelpBox("브러쉬 데이터 파일", MessageType.Info, true);
        }
        EditorGUI.BeginChangeCheck();
        prefabBrushData = EditorGUILayout.ObjectField("BrushData", prefabBrushData, typeof(PrefabBrushData), false) as PrefabBrushData;
        if (EditorGUI.EndChangeCheck())
        {
            if (prefabBrushData != null)
            {
                RefreshBrushData();
            }
            else
            {
                prefabBrushDataSO = null;
            }
        }

        if (prefabBrushDataSO != null)
        {
            prefabBrushDataSO.Update();
            reorderableList.DoLayoutList();
            prefabBrushDataSO.ApplyModifiedProperties();
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.EndScrollView();
    }

    private void InitializeSetting()
    {
        var settingData = PrefabBrushSettings.GetOrCreateSettings();
        settingDataSO = new SerializedObject(settingData);
    }

    private void CreateBrushObject()
    {
        if (brushTransfom == null)
        {
            var brushObject = GameObject.Find("[PrefabBrushObject]");

            if (brushObject == null)
            {
                brushObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                brushObject.name = "[PrefabBrushObject]";
                DestroyImmediate(brushObject.GetComponent<Collider>());

                brushMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
                var brushColor = settingDataSO.FindProperty("drawBrushColor").colorValue;
                brushMaterial.SetColor("_Color", brushColor);
                brushMaterial.hideFlags = HideFlags.HideAndDontSave;

                Renderer renderer = brushObject.GetComponent<Renderer>();
                renderer.sharedMaterial = brushMaterial;
            }

            brushObject.hideFlags = HideFlags.HideAndDontSave;

            brushTransfom = brushObject.transform;
            brushTransfom.rotation = Quaternion.identity;
            brushTransfom.localScale = Vector3.one * Mathf.Max(1f, settingDataSO.FindProperty("drawBrushSize").floatValue);
        }
    }

    private void UpdateBrushObject(Vector3 point)
    {
        brushTransfom.position = point;
        brushTransfom.localScale = Vector3.one * Mathf.Max(1f, settingDataSO.FindProperty("drawBrushSize").floatValue);
    }

    private void UpdateBrushColor()
    {
        brushMaterial.SetColor("_Color", settingDataSO.FindProperty("drawBrushColor").colorValue);
    }

    private void UpdateScene(SceneView sceneView)
    {
        DisplayLines();

        if (isDraw)
        {
            Event e = Event.current;
            ProcecssKeyInput(sceneView, e);
            ProcessMouseInput(sceneView, e);
        }

    }

    private void DisplayLines()
    {
        var stroke = settingDataSO.FindProperty("drawStroke").floatValue;

        Handles.color = settingDataSO.FindProperty("drawLineColor").colorValue;
        for (var i = 0; i < drawDetailPointList.Count - 1; ++i)
        {
            Handles.DrawLine(drawDetailPointList[i], drawDetailPointList[i + 1], stroke);
        }

        Handles.color = settingDataSO.FindProperty("drawNormalColor").colorValue;
        for (var i = 0; i < drawNormalPointList.Count; ++i)
        {
            Handles.DrawLine(drawPointList[i], drawPointList[i] + drawNormalPointList[i] * settingDataSO.FindProperty("drawNormalHeightOffset").floatValue, stroke);
        }
    }

    private void ProcecssKeyInput(SceneView sceneView, Event e)
    {
        if (e.keyCode == KeyCode.None)
            return;

        switch (e.keyCode)
        {
            case KeyCode.LeftShift:
                {
                    if (e.type == EventType.KeyDown)
                    {
                        isStraightMode = true;
                    }
                    else if (e.type == EventType.KeyUp)
                    {
                        isStraightMode = false;
                    }
                }
                break;


        }
    }

    private void ProcessMouseInput(SceneView sceneView, Event e)
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);

        var mouseRay = GetRayFromMouse(sceneView);
        RaycastHit hit;
        if (IntersectRayMeshInSceneView(mouseRay, out hit))
        {
            UpdateBrushObject(hit.point);
            sceneView.Repaint();
            if (e.button == 0 && prefabBrushData != null)
            {
                switch (e.type)
                {
                    case EventType.MouseMove:
                    case EventType.MouseDrag:
                    case EventType.Repaint:
                    case EventType.Layout:
                        if (!isMouseDown)
                            return;

                        var stepDistance = settingDataSO.FindProperty("stepDistance").floatValue;

                        if (isStraightMode)
                        {
                            if (drawPointList.Count > 2)
                            {
                                drawPointList.RemoveRange(1, drawPointList.Count - 1);
                                drawNormalPointList.RemoveRange(1, drawNormalPointList.Count - 1);
                            }

                            var pivot = drawPointList[0];
                            var diffVec = hit.point - pivot;
                            var distance = diffVec.magnitude;
                            var direction = diffVec.normalized;

                            RaycastHit automaticHit;

                            for (var i = 1; ; ++i)
                            {
                                var splitDistance = stepDistance * i;
                                if (splitDistance > distance)
                                {
                                    break;
                                }
                                var splitWorldPosition = pivot + direction * splitDistance;

                                var ray = new Ray(sceneView.camera.transform.position, (splitWorldPosition - sceneView.camera.transform.position).normalized);
                                if (IntersectRayMeshInSceneView(ray, out automaticHit))
                                {
                                    drawPointList.Add(automaticHit.point);
                                    drawNormalPointList.Add(automaticHit.normal.normalized);
                                }
                            }
                        }
                        else
                        {
                            var distance = (prevPoint - hit.point).magnitude;
                            if (distance >= stepDistance)
                            {
                                prevPoint = hit.point;
                                drawPointList.Add(hit.point);
                                drawNormalPointList.Add(hit.normal.normalized);
                            }
                        }

                        RefreshSplineDraw();
                        HandleUtility.AddDefaultControl(id);
                        break;

                    case EventType.MouseDown:
                        prevPoint = hit.point;
                        drawPointList.Add(hit.point);
                        drawNormalPointList.Add(hit.normal.normalized);

                        isMouseDown = true;
                        GUIUtility.hotControl = id;
                        e.Use();
                        break;

                    case EventType.MouseUp:
                        if (!isMouseDown)
                            return;

                        var brushSize = settingDataSO.FindProperty("drawBrushSize").floatValue;

                        for (var i = 0; i < drawPointList.Count; ++i)
                        {
                            var density = drawDensity;
                            if (useRandomDensity)
                            {
                                density = randomDensityRange.GetRandomAmount();
                            }

                            for (var k = 0; k < density; ++k)
                            {
                                var rayPoint = drawPointList[i] + Random.insideUnitSphere * (brushSize > 1f ? brushSize * 0.5f : 0f);
                                var ray = new Ray(sceneView.camera.transform.position, (rayPoint - sceneView.camera.transform.position).normalized);
                                RaycastHit automaticHit;
                                if (IntersectRayMeshInSceneView(ray, out automaticHit))
                                {
                                    var selectPrefab = prefabBrushData.GetRandomPrefab();

                                    var placePosition = automaticHit.point;
                                    var normalPoint = automaticHit.normal.normalized;
                                    var placeRotation = Quaternion.identity;
                                    var placeScale = selectPrefab.transform.localScale;

                                    if (useVertexNormal)
                                    {
                                        placeRotation = Quaternion.FromToRotation(Vector3.up, normalPoint);
                                    }

                                    if (useCurveLookAt)
                                    {
                                        var diffVector = Vector3.zero;

                                        if (i < drawPointList.Count - 1)
                                        {
                                            diffVector = drawPointList[i + 1] - drawPointList[i];
                                        }
                                        else if (drawPointList.Count > 1)
                                        {
                                            diffVector = drawPointList[i] - drawPointList[i - 1];
                                        }
                                        diffVector.Normalize();

                                        var lookAtRotation = Quaternion.LookRotation(diffVector, normalPoint);
                                        lookAtRotation.x = 0;
                                        lookAtRotation.z = 0;

                                        placeRotation *= lookAtRotation;
                                    }

                                    if (useWorldOffset)
                                    {
                                        placePosition += worldOffsetPosition;

                                        if (useWorldRandomOffsetRange)
                                        {
                                            placePosition += randomWorldPositionRange.GetRandomAmount();
                                        }
                                    }

                                    if (useLocalOffset)
                                    {
                                        var localPosition = localOffsetPosition;
                                        var localRoation = localOffsetRotation;
                                        var localScale = localOffsetScale;

                                        if (useLocalRandomOffsetRange)
                                        {
                                            localPosition += randomLocalPositionRange.GetRandomAmount();
                                            localRoation += randomLocalRotationRange.GetRandomAmount();
                                            localScale += useAxisByLocalScaleRange ?
                                                randomLocalScaleRange.GetRandomAmount() : Vector3.one * Random.Range(randomLocalScaleRange.min.x, randomLocalScaleRange.max.x);
                                        }

                                        placePosition += placeRotation * localPosition;
                                        placeRotation *= Quaternion.Euler(localRoation);
                                        placeScale += localScale;
                                    }

                                    placecdObjectList.Add(new PlacecdObject()
                                    {
                                        position = placePosition,
                                        rotation = placeRotation,
                                        scale = placeScale,
                                        prefab = selectPrefab
                                    });
                                }
                            }
                        }

                        foreach (var item in placecdObjectList)
                        {
                            var placecdObject = (GameObject)PrefabUtility.InstantiatePrefab(item.prefab);
                            placecdObject.transform.SetPositionAndRotation(item.position, item.rotation);
                            placecdObject.transform.localScale = item.scale;

                            Undo.RegisterCreatedObjectUndo(placecdObject, "Undo PrefabBrush");
                        }

                        placecdObjectList.Clear();
                        drawPointList.Clear();
                        drawNormalPointList.Clear();
                        drawDetailPointList.Clear();

                        GUIUtility.hotControl = 0;
                        e.Use();
                        isMouseDown = false;
                        break;
                }
            }
        }
    }

    private Ray GetRayFromMouse(SceneView sceneView)
    {
        Vector3 mousePosition = Event.current.mousePosition;

        float mult = 1;
#if UNITY_5_4_OR_NEWER
        mult = EditorGUIUtility.pixelsPerPoint;
#endif

        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * mult;
        mousePosition.x *= mult;

        return sceneView.camera.ScreenPointToRay(mousePosition);
    }

    private void RefreshBrushData()
    {
        prefabBrushDataSO = new SerializedObject(prefabBrushData);
        reorderableList = new ReorderableList(prefabBrushDataSO, prefabBrushDataSO.FindProperty("brushElementList"), true, true, true, true);

        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Prefab List");
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += 2f;
            GUIContent objectLabel = new GUIContent($"Item {index}");
            EditorGUI.PropertyField(rect, reorderableList.serializedProperty.GetArrayElementAtIndex(index), objectLabel, true);
        };
        reorderableList.elementHeightCallback = (index) =>
        {
            return GetHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index));
        };
    }

    private void RefreshSplineDraw()
    {
        var drawDetail = settingDataSO.FindProperty("drawDetail").intValue;
        drawDetailPointList.Clear();
        for (var i = 0; i < drawPointList.Count; ++i)
        {
            drawDetailPointList.Add(drawPointList[i]);
        }
        /*
            for (var i = 0; i < drawPointList.Count - 3; ++i)
            {
                for (var j = 1; j <= drawDetail; ++j)
                {
                    float t = j * (1.0f / drawDetail);
                    var splinePoint = SplineUtility.CatmullRomSplineInterp(drawPointList[i], drawPointList[i + 1], drawPointList[i + 2], drawPointList[i + 3], t);

                    drawDetailPointList.Add(splinePoint);
                }
            }
        */
    }

    private float GetHeight(SerializedProperty brushElement)
    {
        var isExpended = brushElement.isExpanded;

        var height = EditorGUIUtility.singleLineHeight;

        if (isExpended)
        {
            height += EditorGUIUtility.singleLineHeight * 3;
        }

        return height;
    }

    private bool IntersectRayMeshInSceneView(Ray ray, out RaycastHit hit)
    {
        RaycastHit resultHit = new RaycastHit();
        hit = resultHit;

        MeshFilter[] meshFilters = GameObject.FindObjectsOfType<MeshFilter>();
        var nearestDistance = float.MaxValue;
        foreach (var meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;

            if (mesh == null)
                continue;

            if (IntersectRayMesh(ray, mesh, meshFilter.transform.localToWorldMatrix, out resultHit))
            {
                if (resultHit.distance < nearestDistance)
                {
                    hit = resultHit;
                    nearestDistance = resultHit.distance;
                }
            }
        }

        return nearestDistance != float.MaxValue;
    }

    private bool IntersectRayMesh(Ray ray, MeshFilter meshFilter, out RaycastHit hit)
    {
        return IntersectRayMesh(ray, meshFilter.mesh, meshFilter.transform.localToWorldMatrix, out hit);
    }

    private bool IntersectRayMesh(Ray ray, Mesh mesh, Matrix4x4 matrix, out RaycastHit hit)
    {
        var parameters = new object[] { ray, mesh, matrix, null };
        bool result = (bool)meth_IntersectRayMesh.Invoke(null, parameters);
        hit = (RaycastHit)parameters[3];

        return result;
    }
}
