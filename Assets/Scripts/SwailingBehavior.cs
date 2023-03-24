using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SwailingBehavior : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private GameObject portal;
    [SerializeField] private InterpolationScalar interpolationScalar;

    public Vector2 swalingRangeInterpolation;
	public float portalDeepnes;
	public int swalingRenderOrder;
	public int normalRenderOrder;

    //TelePortlasCollisionHandler telePortlas;

    private Vector3 _portalCurrentPosition;
    private float _delayTime;
    private float swalingValue;
    private bool canSwalling = false;

    private void OnEnable()
    {
	    //telePortlas = portal.GetComponent<TelePortlasCollisionHandler>();
	    material = GetComponent<MeshRenderer>().material;
    }

    

    void SwalingTime()
    {

        interpolationScalar.scalarInterpolation = 1 / _delayTime;

        swalingValue = Mathf.Lerp(swalingRangeInterpolation.x, swalingRangeInterpolation.y, interpolationScalar.interpolationValue);

        material.SetFloat("_rangeOfSwaling", swalingValue);
    }
    
    public void GetPortalCurrentPostion()
    {
        _portalCurrentPosition = portal.transform.position + portal.transform.forward * portalDeepnes;
	    material.SetVector("_portalPosition", _portalCurrentPosition);
	    canSwalling = true;
	    
    }

    public void GetDelayTime(float time)
    {
        _delayTime = time;
    }
	
	
	// Set value after 
    public void AfterSwall()
    {
	    
	    
	    swalingValue = 0;
	     
        interpolationScalar.interpolationValue = 0;
        _portalCurrentPosition = new Vector3(0, -10, 0);
	    
	    material.SetFloat("_rangeOfSwaling", swalingValue);
	    
	    material.SetVector("_portalPosition", _portalCurrentPosition);
	    canSwalling = false;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (canSwalling)
        {
            SwalingTime();
        }
	    
    }
    
    
	public void SetMaterialRenderQueue(int renderQueue)
	{
		material.renderQueue = renderQueue;
	}
}
