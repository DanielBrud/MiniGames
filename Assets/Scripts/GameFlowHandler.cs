﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameFlowHandler : MonoBehaviour
{

	[SerializeField] private GameObject palmHand;
	[SerializeField] private Camera myCamera;
	[SerializeField] private GameObject loader;
	[SerializeField] private InterpolationScalar interpolationScalar;
	[SerializeField] private UnityEvent switchOfMenuButtons;
	[SerializeField] private float loaderOffsetFromHand;
	[SerializeField] private float loadTiemToMaterialMinValue;
	[SerializeField] private float loadTiemToMaterialMaxValue;
	
	
	private MeshRenderer loaderMeshRender;
	private Material loadermaterial;

    [Range(0,1)]
    public float dotProducTreshold;

	private float dotProductValue;
	private bool isMenuVisible = false;

	// Set menu invisible and set material to loader
	
    private void OnEnable()
    {
        isMenuVisible = false;
        loaderMeshRender = loader.GetComponent<MeshRenderer>();
        loadermaterial = loaderMeshRender.material;
    }

    // Update is called once per frame
    void Update()
    {

	    IsLoaderActive();

    }
    
	// Check is camera look at open hand and start loading
	private void IsLoaderActive()
	{
		dotProductValue = Vector3.Dot(-palmHand.transform.up, myCamera.transform.forward);
        
		if(dotProductValue > dotProducTreshold)
		{
            
            
			loader.SetActive(true);
            
			loader.transform.position = palmHand.transform.position + palmHand.transform.up * loaderOffsetFromHand;
			loader.transform.rotation = Quaternion.LookRotation(-palmHand.transform.up, -palmHand.transform.right);
			if (!isMenuVisible)
			{
				loaderMeshRender.enabled = true;
				interpolationScalar.StartInterpolaation();
                
			}
			float loadTime = Mathf.Lerp(loadTiemToMaterialMaxValue, loadTiemToMaterialMinValue, interpolationScalar.interpolationValue);
			loadermaterial.SetFloat("_loadTime", loadTime);
			isMenuVisible = true;
		}
		else if (isMenuVisible)
		{
			loader.transform.position = new Vector3(0, -5, 0);
			switchOfMenuButtons.Invoke();
			isMenuVisible = false;
			loaderMeshRender.enabled = false;
			interpolationScalar.startValue = 0;
			interpolationScalar.interpolationValue = 0;
			
            
            
		}
	}

    public void RestScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        

#if UNITY_EDITOR

        EditorApplication.ExitPlaymode();
#endif
        
        Application.Quit();

    }
}