using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBulletEffect : MonoBehaviour
{
    
	[SerializeField] private int maxPlayerHits;
	[SerializeField] private float poisenSpeed;
	
	
	private Material material;
	private int currentHit;
	private Color currentColor;
	private Color startColor;
	private float poisen;
	
	[SerializeField] private UnityEvent playerDie;
	
	
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		material = GetComponent<MeshRenderer>().material;
		startColor = material.color;
		currentColor = startColor;
		ResetHitCounter();
	}
	
	
    
	public void PlayerHit(Vector3 hitPosition,Color bulletColor)
	{
		
		material.color = bulletColor;
		currentHit += 1;
		
		Vector3 hitDirection = Vector3.Normalize(hitPosition - transform.position);
		
		if(currentHit > maxPlayerHits)
		{
			
			ResetHitCounter();
			playerDie.Invoke();
			
		}
	}
	
	
	public void PlayerEnterPoisenEffect()
	{
		
		currentColor = material.color;
		currentHit += 1;
		
		if(currentHit > maxPlayerHits)
		{
			
			playerDie.Invoke();
			
			
		}
	}
	
	
	public void PlayerPoisenEffect(Color poisenColor)
	{
		if(poisen < 1f)
		{
			poisen += poisenSpeed;
		}
		
		
		material.color = Color.Lerp(currentColor,poisenColor,poisen);
	}
	
	// Set poisen strength value	
	public void SetPoisenStrenght(float value)
	{
		poisen = value;
	}
	
	// Reset player current hit value	
	public void ResetHitCounter()
	{
		currentHit = 0;
	}
}
