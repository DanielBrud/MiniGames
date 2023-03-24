using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;


public class EventCollisionTelePortals : MonoBehaviour
{

    [SerializeField] Material material;
    [SerializeField] LayerMask  mask;
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] GameObject InPortal;
    //[SerializeField] GameObject OutPortal;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] private UnityEvent Collision;
    int m;
    bool firstHit = true;
    Vector3 firstColPos;
    public float distanceInNormalHitDirection;



    // Start is called before the first frame update
    void Start()
    {
        
        m = 6;
        //m = LayerMask.NameToLayer("Collide");
        Debug.Log(LayerMask.NameToLayer("Collide").ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.GetContact(0).point);
        
        //mask = LayerMask.GetMask("Collide");

        //Debug.Log(m.ToString());
        if (firstHit)
        {
            firstColPos = collision.GetContact(0).point;
            
            firstHit = false;
        }


        if (collision.gameObject.layer == m )
        {
            InPortal.SetActive(true);


            
            Vector3 posCol = collision.GetContact(0).point;
            Vector3 normalCol = collision.GetContact(0).normal;
            

            
            InPortal.transform.position = posCol + normalCol * distanceInNormalHitDirection;
            




            
            InPortal.transform.forward = -normalCol;
            

            Collision.Invoke();
            firstHit = true;
            
            
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        firstHit = true;
        if (!sphereCollider.enabled)
        {
            sphereCollider.enabled = true;
        }
    }
    private void OnDisable()
    {
        
    }

    public void SetPositionBeforeCameraEye()
    {
        Camera myCam = Camera.main;

        transform.position = myCam.transform.position + myCam.transform.forward * 0.5f;
    }
    // Update is called once per frame

}
