using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UITriggerHandler : MonoBehaviour
{
   
    [SerializeField] private UnityEvent<Collider> TriggerEnter;
    [SerializeField] private UnityEvent<Collider> TriggerExit;


    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExit.Invoke(other);
    }


}
