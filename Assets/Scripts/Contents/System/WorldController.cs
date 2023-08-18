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
    private StatusInfo statusInfo;

    [SerializeField]
    private float changeBiomeTime = 5f;
    private float currentChangeBiomeTime;

    public UnityEvent<StatusInfo> updateStatusEvent;
    public UnityEvent<BiomeData> changeBiomeEvent;

    private void Start()
    {
        currentChangeBiomeTime = changeBiomeTime;
    }

    private void Update()
    {
        currentChangeBiomeTime -= Time.deltaTime;

        if (currentChangeBiomeTime < 0f)
        {
            var resultBiome = biomeContainer.GetBiome(statusInfo);

            if (resultBiome == null)
            {
                resultBiome = biomeContainer.GetBiome("Default");
            }

            if (!currentBiomeKey.Equals(resultBiome.Key))
            {
                biomeController.Hide();

                //Biome Update
                var biomeEnviroment = Instantiate(resultBiome.EnviromentPrefab);
                biomeController = biomeEnviroment.GetComponent<BiomeController>();
                biomeController.Show();

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

}
