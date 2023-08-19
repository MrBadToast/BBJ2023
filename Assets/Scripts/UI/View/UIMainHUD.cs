using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainHUD : UIBaseView
{
    private WorldController worldController;
    UIPausePopup pausePopup;

    [SerializeField]
    private UIBaseGauge surfaceGauge;
    [SerializeField]
    private UIBaseGauge humidityGauge;
    [SerializeField]
    private UIBaseGauge temperatureGauge;

    private void Awake()
    {
        worldController = FindObjectOfType<WorldController>();
        worldController.updateStatusEvent.AddListener(UpdateStatus);
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Init(UIData uiData)
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausePopup == null)
        {
            pausePopup = (UIPausePopup)UIController.Instance.OpenPopup("Pause");

            pausePopup.endCloseEvent.AddListener(() =>
            {
                pausePopup = null;
            });
        }
    }

    public void UpdateStatus(StatusInfo statusInfo)
    {
        surfaceGauge.UpdateGauge((statusInfo.GetElement(StatusType.Surface).CalculateTotalAmount() + 1) * 0.5f);
        humidityGauge.UpdateGauge(statusInfo.GetElement(StatusType.Humidity).CalculateTotalAmount());
        temperatureGauge.UpdateGauge((statusInfo.GetElement(StatusType.Temperature).CalculateTotalAmount() + 1) * 0.5f);
    }

}
