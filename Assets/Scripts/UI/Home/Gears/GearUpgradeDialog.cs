﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearUpgradeDialog : BaseDialog
{

    [Header("UI Configs")] 
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI currentStats;
    [SerializeField] private TextMeshProUGUI NextStats;
    [SerializeField] private TextMeshProUGUI stats;
    [SerializeField] private Image imgStats;

    [Header("Buttons")] 
    [SerializeField] private Button onUpgrade;
    [SerializeField] private Button onQuit;
    public static Action<GearType> OnGearClick;

    private GearData _currentGear;
    private void Start()
    {
        OnGearClick += OnGearClicked;
        
        onUpgrade.onClick.AddListener(OnUpgrade);
        
        onQuit.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    void OnGearClicked(GearType typeOfGear)
    {
        var assets = GearItemAssets.Instance.GetAsset(typeOfGear);
        var data = PlayerDataManager.Instance.data.GearDatas.GetDataByType(typeOfGear);
        name.text = assets.name;
        int currentStat = assets.valueAfterLevelup * data.level;
        currentStats.text = currentStat.ToString();
        NextStats.text = (currentStat + assets.valueAfterLevelup).ToString();
        stats.text = assets.stats.statsEnum.ToString();
        imgStats.sprite = assets.stats.statsImg;

        _currentGear = data;
    }

    void OnUpgrade()
    {
        _currentGear.level++;
        OnGearClick(_currentGear.type);
        GearUIElement.OnUpdateLevel?.Invoke();
    }
}
