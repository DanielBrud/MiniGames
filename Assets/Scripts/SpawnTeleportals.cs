using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnTeleportals : MonoBehaviour
{
    [SerializeField] GameObject InPortal;
    [SerializeField] GameObject OutPortal;
    [SerializeField] GameObject TeleBall;
    [SerializeField] GameObject LandSpot;
    [SerializeField] GameObject SpawnSpot;
    [SerializeField] GameObject MyCamera;
	[SerializeField] UnityEvent FinishExperience;
	[SerializeField] UnityEvent NewRound;

    public float spawnBoundsOutPortal;
    public float spawnBoundsInPortal;
    public Vector3 SpotSpawnBoundOffset;
    public float spawnSpotDistanceFromCamera;
    public float minPortalDistanceFromCamera;
    public int roundCounter;
    public bool isSpawnColseToPlayer;
    int currentRound;

    void RandomTeleportalSpawner(GameObject portal, GameObject spot, float disttanceFromCamMin, float disttanceFromCamMax,float minDistance)
    {
        
        Vector3 randomPos = new Vector3(Random.Range(disttanceFromCamMin, disttanceFromCamMax), MyCamera.transform.position.y + Random.Range(-0.3f, -0.1f), Random.Range(disttanceFromCamMin, disttanceFromCamMax));
        

        Vector3 portalPos =  randomPos;
        portal.transform.position = portalPos;

        portal.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(portalPos - MyCamera.transform.position));

        
        spot.transform.position = portalPos + -portal.transform.forward * 0.5f + new Vector3(Random.Range(-SpotSpawnBoundOffset.x, SpotSpawnBoundOffset.x), Random.Range(-SpotSpawnBoundOffset.y, SpotSpawnBoundOffset.y), Random.Range(-SpotSpawnBoundOffset.z, SpotSpawnBoundOffset.z));
	    spot.transform.rotation = Quaternion.LookRotation(Vector3.up);
    }

    public void SpawnObjects()
    {
        RandomTeleportalSpawner(InPortal, SpawnSpot, -spawnBoundsInPortal, spawnBoundsInPortal, minPortalDistanceFromCamera);
        RandomTeleportalSpawner(OutPortal, LandSpot, -spawnBoundsOutPortal, spawnBoundsOutPortal, minPortalDistanceFromCamera * 3f);
        if (isSpawnColseToPlayer)
        {
            SpawnSpot.transform.position = MyCamera.transform.position + MyCamera.transform.forward * spawnSpotDistanceFromCamera;
        }
        
        TeleBall.transform.position = SpawnSpot.transform.position + new Vector3(0,0.3f,0);
        TeleBall.GetComponent<OutsideGameBound>().SetInitialPosition();
	    TelePortals_Ball.hasPassPortal = false;
	    NewRound.Invoke();
    }


    public void RoundCounter()
    {
        currentRound += 1;
        if(currentRound >= roundCounter)
        {
            FinishExperience.Invoke();
        }


    }



    
}
