using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Bullet_VFXBehavior : MonoBehaviour
{

	[SerializeField] UnityEvent EffectSelected;
	
	
	[SerializeField] VisualEffect visualEffect;
	[SerializeField] MeshRenderer meshRenderer;
	[SerializeField] AudioSource audioSource;
	[SerializeField] PlayableDirector playableDirector;
    [SerializeField] private float _lifeTime;
	[Range(-1,1)]
	[SerializeField] private float worldYAxisOffset;
	[SerializeField] private AudioClip thunderLighting;
	

    
	public int indexOfEffect;
	public SetOfEffects[] listOfEffects;
    
	private float _bornTime;
	private float _leftTimeToDestroy;
	private Material currentMaterial;
	private VisualEffect currentVFX;
	private TimelineAsset currentTimelineEffect;


	private readonly int constantSpawnRateFireVFXID = Shader.PropertyToID("ConstantSpawnRateFire");
	private readonly int constantSpawnRateGelVFXID = Shader.PropertyToID("ConstantSpawnRateGel");
	private readonly int constantSpawnRateTornadoVFXID = Shader.PropertyToID("ConstantSpawnRateTornado");
	private readonly int lightningSpawnRateVFXID = Shader.PropertyToID("LightningSpawnRate");
	private readonly int tornadoDensityVFXID = Shader.PropertyToID("tornadoDensity");
	private readonly int tornadoVarietyVFXID = Shader.PropertyToID("tornadoVariety");
	private readonly int tornadoSpeedVFXID = Shader.PropertyToID("tornadoSpeed");
	private readonly int tornadoWidthMinVFXID = Shader.PropertyToID("tornadoWidthMin");
	private readonly int tornadoWidthMaxVFXID = Shader.PropertyToID("tornadoWidthMax");



	[System.Serializable]
    public struct SetOfEffects
    {
        public VisualEffectAsset effectObject;
	    public Material material;
	    public TimelineAsset timelineEffect;
    }
    
    
    private void OnEnable()
    {
        
	    //indexOfEffect = Random.Range(0, listOfEffects.Length);
        //transform.forward = -Vector3.up;
        //transform.position += new Vector3(0f, 0.01f, 0f); 
        //meshRenderer.material = listOfEffects[indexOfEffect].material;
	    //visualEffect.visualEffectAsset = listOfEffects[indexOfEffect].effectObject;
	    //EffectSelected.Invoke();
        //_leftTimeToDestroy = 0;
	    //_bornTime = Time.time;
	    
    }
    
	public void SetEffectsData(int id)
	{
		transform.position = new Vector3(transform.position.x, worldYAxisOffset, transform.position.z); 
		meshRenderer.material =  listOfEffects[id].material;
		visualEffect.visualEffectAsset = listOfEffects[id].effectObject;
		playableDirector.playableAsset = listOfEffects[id].timelineEffect;
		currentTimelineEffect = listOfEffects[id].timelineEffect;
		//audioSource.clip = listOfEffects[id].audioClip[0];
		EffectSelected.Invoke();
		
		_leftTimeToDestroy = 0;
		_bornTime = Time.time;
		SetMaterialProperties(id);
		SetVFXProperties(id);
		SetTimelineProperties(id);
		
	}


    

    // Update is called once per frame
    void Update()
    {
        LifeTimeCounter();
        //Debug.Log(_leftTimeToDestroy.ToString());
	    if(_leftTimeToDestroy >= _lifeTime) { Object.Destroy(gameObject); }
        
	    
    }

    void LifeTimeCounter()
    {
        _leftTimeToDestroy = Time.time - _bornTime;
    }
    
	public void SetMaterialProperties(int id)
	{
		currentMaterial = GetComponent<MeshRenderer>().material;
		
		
		switch (id)
		{
		case 0:
			
			currentMaterial.SetFloat("_AlphaNoiseScale",Random.Range(15f,60f));
			currentMaterial.SetFloat("_EmmisionStrenght",Random.Range(10f,12f));
			currentMaterial.SetFloat("_UVNoiseScale",Random.Range(0f,10f));
			currentMaterial.SetFloat("_UVNoiseLerp",Random.Range(0.1f,0.3f));
			currentMaterial.SetFloat("_UVNoiseMultiplier",Random.Range(2f,4f));
			break;
		case 1:
			currentMaterial.SetFloat("_AlphaNoiseScale",Random.Range(20f,70f));
			currentMaterial.SetFloat("_EmmisionStrenght",Random.Range(5f,9f));
			currentMaterial.SetFloat("_UVNoiseScale",Random.Range(3f,12f));
			currentMaterial.SetFloat("_UVNoiseLerp",Random.Range(0f,0.35f));
			currentMaterial.SetFloat("_UVNoiseMultiplier",Random.Range(2f,5f));
			currentMaterial.SetFloat("_UVRotationRadians",Random.Range(0f,0.4f));
			
			break;
		case 2:
			
			currentMaterial.SetFloat("_AlphaNoiseScale",Random.Range(27f,50f));
			currentMaterial.SetFloat("_EmmisionStrenght",Random.Range(4f,7f));
			currentMaterial.SetFloat("_UVNoiseScale",Random.Range(30f,45f));
			currentMaterial.SetFloat("_UVNoiseLerp",Random.Range(0.8f,0.9f));
			currentMaterial.SetFloat("_UVNoiseMultiplier",Random.Range(-0.12f,0f));
			currentMaterial.SetVector("_UVOffsetTimeMultiplier",new Vector2(0f,Random.Range(-0.2f,-0.2f)));
			break;
		}
		
		
		
		
	}
	public void SetVFXProperties(int id)
	{
		currentVFX = GetComponent<VisualEffect>();
		
		switch (id)
		{
		case 0:
			currentVFX.SetInt(constantSpawnRateFireVFXID, Random.Range(20,30));
			
			break;
		case 1:
			currentVFX.SetInt(constantSpawnRateGelVFXID, Random.Range(5,20));
			
			break;
		case 2:
			currentVFX.SetInt(constantSpawnRateTornadoVFXID,Random.Range(50,75));
			currentVFX.SetInt(lightningSpawnRateVFXID,Random.Range(1,2));
			currentVFX.SetFloat(tornadoDensityVFXID,Random.Range(0.5f,1.5f));
			currentVFX.SetFloat(tornadoVarietyVFXID,Random.Range(0.5f,2.5f));
			currentVFX.SetFloat(tornadoSpeedVFXID,Random.Range(5f,10f));
			currentVFX.SetFloat(tornadoWidthMinVFXID,Random.Range(0.2f,0.3f));
			currentVFX.SetFloat(tornadoWidthMaxVFXID,Random.Range(0.4f,0.55f));
			
			break;
		}
	}
	
	public void SetTimelineProperties(int id)
	{
		//audioSource.clip = currentTimelineEffect.GetOutputTrack(0);
		playableDirector.Play();
		
			//audioSource.Play();
		switch (id)
		{
		case 0:
			//currentVFX.SetInt("ConstantSpawnRate",Random.Range(1,70));
			//currentTimelineEffect.Play();
			break;
		case 1:
			//currentVFX.SetInt("ConstantSpawnRate",Random.Range(1,20));
			//currentTimelineEffect.Play();
			//audioSource.volume =1;
			break;
		case 2:
			//currentVFX.SetInt("ConstantSpawnRate",Random.Range(1,100));
			//currentTimelineEffect.Play();
			break;
		}
		
	}
	
	public void LightingThunderAudio()
	{
		//Debug.Log("Thunder");
		
		audioSource.PlayOneShot(thunderLighting,0.4f);
	}
    
    
	

    
}
