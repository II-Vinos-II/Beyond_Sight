using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TestForScanManager : MonoBehaviour
{
    [Header("Small Scan")]
    public float smallSize = 5f;
    public float smallSpeed = 0.5f;

    [Header("Medium Scan")]
    public float mediumSize = 3f;
    public float mediumSpeed = 0.5f;

    [Header("Big Scan")]
    public float bigSize = 5f;
    public float bigSpeed = 0.5f;

    public void RunScan()
    {
        PointLightScanner.GetInstance().StartScanner(PointLightScanner.GetInstance().transform.position, mediumSize, mediumSpeed);
    }

    public void SmallScan()
    {
        PointLightScanner.GetInstance().StartScanner(transform.position, smallSize, smallSpeed);
    }

    public void MediumScan()
    {
        PointLightScanner.GetInstance().StartScanner(transform.position, mediumSize, mediumSpeed);
    }

    public void BigScan()
    {
        PointLightScanner.GetInstance().StartScanner(transform.position, bigSize, bigSpeed);
    }

    public void LadderScan()
    {
        PointLightScanner.GetInstance().StartScanner(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), smallSize, smallSpeed);
        
        PlayerWalkSounds.GetInstance().PlayRandom(0);
    }

    public void ClimbScan()
    {
        PointLightScanner.GetInstance().StartScanner(new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), smallSize, smallSpeed);
    }

    public void WithleScan()
    {
        PointLightScanner.GetInstance().StartScanner(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), 100f, 1f);
    }
}
