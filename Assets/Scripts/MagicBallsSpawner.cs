using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallsSpawner : MonoBehaviour
{

    
	[SerializeField] private Camera mycamera;
	[SerializeField] private float startExeperienceDelay;
	[SerializeField] private float ballSpawnOffsetFromCamera;
	
	public static int gameID { get; private set; }
    
    private GameObject[] magicBalls;
	private GameObject currentBall;
	private Vector3 cameraPosition;
	private Vector3 cameraDirection;
	private float distanceFromCamera;
	private int ballscount;
	
	
	



    private void OnEnable()
    {
        
        ballscount = transform.childCount;       
        magicBalls = new GameObject[ballscount];
        for ( int i  = 0; i < ballscount; i++)
        {
	        magicBalls[i] = transform.GetChild(i).gameObject;
            
        }
        
    }
	
	// Start is called before the first frame update
	private void Start()
	{
		StarNewExperienceDelay(startExeperienceDelay);
        
        
	}

	// Spawn random ball 
    public void RandomSpawner()
    {
        
        
        cameraPosition = mycamera.transform.position;
        cameraDirection = mycamera.transform.forward;
        
        int rndInt = Random.Range(0, ballscount);
        gameID = rndInt;
        GameObject chosenObject = magicBalls[rndInt];

        // For debugging
        //GameObject chosenObject = magicBalls[1];

        if (!chosenObject.activeSelf) 
        {
            chosenObject.SetActive(true);

            chosenObject.transform.position = cameraPosition + cameraDirection * ballSpawnOffsetFromCamera;

            int childCount = chosenObject.transform.childCount;

            if (childCount > 0)
            {
                GameObject childFirst = chosenObject.transform.GetChild(0).gameObject;

                
                childFirst.SetActive(true);
                childFirst.transform.position = cameraPosition + cameraDirection * ballSpawnOffsetFromCamera;
                childFirst.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                childFirst.GetComponent<Rigidbody>().velocity = Vector3.zero;


                if (childCount > 1)
                {
                    GameObject[] childs = new GameObject[childCount];

                    for( int i = 1; i < childCount; i++)
                    {
	                    childs[i] = chosenObject.transform.GetChild(i).gameObject;
                        childs[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                        childs[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        
                    }
    
                }
                
            }

        }

    }
     
    
    
	IEnumerator cameraData(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
	    RandomSpawner();
	    
    }
	public void StarNewExperienceDelay(float delay)
	{
		StartCoroutine(cameraData(delay));
	}

   


}
