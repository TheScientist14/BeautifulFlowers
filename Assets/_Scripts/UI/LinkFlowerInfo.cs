using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkFlowerInfo : MonoBehaviour
{

    // TMP
    public TextMeshProUGUI FlowerSizeTMP;
    public TextMeshProUGUI StemWidthTMP;
    public TextMeshProUGUI CapitulumWidthTMP;
    public TextMeshProUGUI LeavesNumberTMP;
    public TextMeshProUGUI PetalsNumberTMP;

    // Atributes values
    private float flowerSize;
    private float stemWidth;
    private float capitulumWidth;
    private int leavesNumber;
    private int petalsNumber;

    // Flower generator links
    public FlowerGenerator FGCam1;
    public FlowerGenerator FGCam2;
    public FlowerGenerator FGCam3;
    public FlowerGenerator FGCam4;
    public FlowerGenerator FGCam5;
    public FlowerGenerator FGCam6;
    public FlowerGenerator ActiveFlower;

    private FlowerInstance m_FlowerInstance;


    void Start()
    {
    }

    public void SetAttributes(FlowerGenerator cam)
    {
        ActiveFlower = cam;
        m_FlowerInstance = cam.GetFlowerInstance();

        flowerSize = m_FlowerInstance.TotalStemHeight;
        stemWidth = m_FlowerInstance.StemRadius * 2;
        capitulumWidth = m_FlowerInstance.CapitulumRadius * 2;
        leavesNumber = m_FlowerInstance.Leaves.Count;
        petalsNumber = m_FlowerInstance.Petals.Count;

        FlowerSizeTMP.text = "Flower Size : " + flowerSize;
        StemWidthTMP.text = "Stem Width : " + stemWidth;
        CapitulumWidthTMP.text = "Capitulum width : " + capitulumWidth;
        LeavesNumberTMP.text = "Leaves number : " + leavesNumber;
        PetalsNumberTMP.text = "Petals number : " + petalsNumber;
    }

    public void ChangeActiveFlower(TextMeshProUGUI nameButton)
    {
        switch (nameButton.ToString())
        {
            case "Cam1":
                SetAttributes(FGCam1);
                break;
            case "Cam2":
                SetAttributes(FGCam1);
                break;
            case "Cam3":
                SetAttributes(FGCam1);
                break;
            case "Cam4":
                SetAttributes(FGCam1);
                break;
            case "Cam5":
                SetAttributes(FGCam1);
                break;
            case "Cam6":
                SetAttributes(FGCam1);
                break;
        }
    }

    public void RefreshFlower()
    {
        ActiveFlower.GenerateSubject();
    }
}
