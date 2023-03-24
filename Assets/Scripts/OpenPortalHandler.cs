using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPortalHandler : MonoBehaviour
{

    [SerializeField] private Material material;
    [SerializeField] private InterpolationScalar interpolationScalar;


    private void OnEnable()
    {
        interpolationScalar.StartInterpolaation();
    }

    // Update is called once per frame
    void Update()
    {
        OpenandClosePortal();
    }

    void OpenandClosePortal()
    {
        float interpolatiovalue = interpolationScalar.interpolationValue;

        material.SetFloat("_PortalOpen", interpolatiovalue);
    }
    private void OnDisable()
    {
        

        interpolationScalar.interpolationValue = 0;
    }

   

    
}
