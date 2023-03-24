using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TelePortlasCollisionHandler : MonoBehaviour
{

    [SerializeField] private GameObject anotherPortal;
    [SerializeField] private GameObject teleBall;
    [SerializeField] private TelePortlasCollisionHandler anotherPortalScript;
	[SerializeField] private UnityEvent collissionStart;
	[SerializeField] private UnityEvent afterTeleport;
	[SerializeField] private Material material;
	[SerializeField] private InterpolationScalar interpolationScalar;
	[SerializeField] private float afterTeleportPostionOffset;
	

    public bool isColiding = true;
    public float forceMultiplier;
    public float delayTime;
	private float speed;
	private float collisionSpeed;
	private Vector3 angularVelocityColider;
	private Vector3 forwardVectoreColider;
	private Vector3 previousPostion;
	private Vector3 creviousPostion;

    Rigidbody coliderRiggenbody;
    
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		previousPostion = creviousPostion;
	}
    
	// Delay time to execute action
	IEnumerator DelayToForceAplly(float delayTime)
    {
      yield return new WaitForSeconds(delayTime);
        DelayTeleportForce();
    }
	
	
	// Set values on first portal hit
    private void OnCollisionEnter(Collision collision)
    {
        
        if (isColiding)
        {
            coliderRiggenbody = collision.rigidbody;
            anotherPortalScript.isColiding = false;
	        collisionSpeed = speed;
            angularVelocityColider = collision.rigidbody.angularVelocity;
            coliderRiggenbody.isKinematic = true;
            collissionStart.Invoke();
            TelePortals_Ball.hasPassPortal = true;

            StartCoroutine(DelayToForceAplly(delayTime));
            
            isColiding = false ;
        }
        
    }
	
	// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	protected void FixedUpdate()
	{
		creviousPostion  = teleBall.transform.position;
		speed = Vector3.Distance(creviousPostion,previousPostion );
		previousPostion = creviousPostion;
	}
	
	// Action after teleport to second portal
    void DelayTeleportForce()
    {

        coliderRiggenbody.isKinematic = false;
	    coliderRiggenbody.position = anotherPortal.transform.position + -anotherPortal.transform.forward * afterTeleportPostionOffset;
        coliderRiggenbody.velocity = -anotherPortal.transform.forward * (collisionSpeed  * forceMultiplier);
	    afterTeleport.Invoke();
	    
    }
    
    
    private void OnCollisionExit(Collision collision)
    {
        if (!isColiding)
        {             
            this.isColiding = true;
        }
        
    }

   

    
}
