using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteripolationCurve : MonoBehaviour
{
    [SerializeField] AnimationCurve[] animationCurve;
    [SerializeField][Range(0,1)]float[] valueT;
    [SerializeField] [Range(0.01f, 10)] float[] scalarT;
    bool isFocus;
    [SerializeField] bool[] belowZero;
   
    // Start is called before the first frame update
    private void OnEnable()
    {
        for( int i = 0; i < valueT.Length; i++)
        {
            valueT[i] = 1;
            belowZero[i] = false;
        }

        

    }
    // Update is called once per frame
    void Update()
    {
        SampleCurve();
    }
    public void FocusOn()
    {
        gameObject.SetActive(true);
        isFocus = true;
        print(isFocus.ToString());
    }
    public void FocusOff()
    {
        isFocus = false;
    }
    public float SampleCurve()
    {
        if (valueT.Length > 0)
        {
            for (int i = 0; i < valueT.Length; i++)
            {
                valueT[i] = isFocus ? valueT[i] + Time.deltaTime * scalarT[i] : valueT[i] - Time.deltaTime * scalarT[i];

                if (valueT[i] < 0f)
                {
                    belowZero[i] = true;
                }
                return valueT[i];
            }
            if(ArrayList.ReferenceEquals(belowZero, true))
            {
                gameObject.SetActive(false);
            }
            
            //for (int i = 0; i < valueT.Length; i++)
            //{ 
            //    if (belowZero[i])
            //    {
                    
            //    }

                
            //}
            
        }
        
        return 0;
        
    }
}
