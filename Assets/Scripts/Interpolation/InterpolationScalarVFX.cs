using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class InterpolationScalarVFX : MonoBehaviour
{
    [SerializeField] UnityEvent bindScaleUP;
    [SerializeField] UnityEvent bindScaleDown;
    [SerializeField] UnityEvent ToogleWorld;
    [SerializeField] float tresholdUP;
    [SerializeField] float tresholdDown;
    public AnimationCurve animationCurve;
    [SerializeField] VisualEffect vfx;

    public float startValue;
    public float reverseValue;
    public float interpolationValue;
    public float scalarInterpolation;
    public bool startInter = false;
    public bool reversInter = false;


    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartInterpolaation()
    {
        startInter = true;
        startValue = 0;
    }
    public void ReverseInterpolaation()
    {
        reversInter = true;
        reverseValue = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (startInter)
        {
            startValue += Time.deltaTime * scalarInterpolation;
            interpolationValue = animationCurve.Evaluate(startValue);
            vfx.SetFloat("InterpolationValue", interpolationValue);
            if (interpolationValue > tresholdUP)
            {
                bindScaleUP.Invoke();
            }
        }
        if(startValue > 1)
        {
            startInter = false;
            startValue = 1;
        }
        if (reversInter)
        {
            reverseValue -= Time.deltaTime * scalarInterpolation;
            interpolationValue = animationCurve.Evaluate(reverseValue);
            vfx.SetFloat("InterpolationValue", interpolationValue);
            if (interpolationValue < tresholdDown)
            { 
                bindScaleDown.Invoke();
            }
        }
        if(reverseValue < 0)
        {
            reversInter = false;
            reverseValue = 0;
            //ToogleWorld.Invoke();
        }

    } 
}
