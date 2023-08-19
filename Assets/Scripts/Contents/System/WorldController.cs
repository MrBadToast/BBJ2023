using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    private BiomeContainer biomeContainer;

    [SerializeField]
    private string currentBiomeKey = "Default";
    [SerializeField]
    private BiomeController biomeController;

    [SerializeField]
    private List<MeshRenderer> surfaceMeshRendererList;
    [SerializeField]
    private List<MeshRenderer> groundMeshRendererList;

    [SerializeField]
    private StatusInfo statusInfo;

    [SerializeField]
    private float changeBiomeTime = 5f;
    private float currentChangeBiomeTime;

    public UnityEvent<StatusInfo> updateStatusEvent;
    public UnityEvent<BiomeData> changeBiomeEvent;


    [SerializeField]
    private float colorSurfaceLerpTime = 1f;
    private MaterialPropertyBlock mpbSurface;
    [SerializeField]
    private float colorGroundLerpTime = 1f;
    private MaterialPropertyBlock mpbGround;

    private Coroutine updateSurfaceColor;
    private Coroutine updateGroundColor;

    private void Awake()
    {
        mpbSurface = new MaterialPropertyBlock();
        mpbGround = new MaterialPropertyBlock();
    }

    private void Start()
    {
        currentChangeBiomeTime = changeBiomeTime;

        changeBiomeEvent.AddListener(ChangeGroundColor);
        changeBiomeEvent.AddListener(ChangeSurfaceColor);
    }

    private void Update()
    {
        currentChangeBiomeTime -= Time.deltaTime;

        if (currentChangeBiomeTime < 0f)
        {
            var resultBiome = biomeContainer.GetBiome(statusInfo);
            Debug.Log($"Filter Biome Result {resultBiome}");

            if (resultBiome == null)
            {
                resultBiome = biomeContainer.GetBiome("Default");
            }

            if (!currentBiomeKey.Equals(resultBiome.Key))
            {
                biomeController.Hide();

                //Biome Update
                Debug.Log($"Change Biome => {resultBiome.Key}");

                var biomeEnviroment = Instantiate(resultBiome.EnviromentPrefab);
                biomeController = biomeEnviroment.GetComponent<BiomeController>();
                biomeController.Show();
                currentBiomeKey = resultBiome.Key;
                changeBiomeEvent?.Invoke(resultBiome);
            }

            currentChangeBiomeTime = changeBiomeTime;
        }
    }

    public void AddStatus(StatusInfo addStatus)
    {
        statusInfo.AddStatusInfo(addStatus);

        updateStatusEvent?.Invoke(statusInfo);
    }

    public void SubStatus(StatusInfo subStatus)
    {
        statusInfo.SubStatusInfo(subStatus);

        updateStatusEvent?.Invoke(statusInfo);
    }

    public void UpdateStatus(StatusInfo addStatus, StatusInfo subStatus)
    {

        statusInfo.AddStatusInfo(addStatus);
        statusInfo.SubStatusInfo(subStatus);

        updateStatusEvent?.Invoke(statusInfo);
    }

    [Button("¸Þ½¬ ·»´õ·¯ Å½»ö")]
    private void AutoSetupMeshRenderers()
    {
        var meshRednerers = this.transform.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRednerers)
        {
            if (meshRenderer.gameObject.CompareTag("Ground"))
            {
                groundMeshRendererList.Add(meshRenderer);
            }

            if (meshRenderer.gameObject.CompareTag("Surface"))
            {
                surfaceMeshRendererList.Add(meshRenderer);
            }
        }
    }

    public void ChangeSurfaceColor(BiomeData biomeData)
    {
        if (updateSurfaceColor != null)
            StopCoroutine(updateSurfaceColor);

        updateSurfaceColor = StartCoroutine(UpdateSurfaceColor(biomeData.SurfaceColor));
    }

    private IEnumerator UpdateSurfaceColor(Color color)
    {
        var startColor = surfaceMeshRendererList[0].sharedMaterial.GetColor("_BaseColor");
        var lerpTime = 0f;
        while (lerpTime < colorSurfaceLerpTime)
        {
            lerpTime += Time.deltaTime;
            var lerpColor = Color.Lerp(startColor, color, lerpTime / colorSurfaceLerpTime);
            mpbSurface.SetColor("_BaseColor", lerpColor);

            foreach (var renderer in surfaceMeshRendererList)
            {
                renderer.SetPropertyBlock(mpbSurface);
            }
            yield return null;
        }
    }

    public void ChangeGroundColor(BiomeData biomeData)
    {
        if (updateGroundColor != null)
            StopCoroutine(updateGroundColor);

        updateGroundColor = StartCoroutine(UpdateGroundColor(biomeData.GroundColor));
    }

    private IEnumerator UpdateGroundColor(Color color)
    {
        var startColor = groundMeshRendererList[0].sharedMaterial.GetColor("_BaseColor");
        var lerpTime = 0f;
        while (lerpTime < colorGroundLerpTime)
        {
            lerpTime += Time.deltaTime;
            var lerpColor = Color.Lerp(startColor, color, lerpTime / colorGroundLerpTime);
            mpbGround.SetColor("_BaseColor", lerpColor);

            foreach (var renderer in groundMeshRendererList)
            {
                renderer.SetPropertyBlock(mpbGround);
            }
            yield return null;
        }
    }

}
