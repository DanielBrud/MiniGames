using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class InterpolationScalar : MonoBehaviour
{
	[SerializeField] private UnityEvent bindScaleUP;
	[SerializeField] private UnityEvent bindScaleDown;
	[SerializeField] private UnityEvent ToogleWorld;
    [SerializeField] private AnimationCurve[] animationCurve;

    public float tresholdUP;
    public float tresholdDown;
    

    public float startValue;
    public float reverseValue;
    public float interpolationValue;
    public float scalarInterpolation;
    public bool startInter = false;
    public bool reversInter = false;
    
    
    
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

    public void SetInterpolationValue(float value)
    {
        interpolationValue = value;
    }

    public void CanStartInterpolate(bool start)
    {
        startInter = start;
        
    }
    
	public void CanReverseInterpolate(bool reverse)
    {
        reversInter = reverse;
        
    }
    
	
	public void SetStartValue(float value)
	{
		startValue = value;
	}
	
	public void SetReverseValue(float value)
	{
		reverseValue = value;
	}
    // Update is called once per frame
    void Update()
    {
        if (startInter)
        {
            startValue += Time.deltaTime * scalarInterpolation;
            interpolationValue = animationCurve[0].Evaluate(startValue);
            if (startValue > tresholdUP)
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
            interpolationValue = animationCurve[1].Evaluate(reverseValue);
            if (reverseValue < tresholdDown)
            { 
                bindScaleDown.Invoke();
            }
        }
        if(reverseValue < 0)
        {
            reversInter = false;
            reverseValue = 0;
            
        }

    } 
}
