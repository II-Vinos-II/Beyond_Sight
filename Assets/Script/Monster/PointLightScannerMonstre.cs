using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightScannerMonstre : MonoBehaviour
{
    public Transform pointLightMonstrePos;


    public void ScanIntroMonstre()
    {
        PointLightScanner.GetInstance().StartScanner(pointLightMonstrePos.transform.position, 100, 1, 2);
    }
}
