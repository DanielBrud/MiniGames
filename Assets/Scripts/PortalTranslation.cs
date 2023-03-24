using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class PortalTranslation : MonoBehaviour
{
    Vector3 initialPosition;
    Quaternion currentRotation;
    [SerializeField] OneGrabFreeTransformer oneGrabFreeTransformer;

    public void InitialPostion()
    {
        oneGrabFreeTransformer.InitialPosition = transform.position;
    }


    
    // Update is called once per frame
    void Update()
    {
        //currentRotation = transform.rotation;
    }
    private void LateUpdate()
    {
        //transform.position = initialPosition;
        //transform.rotation = Quaternion.Inverse(currentRotation);
    }


}
