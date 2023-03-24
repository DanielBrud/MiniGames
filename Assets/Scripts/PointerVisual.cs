using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerVisual : MonoBehaviour
{

    [SerializeField] private pointerState[] state;

    private Material material;

    
    // Material state properties
    [System.Serializable]
    public struct pointerState 
    {
        public Color innerColor;
        public Color outlineColor;
        public float pointerAlpha;
        public float radialGradientIntensity;
        public float radialGradienScale;
        public float radialGradientBackgroundOpacity;
        public float radialGradientOpacity;
    }

    // Set instance material
    private void OnEnable()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Set material properties 
    void SetOculusMaterialPointerProperties(Material pointerMaterial, Color innerColor, Color outlineColor, float alpha = 1f, float radialGradientIntensity = 1f, float radialGradienScale = 0.263f, float radialGradientBackgroundOpacity = 0.3f, float radialGradientOpacity = 1f)
    {

        pointerMaterial.SetColor("_Color", innerColor);
        pointerMaterial.SetColor("_OutlineColor", outlineColor);
        pointerMaterial.SetFloat("_Alpha", alpha);
        pointerMaterial.SetFloat("_RadialGradientIntensity;", radialGradientIntensity);
        pointerMaterial.SetFloat("_RadialGradientScale", radialGradienScale);
        pointerMaterial.SetFloat("_RadialGradientBackgroundOpacity", radialGradientBackgroundOpacity);
        pointerMaterial.SetFloat("_RadialGradientOpacity", radialGradientOpacity);
    }

    // Invoke changes on material depndes of state
    public void SelectedOculusMaterialPointerProperties()
    {
        SetOculusMaterialPointerProperties(material, state[0].innerColor, state[0].outlineColor, state[0].pointerAlpha, state[0].radialGradientIntensity, state[0].radialGradienScale, state[0].radialGradientBackgroundOpacity, state[0].radialGradientOpacity);
    }
    public void UnSelectedOculusMaterialPointerProperties()
    {
        SetOculusMaterialPointerProperties(material, state[1].innerColor, state[1].outlineColor, state[1].pointerAlpha, state[1].radialGradientIntensity, state[1].radialGradienScale, state[1].radialGradientBackgroundOpacity, state[1].radialGradientOpacity);
    }

}
