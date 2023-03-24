using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBahavior : MonoBehaviour
{

    [SerializeField] private Transform _traget;
    [SerializeField] private GameObject _hole;
	[SerializeField] private InterpolationScalar InterpolationScalar;
	[SerializeField] private Material material;
	
    //[SerializeField] private GameObject bullet;

	public Vector2 bulletDirectionThreshhold;
	public Vector2 bulletCollisionDirectionThreshhold;
    public Vector3 enemySpaceBoundBahavior;
    [Range(0,10f)]
    public float smoothTime;


    private Vector3 _enemyNextTargetPosition;
    private Vector3 _enemyPreviousTargetPosition;
	private Vector3 _velocityPlayer;
	private float _bulletLerpDirection;
	private float _bulletCrossDirection;
	private Vector2 startBulletDirectionValue;
    

    private void OnEnable()
    {
        
	    startBulletDirectionValue = bulletDirectionThreshhold;
	    
        _velocityPlayer = Vector3.zero;
        transform.position = _hole.transform.position + new  Vector3(0f, 0.1f,0f);
        ChangeEnemyPostionTarget();
        InterpolationScalar.StartInterpolaation();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        
    }

    void SpawnPosition()
    {
        transform.position = new Vector3( Mathf.Lerp(_traget.position.x, _hole.transform.position.x,0.5f), Mathf.Lerp(0f,Random.Range(0.2f,2f),0.5f), Mathf.Lerp(_traget.position.z, _hole.transform.position.z, 0.5f));
    }

    void UpdatePosition()
    {
        if (InterpolationScalar.startInter || InterpolationScalar.reversInter)
        {
            
            transform.position = Vector3.SmoothDamp(transform.position, _enemyNextTargetPosition, ref _velocityPlayer, smoothTime);
	        
	        
	        Vector3 bulletShotDirection = Vector3.Lerp(_traget.position,_hole.transform.position,_bulletLerpDirection) + Vector3.Cross(Vector3.Normalize(_traget.position -_hole.transform.position),Vector3.up) * _bulletCrossDirection ;
	         
	        
	        transform.forward = Vector3.Normalize(bulletShotDirection - transform.position);
            
        }

    }
	public void SetStartBulletDirectionTresholdValue()
	{
		bulletDirectionThreshhold = startBulletDirectionValue;
	}
	
	public void SetCollisionBulletDirectionTresholdValue()
	{
		bulletDirectionThreshhold = bulletCollisionDirectionThreshhold;
    
	}
    

    public void ChangeEnemyPostionTarget()
    {
        
        smoothTime = Random.Range(1f, 3.5f);
	    InterpolationScalar.tresholdUP = Random.Range(0.3f, 0.75f);
	    _bulletLerpDirection = Random.Range(bulletDirectionThreshhold.x,bulletDirectionThreshhold.y);
	    _bulletCrossDirection = Random.Range(-bulletDirectionThreshhold.x,bulletDirectionThreshhold.x);
        _enemyPreviousTargetPosition = _enemyNextTargetPosition;
        _enemyNextTargetPosition = new Vector3(Mathf.Cos(Random.Range(0.1f, 10f)) * enemySpaceBoundBahavior.x, Mathf.Abs(Mathf.Cos(Random.Range(0.1f, 10f)) * enemySpaceBoundBahavior.y), Mathf.Sin(Random.Range(0.1f, 10f)) * enemySpaceBoundBahavior.z);
    }
}
