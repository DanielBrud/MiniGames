using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;


public class EventCollision : MonoBehaviour
{

    [SerializeField] Material material;
    [SerializeField] LayerMask  mask;
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] GameObject spawmOBJ;
    [SerializeField] GameObject firsthitobj;
    [SerializeField] private UnityEvent Collision;
    int m;
    bool firstHit = true;
    Vector3 firstColPos;



    // Start is called before the first frame update
    void Start()
    {
        //material.color = Color.red;
        m = 6;
        //m = LayerMask.NameToLayer("Collide");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.GetContact(0).point);
        
        //mask = LayerMask.GetMask("Collide");

        //Debug.Log(m.ToString());
        if (firstHit)
        {
            firstColPos = collision.GetContact(0).point;
            //firsthitobj.transform.position = firstColPos;
            //Debug.Log(firstColPos.ToString());
            firstHit = false;
        }


        if (collision.gameObject.layer == m )
        {
            spawmOBJ.SetActive(true);


            //material.color = Color.blue;
            //Debug.Log("touch");
            Vector3 posCol = collision.GetContact(0).point;
            Vector3 normalCol = collision.GetContact(0).normal;
            Vector3 dirCol= Vector3.Normalize(posCol - firstColPos);
            Vector3 projectPlaneDir = Vector3.ProjectOnPlane(dirCol, -normalCol);
            //Debug.Log(dirCol.ToString());
           //Debug.DrawRay(firstColPos, dirCol, Color.red,20f);

            visualEffect.transform.position = posCol;
            //visualEffect.Play();
            spawmOBJ.transform.position = posCol + normalCol * 0.003f ;
            float angle = Vector3.Angle(spawmOBJ.transform.right, projectPlaneDir);
            Quaternion normalRot = Quaternion.LookRotation(projectPlaneDir ,normalCol);
            spawmOBJ.transform.rotation = normalRot * Quaternion.AngleAxis(90, spawmOBJ.transform.right) * Quaternion.AngleAxis(90, spawmOBJ.transform.forward);
            //Quaternion rightRot = Quaternion.LookRotation(projectPlaneDir);
            //spawmOBJ.transform.rotation *= rightRot;

             


            //spawmOBJ.transform.right = projectPlaneDir;
            //spawmOBJ.transform.forward = -normalCol;
            //spawmOBJ.transform.rotation = Quaternion.AngleAxis(angle, normalCol);


            //spawmOBJ.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.right, Vector3.ProjectOnPlane(dirCol, -normalCol),6,10f));

            //spawmOBJ.transform.rotation = Quaternion.LookRotation(-normalCol, Vector3.up) * Quaternion.LookRotation(Vector3.ProjectOnPlane(dirCol, -normalCol), normalCol);
            // spawmOBJ.transform.rotation = Quaternion.FromToRotation(spawmOBJ.transform.up, Vector3.ProjectOnPlane(dirCol, -normalCol));

            //Debug.DrawRay(posCol, Vector3.ProjectOnPlane(dirCol, -normalCol), Color.red, 20f);
            //spawmOBJ.transform.rotation = Quaternion.RotateTowards(spawmOBJ.transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(dirCol , - normalCol), normalCol),90f);
            //spawmOBJ.transform.rotation = Quaternion.AngleAxis(-90f, Vector3.forward);
            //spawmOBJ.transform.position += spawmOBJ.transform.right * (spawmOBJ.transform.lossyScale.x /3f);
            spawmOBJ.transform.position += spawmOBJ.transform.right * 1f;

            Collision.Invoke();
            firstHit = true;
            //transform.position = posCol;

            gameObject.SetActive(false);
        }
    }
    
    // Update is called once per frame
    
}
