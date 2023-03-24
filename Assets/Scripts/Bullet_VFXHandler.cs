using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Bullet_VFXHandler : MonoBehaviour
{

	public GameObject player;
	GameObject[] childs;
	[SerializeField] UnityEvent EnterColideWithVFXEffect;
	[SerializeField] UnityEvent ExitColideWithVFXEffect;
	[SerializeField] UnityEvent StayColideWithVFXEffect;
	[SerializeField] UnityEvent<Color> PoisenVFXEffect;
	
    
    
   

    public void DestoryChild()
    {
        int childCount = transform.childCount;

        childs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childs[i] = transform.GetChild(i).gameObject;

            Object.Destroy(childs[i]);
        }
    }
    
	public void VFXEffectEnterColisiion()
	{
		EnterColideWithVFXEffect.Invoke();
	}
	public void VFXEffectExitColisiion()
	{
		ExitColideWithVFXEffect.Invoke();
	}

	public void VFXEffectStayColisiion(Color coliderColor)
	{
		StayColideWithVFXEffect.Invoke();
		PoisenVFXEffect.Invoke(coliderColor);
	}


}
