using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePortals_Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private OutsideGameBound outsideGameBound;
    [SerializeField] private InterpolationScalar interpolationScalar;

    public static bool hasPassPortal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {

            outsideGameBound.RestetPhysicsValues();
            hasPassPortal = false;
            interpolationScalar.interpolationValue = 0;
        }
    }
}
