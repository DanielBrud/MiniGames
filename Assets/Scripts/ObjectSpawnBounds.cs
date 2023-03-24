using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnBounds : MonoBehaviour
{
    [SerializeField] Transform anotherportal;

    public Vector3 spawBounds;
    public float distanceFromGround;
    public float closestDistanceTreshold;
    

    public void RandomSpawnPostion()
    {
         

        transform.position = new Vector3(Random.Range(-spawBounds.x, spawBounds.x), distanceFromGround, Random.Range(-spawBounds.z, spawBounds.z));

        float distance = Vector2.Distance(new Vector2( transform.position.x, transform.position.z), new Vector2(anotherportal.position.x, anotherportal.position.z));
        
        if(distance < closestDistanceTreshold)
        {
            Debug.Log("True");
            Vector3 direction = Vector3.Normalize(transform.position - anotherportal.position);


            transform.position += Vector3.ProjectOnPlane(direction, Vector3.up) * (closestDistanceTreshold - distance);

            if((transform.position.x > spawBounds.x || transform.position.x < -spawBounds.x) || (transform.position.z > spawBounds.z || transform.position.z < -spawBounds.z))
            {
                transform.position = new Vector3(0.5f, transform.position.y, 0.5f);
            }
        }


    }
}
