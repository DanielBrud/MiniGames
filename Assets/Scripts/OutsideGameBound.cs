

using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class OutsideGameBound : MonoBehaviour
{



    Vector3 startPos;
    Vector3 currentPos;
    float distanceFromStart;
    public float distanceTreshhold;
    [SerializeField]Rigidbody rigidbody;

   

    

    private void OnEnable()
    {
        startPos = transform.position;
        
    }

    public void SetInitialPosition()
    {
        startPos = transform.position;
        RestetPhysicsValues();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;

        distanceFromStart = Vector3.Distance(startPos, currentPos);
        
        if(distanceFromStart > distanceTreshhold)
        {
            RestetPhysicsValues();
        }

        
    }

    public void RestetPhysicsValues()
    {
        gameObject.transform.position = startPos;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }


}
