using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SpotMaterialBahavior : MonoBehaviour
{
    
	
	[SerializeField] private Material[] materials;
	
	
	public float EdgeSmoothNormalValue;
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    //EnergyWaveMaterial();
	    EdgeNormalsMaterial(materials[2]);
    }
    
	public void EdgeNormalsMaterial(Material material)
	{
		
			material.SetFloat("_EdgeSmoothWidth",EdgeSmoothNormalValue);
		
	}
    
    
}
