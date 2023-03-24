using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
	

[RequireComponent(typeof(Collider))]
public class ColliderHandler : MonoBehaviour
{
    
	[SerializeField] private Collider collider;
	[SerializeField] private UnityEvent onTriggerEnter;
	[SerializeField] private UnityEvent onTriggerStay;
	[SerializeField] private UnityEvent onTriggerExit;
	
	public bool canEnter;
	public bool canStay;
	public bool canExit;
	
    
    
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		Debug.Log("Enter");
		if(canEnter){onTriggerEnter.Invoke();}
	}
	
	// OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
	protected void OnTriggerStay(Collider other)
	{
		if(canStay){onTriggerStay.Invoke();}
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit(Collider other)
	{
		if(canExit){onTriggerExit.Invoke();}
	}
}
