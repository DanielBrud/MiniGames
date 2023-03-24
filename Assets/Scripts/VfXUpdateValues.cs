using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfXUpdateValues : MonoBehaviour
{
    RichPoint richPoint;
    [SerializeField] VisualEffect vfx;
    float currentValue;
    float previousValue = 0;
    bool onUpdate = false;
    public float filSpeed;

    private void OnEnable()
    {
        richPoint = GetComponentInParent<RichPoint>();

        richPoint.Connection  += OnConection;
    }
    private void OnDisable()
    {
        richPoint.Connection -= OnConection;
        currentValue = 0;
        previousValue = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (onUpdate && previousValue <= currentValue)
        {
            previousValue += Time.deltaTime * filSpeed;
            vfx.SetFloat("ObjectForceDistance", previousValue);
        }
    }
    void OnConection()
    {
        currentValue = richPoint.all;
        onUpdate = true;
    }
    
}