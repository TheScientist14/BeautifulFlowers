using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LinkFlowerInfo : MonoBehaviour
{
    #region TMP

    // Attributes

    public TextMeshProUGUI FlowerSizeTMP;
    public TextMeshProUGUI StemWidthTMP;
    public TextMeshProUGUI CapitulumWidthTMP;
    public TextMeshProUGUI LeavesNumberTMP;
    public TextMeshProUGUI PetalsNumberTMP;


    // Traits

    public TextMeshProUGUI WindStrentghTMP;
    public TextMeshProUGUI BlossomStateTMP;
    public TextMeshProUGUI HydratationStateTMP;


    // Summary

    public TextMeshProUGUI Summary;

    #endregion

    #region Values

    // Attributes values

    private float _flowerSize;
    private float _stemWidth;
    private float _capitulumWidth;
    private int _leavesNumber;
    private int _petalsNumber;


    // Traits values

    private float _windStrength;
    private float _blossomState;
    private float _hydrationState;

    public Slider WindSlider;
    public Slider BlossomSlider;
    public Slider HydrationSlider;


    // Summary
    private string _summary;

    #endregion

    #region Flower

    // Flowers links

    public FlowerGenerator FGCam1;
    public FlowerGenerator FGCam2;
    public FlowerGenerator FGCam3;
    public FlowerGenerator FGCam4;
    public FlowerGenerator FGCam5;
    public FlowerGenerator FGCam6;
    
    private FlowerGenerator _activeFlower;
    private FlowerInstance _flowerInstance;

    #endregion

    void Start()
    {
        //SetAttributes(FGCam1);
    }

    public void SetAttributes(FlowerGenerator cam)
    {
        _activeFlower = cam;
        _flowerInstance = cam.GetFlowerInstance();

        // Attributes
        _flowerSize = _flowerInstance.TotalStemHeight;
        _stemWidth = _flowerInstance.StemRadius * 2;
        _capitulumWidth = _flowerInstance.CapitulumRadius * 2;
        _leavesNumber = _flowerInstance.Leaves.Count;
        _petalsNumber = _flowerInstance.Petals.Count;

        FlowerSizeTMP.text = "Flower Size : " + _flowerSize.ToString("F2");
        StemWidthTMP.text = "Stem Width : " + _stemWidth.ToString("F2");
        CapitulumWidthTMP.text = "Capitulum width : " + _capitulumWidth.ToString("F2");
        LeavesNumberTMP.text = "Leaves number : " + _leavesNumber;
        PetalsNumberTMP.text = "Petals number : " + _petalsNumber;

        // Traits
        _windStrength = _activeFlower.m_WindStrength;
        _blossomState = _activeFlower.GetBlossomingState();
        _hydrationState = _activeFlower.GetHydrationState();

        WindSlider.value = _windStrength;
        BlossomSlider.value = _blossomState;
        HydrationSlider.value = _hydrationState;

        // Summary
        _summary = _activeFlower.GetSummary();

        Summary.text = _summary;
    }

    public void ChangeActiveFlower(TextMeshProUGUI nameButton)
    {
        string button = nameButton.text;

        switch (button)
        {
            case "Cam1":
                SetAttributes(FGCam1);
                break;
            case "Cam2":
                SetAttributes(FGCam2);
                break;
            case "Cam3":
                SetAttributes(FGCam3);
                break;
            case "Cam4":
                SetAttributes(FGCam4);
                break;
            case "Cam5":
                SetAttributes(FGCam5);
                break;
            case "Cam6":
                SetAttributes(FGCam6);
                break;
            case null:
                break;
        }
    }

    public void RefreshFlower()
    {
        _activeFlower.GenerateSubject();
        SetAttributes(_activeFlower);
    }

    public void SliderWindChange(float value)
    {
        _activeFlower.m_WindStrength = value;
        value *= 100;
        WindStrentghTMP.text = "Wind strentgh : " + value.ToString("F0") + "%";
    }

    public void SliderBlossomChange(float value)
    {
        _activeFlower.SetBlossomingState(value);
        value *= 100;
        BlossomStateTMP.text = "Blossom state : " + value.ToString("F0") + "%";
    }

    public void SliderHydrationChange(float value)
    {
        _activeFlower.SetHydrationState(value);
        value *= 100;
        HydratationStateTMP.text = "Hydratation state : " + value.ToString("F0") + "%";
    }
}
