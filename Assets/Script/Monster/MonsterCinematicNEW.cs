using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCinematicNEW : MonoBehaviour
{
    public AudioSource aS;
    public AudioClip bigBang;
    public AudioClip whistle;
    public AudioClip monsterScream;
    public AudioClip bodyfall;

    public GameObject scanPlacement;

    public void BigBang()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 15, 1f);

        aS.PlayOneShot(bigBang);
        aS.PlayOneShot(bigBang);
    }

    public void Whistle()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 100f, 1f);

        aS.PlayOneShot(whistle);
    }

    public void StepScan()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 5, 0.1f);

        PlayerWalkSounds.GetInstance().PlayRandom(2);
        aS.PlayOneShot(bodyfall);
    }

    public void MediumScan()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 10, 0.25f);

        PlayerWalkSounds.GetInstance().PlayRandom(3);
    }

    public void ScanIntroMonstre()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 100, 1, 2);

        aS.PlayOneShot(monsterScream);
    }
}
