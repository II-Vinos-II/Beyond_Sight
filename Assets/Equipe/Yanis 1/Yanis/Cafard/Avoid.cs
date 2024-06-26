using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class avoid : MonoBehaviour
{
    public VisualEffect visualEffect;
    public Transform sphereTransform;

    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }


    void Update()
    {
        visualEffect.SetVector3("avoidPos", sphereTransform.localPosition);
    }
}