using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MaterialBehavior : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] InterpolationScalar interpolation;
    [SerializeField] DecalProjector decalProjector;
    // Start is called before the first frame update
    void Start()
    {
        decalProjector.startAngleFade = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (interpolation.startInter)
        {
            float curveValue = interpolation.interpolationValue;

            material.SetFloat("_Transition", curveValue);
        }
        if (interpolation.reversInter)
        {
            float curveValue = interpolation.interpolationValue;

            material.SetFloat("_Transition", curveValue);
        }
    }



}
