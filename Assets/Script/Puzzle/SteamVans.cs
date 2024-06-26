using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVans : MonoBehaviour
{
    public bool isSteaming;
    public GameObject steam;

    private void Start()
    {
        ChangeState();
    }

    public void ChangeState()
    {
        if (isSteaming)
        {
            steam.SetActive(true);
        }
        else
        {
            steam.SetActive(false);
        }
    }
}
