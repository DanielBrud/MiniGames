using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;


public class EventCollision1Surface : MonoBehaviour
{

    [SerializeField] Material material;
    [SerializeField] int[] layersInt;
    [SerializeField] LayerMask layerMask;
    
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] GameObject spawmOBJ;
    [SerializeField] GameObject firsthitobj;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] private UnityEvent Collision;
    int mask;
    bool maskcompere;
    bool firstHit = true;
    Vector3 firstColPos;
    public float distanceInNormalHitDirection;
    
    public enum faceDirection
    {
        Up,
        Forward,
        Right
    };
    public faceDirection faceDirectionAxis;

    [Range(-1,1)]
    public float faceDirectionmultiplier;
    
    



    

    private void OnCollisionEnter(Collision collision)
    {
        
        int coliderLayer = collision.gameObject.layer;
        
        

        if (firstHit)
        {
            firstColPos = collision.GetContact(0).point;
            
            firstHit = false;
        }
        
        
        for (int i = 0; i < layersInt.Length; i++)
        {
            maskcompere = coliderLayer == layersInt[i];
            if (maskcompere) { break; }
            
        }

        if (maskcompere)
        {
	        spawmOBJ.SetActive(true);
            


            
            Vector3 posCol = collision.GetContact(0).point;
            Vector3 normalCol = collision.GetContact(0).normal;
            
            spawmOBJ.transform.position = posCol + normalCol * distanceInNormalHitDirection;
            



            
            ObjectOrientaionProperites(normalCol, faceDirectionmultiplier);
            

            Collision.Invoke();
            firstHit = true;
            //transform.position = posCol;
            
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
    
    void ObjectOrientaionProperites(Vector3 normal, float faceMultipiler)
    {
        switch (faceDirectionAxis)
        {
            case faceDirection.Up:
                spawmOBJ.transform.up = normal * faceMultipiler;
                break;
            case faceDirection.Forward:
                spawmOBJ.transform.forward = normal * faceMultipiler;
                break;
            case faceDirection.Right:
                spawmOBJ.transform.right = normal * faceMultipiler;
                break;
        }
    }
}
