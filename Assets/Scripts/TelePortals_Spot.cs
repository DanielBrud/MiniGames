using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class TelePortals_Spot : MonoBehaviour
{
	[SerializeField] UnityEvent targetAchived;
	[SerializeField] Transform HandPositionOne;
	[SerializeField] Transform HandPositionTwo;
	[SerializeField] Material materialVAT;
	[SerializeField] VisualEffect vfx;
	[SerializeField] AudioSource firworks;
	[SerializeField] AudioSource SpotEffect;
    
	private readonly int targetColideVFXID = Shader.PropertyToID("TargetColide");
	private VFXEventAttribute eventAttribute;
	
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		if(vfx != null)
		{
			
			eventAttribute = vfx.CreateVFXEventAttribute();
		}
		
	}
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if(HandPositionOne != null || HandPositionTwo != null )
	    {
	    	float distancehandOne = Vector3.Distance(HandPositionOne.position,transform.position);
		    float distancehandTwo = Vector3.Distance(HandPositionTwo.position,transform.position); 
	    	
	    	Vector3 closerHand = distancehandOne < distancehandTwo ? HandPositionOne.position : HandPositionTwo.position;
	    	
	    	
	    	HandPostionToMaterial(closerHand);
	    }
	    
	    
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (TelePortals_Ball.hasPassPortal)
        {
            collision.rigidbody.velocity = Vector3.zero;
	        collision.rigidbody.angularVelocity = Vector3.zero;
	        if(vfx != null)
	        {
		        vfx.SendEvent(targetColideVFXID,eventAttribute);
		        firworks.PlayDelayed(0.4f);
		        
	        }
	        
             new WaitForSeconds(2);
            // Event for vfx and material
	        //targetAchived.Invoke();
            
            // For Prototype reason
            Invoke("DelayAfterTargetAchived", 2);

        }
    }
    
    
    // For Prototype reason
    void DelayAfterTargetAchived()
    {
        targetAchived.Invoke();
    }
    
	void HandPostionToMaterial(Vector3 handPostion)
	{
		if(handPostion != null && materialVAT != null)materialVAT.SetVector("_HandPostion",handPostion);
	}
}
