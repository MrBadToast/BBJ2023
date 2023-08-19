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
    private List<MeshRenderer> groundMeshRendererList;

    [SerializeField]
    private StatusInfo statusInfo;

    [SerializeField]
    private float changeBiomeTime = 5f;
    private float currentChangeBiomeTime;

    public UnityEvent<StatusInfo> updateStatusEvent;
    public UnityEvent<BiomeData> changeBiomeEvent;


    [SerializeField]
    private float colorGroundLerpTime = 1f;
    private MaterialPropertyBlock mpbGround;

    [SerializeField]
    private float waterLevelLerpTime = 1f;

    private Coroutine updateGroundColor;
    private Coroutine updateWaterLevel;

    private void Awake()
    {
        mpbGround = new MaterialPropertyBlock();
    }

    private void Start()
    {
        currentChangeBiomeTime = changeBiomeTime;

        changeBiomeEvent.AddListener(ChangeGroundColor);
        ChangeGroundColor(biomeContainer.GetBiome(currentBiomeKey));
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

    [Button("강제 능력치 업데이트 이벤트 호출")]
    private void ForcedInvokeUpdateStateEvent()
    {
        updateStatusEvent?.Invoke(statusInfo);
    }

    [Button("메쉬 렌더러 탐색")]
    private void AutoSetupMeshRenderers()
    {
        var meshRednerers = this.transform.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRednerers)
        {
            if (meshRenderer.gameObject.CompareTag("Ground"))
            {
                groundMeshRendererList.Add(meshRenderer);
            }
        }
    }

    public void ChangeGroundColor(BiomeData biomeData)
    {
        if (updateGroundColor != null)
            StopCoroutine(updateGroundColor);

        updateGroundColor = StartCoroutine(UpdateGroundColor(biomeData));
    }

    private IEnumerator UpdateGroundColor(BiomeData biomeData)
    {
        var startWet = groundMeshRendererList[0].sharedMaterial.GetFloat("_GrassWetness");
        var startSnow = groundMeshRendererList[0].sharedMaterial.GetFloat("_SnowAmount");
        var startSand = groundMeshRendererList[0].sharedMaterial.GetFloat("_SandAmount");

        var lerpTime = 0f;
        while (lerpTime < colorGroundLerpTime)
        {
            lerpTime += Time.deltaTime;
            var lerpWet = Mathf.Lerp(startWet, biomeData.WetAmount, lerpTime / colorGroundLerpTime);
            var lerpSnow = Mathf.Lerp(startSnow, biomeData.SnowAmount, lerpTime / colorGroundLerpTime);
            var lerpSand = Mathf.Lerp(startSand, biomeData.SandAmount, lerpTime / colorGroundLerpTime);

            mpbGround.SetFloat("_GrassWetness", lerpWet);
            mpbGround.SetFloat("_SnowAmount", lerpSnow);
            mpbGround.SetFloat("_SandAmount", lerpSand);

            foreach (var renderer in groundMeshRendererList)
            {
                renderer.SetPropertyBlock(mpbGround);
            }
            yield return null;
        }
    }

    public void ChangeWaterLevel(float height)
    {
        if (updateWaterLevel != null)
            StopCoroutine(updateWaterLevel);

        updateWaterLevel = StartCoroutine(UpdateWaterLevel(height));
    }

    private IEnumerator UpdateWaterLevel(float height)
    {
        var startWet = groundMeshRendererList[0].sharedMaterial.GetFloat("_Waterlevel");

        var lerpTime = 0f;
        while (lerpTime < waterLevelLerpTime)
        {
            lerpTime += Time.deltaTime;
            var lerpWaterLevel = Mathf.Lerp(startWet, height, lerpTime / waterLevelLerpTime);

            mpbGround.SetFloat("_Waterlevel", lerpWaterLevel);

            foreach (var renderer in groundMeshRendererList)
            {
                renderer.SetPropertyBlock(mpbGround);
            }
            yield return null;
        }
    }

}
