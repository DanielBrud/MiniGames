using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class BulletBehavior : MonoBehaviour
{
    
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject vfx;
    [SerializeField] private Transform VFXParent;
	[SerializeField] private TrailRenderer trailRenderer;
	[SerializeField] private InterpolationScalar interpolationScalar;
	[SerializeField] private Color[] bulletColor;

    public Vector2 frequencyShotRange;
    public int bulletCount;
	public float bulletSpeed;
	public float chargingSpeed;
	
	[SerializeField] UnityEvent<Vector3,Color> playerHit;

    
	Material _enemyMaterial;
	Material _trailRendererMaterial;
	
    
    
    private Vector3 _fireforwardVector;
    private Vector3 _firePostion;
    private float _bulletSpeed;
	private bool canColide = false;
	private float laserVertexAniamtionValue;
	private bool canLaserVertexAnime = false;
	private int effectID;
	

    private void OnEnable()
    {
	    _enemyMaterial = enemy.GetComponent<MeshRenderer>().material;
	    _trailRendererMaterial = trailRenderer.material;


	    StartCoroutine(ShotFrequency());
	    Invoke("StartColiding",0.5f);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    transform.position +=  _fireforwardVector * Time.deltaTime * _bulletSpeed;
	    if(interpolationScalar.startInter)
	    {
	    	_enemyMaterial.SetFloat("_Charge",interpolationScalar.interpolationValue);
	    }
	    if(canLaserVertexAnime)
	    {
			LaserVertexAnimation(10);
	    }
    }

    public void Fire()
    {
        
        transform.position = enemy.transform.position;
        _fireforwardVector = enemy.transform.forward;
        _bulletSpeed = bulletSpeed;
	    trailRenderer.enabled = true;
	    interpolationScalar.startValue = 0;
	    interpolationScalar.startInter = false;
	    _enemyMaterial.SetFloat("_Charge",0.71f);
    }
    IEnumerator ShotFrequency()
    {
        for(int i = 0;i < bulletCount; i++)
        {
	        float wait = Random.Range(frequencyShotRange.x, frequencyShotRange.y);
	         effectID = Random.Range(0,bulletColor.Length);
	        _enemyMaterial.SetColor("_ChargeColor",bulletColor[effectID]);
	        _trailRendererMaterial.SetColor("_Color",bulletColor[effectID]);
	        interpolationScalar.scalarInterpolation = frequencyShotRange.x + chargingSpeed;
	        
	        
            yield return new WaitForSeconds(wait);
	        
            
	        interpolationScalar.StartInterpolaation();
	        

        }

    }
    
	void StartColiding()
	{
		if(!canColide){canColide = true;}
	}

    
    private void OnTriggerEnter(Collider other)
    {
	    
	    if (other.gameObject.layer == 11)
	    {
	    	transform.position = other.transform.position - new Vector3(0f,3f,0f);
	    }
	    
	    if (other.gameObject.layer == 6 && canColide)
        {
	        
            Vector3 forwarddirectionQuad = Vector3.ProjectOnPlane(_fireforwardVector, Vector3.up);
            Quaternion rotationPose = Quaternion.LookRotation(-Vector3.up, forwarddirectionQuad);
	        GameObject spawnObject =  Instantiate(vfx,transform.position, rotationPose, VFXParent);
	        spawnObject.GetComponent<Bullet_VFXBehavior>().SetEffectsData(effectID);
        }
        
	    if (other.gameObject.layer == 9 )
	    {
	    	
	    	playerHit.Invoke(transform.position,_trailRendererMaterial.color);
	    	Debug.Log("PlayerColoision");
	    }
	    
        trailRenderer.enabled = false;
        transform.SetPositionAndRotation(new Vector3(0, -10f, 0),Quaternion.identity);
        _bulletSpeed = 0;

    }
    
	void LaserVertexAnimation(float scalar)
	{
		
		if(laserVertexAniamtionValue < 1)
		{
			laserVertexAniamtionValue += Time.deltaTime * scalar;
			_enemyMaterial.SetFloat("_LaserAnimationValue",laserVertexAniamtionValue);
		}
		else
		{
			//canLaserVertexAnime = false;
			
			
		}
	
		
		
	}
	
	public void StartLaserVertexAnimation()
	{
		laserVertexAniamtionValue = 0;
		canLaserVertexAnime = true;
	}
	public void StopLaserVertexAnimation()
	{
		canLaserVertexAnime = false;
	}
	
	
}
