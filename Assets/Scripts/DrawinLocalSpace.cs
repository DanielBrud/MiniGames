using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;



public class DrawinLocalSpace : MonoBehaviour
{

	
	[SerializeField] Shader shaderBullet;
	[SerializeField] ComputeShader computeTextureDrawing;
	[SerializeField] InterpolationScalar interpolationScalar;
	[SerializeField] MeshRenderer meshRenderer;
	Bullet_VFXHandler vFXHandler;
	[SerializeField] float disctanceFromPlayerTreshold;
	[SerializeField] SphereCollider collider;


	public Vector3[] objects;
	
	public float splashSizeTreshold;
	public float splashDirectionTreshold;
	public float splashDistanceTeshold;

	
	
	private GameObject player;
	private VisualEffect visualEffect;
	private Material material;
	private Material playerMaterial;
	private FollowTarget playerFollow;
	Vector3 []randomDirection;
	float []randomDistance;
	float[] splashSize;
	private Vector3 splashCenter;
	private Vector3 spashDirection;
	private float distanceToPlayer;
	private bool isCloseToPolayer = false;
	private Color coliderColor;
    


    int paitingObjectsArrayID = Shader.PropertyToID("piantObjectPosition");
    //int paitingObjectsArrayIDGPU = Shader.PropertyToID("piantObjectPositionGPU");
    int bushSize = Shader.PropertyToID("brushSize");
    RenderTexture renderTexture;
    GraphicsBuffer paintingObjectsBuffer;

	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		//SetRandomProperites(objects.Length);
	}
	
	
	private void OnEnable()
    {
		//renderTexture.enableRandomWrite = true;

	    vFXHandler = GetComponentInParent<Bullet_VFXHandler>();
		//renderTexture.Create();
	    material = GetComponent<MeshRenderer>().sharedMaterial;
	    //material = GetComponent<MeshRenderer>().material;
	    
	    //CreateRenderTxture(ref renderTexture, 512, 512);
	    //CreateNewMaterialInstance();
	    //CreateGraphicBuffer();
	    //CrateDrawingComputeUnit();
		FindPlayer();


		SetRandomProperites(objects.Length);
	    spashDirection = transform.up;
	    splashCenter = transform.position;

	}
    
	public void AssingSelectedEffectPropierties()
	{
		material = GetComponent<MeshRenderer>().material;
		//CreateNewMaterialInstance();
		visualEffect = GetComponent<VisualEffect>();
		interpolationScalar.StartInterpolaation();
	}
    
   

    // Update is called once per frame
    void Update()
    {
        
        
	    if(interpolationScalar.startInter)
	    {
	    	//PaintingObjectsBehavior(objects.Length,objects);
		    PaintingSimple();
		    //SetRandomProperites();
		    //ComputePaintingObjects();
	    }
            
	    //CalculateDistanceToPlayer(objects.Length,objects);
	    //CalculateDistranceToPlayerSimple();

    }
    
	void FindPlayer()
	{
		 
		player = GetComponentInParent<Bullet_VFXHandler>().player;
		playerMaterial = player.GetComponent<MeshRenderer>().material;
		playerFollow = player.GetComponentInParent<FollowTarget>();
		
	}
	
	void ComputePaintingObjects()
    {
        // Calculate all at one dispatch

        //computeTextureDrawing.SetVectorArray(paitingObjectsArrayIDGPU, testobjectsGPU);
	    paintingObjectsBuffer.SetData(objects);
	    computeTextureDrawing.SetFloat("brushSizeTreshhold",splashSizeTreshold);
        computeTextureDrawing.SetBuffer(0, "piantObjectPosition", paintingObjectsBuffer);
        computeTextureDrawing.SetTexture(0, "DrawTexture", renderTexture);
        computeTextureDrawing.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        // Calculate in loop dispatch

        //for ( int i = 0; i < testobjects.Length; i++)
        //{
        //    computeTextureDrawing.SetTexture(0, "DrawTexture", renderTexture);
        //    computeTextureDrawing.SetVector(paitingObjectsArrayID, testobjects[i]);
        //    computeTextureDrawing.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
        //}

        if (renderTexture != null)
        {
            material.SetTexture("_BulletTexture", renderTexture);
        }

        //visualEffect.SetTexture("MyRenderTexture", renderTexture);
    }

    void CreateRenderTxture(ref RenderTexture rt, int w, int h)
    {
        rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.sRGB);
        rt.enableRandomWrite = true;
        rt.Create();
    }

    void CreateGraphicBuffer()
    {
        paintingObjectsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, objects.Length, 3 * sizeof(float));
        paintingObjectsBuffer.SetData(objects);


    }
    
	
	void CrateDrawingComputeUnit()
    {
        computeTextureDrawing.SetTexture(0, "DrawTexture",renderTexture);
        
    }

	void PaintingObjectsBehavior(int objectCount,Vector3[] objects)
    {
        
	    for(int i=0; i< objectCount;i++)
	    {
	    	//objects[i] = transform.InverseTransformDirection(randomDirection[i]) * interpolationScalar.interpolationValue;
	    	objects[i] = randomDirection[i] * (interpolationScalar.interpolationValue * randomDistance[i]);

	    }
        

    } 
	void PaintingSimple()
	{
		disctanceFromPlayerTreshold = interpolationScalar.interpolationValue;
		collider.radius = disctanceFromPlayerTreshold;
		material.SetFloat("_DistanceFromColision",disctanceFromPlayerTreshold);
	}

	void CalculateDistanceToPlayer(int objectCount,Vector3[] objects)
    {
	    Vector3 playerpostion = player.transform.position;
	    //Vector3 playerpostion = transform.InverseTransformPoint(player.transform.position);
	    Debug.DrawLine(splashCenter,playerpostion,Color.blue);
	    //Debug.Log(playerpostion.ToString());
	    

		for (int i = 0; i < objectCount; i++)
        {
			//distanceToPlayer = Vector3.Distance(playerpostion,splashCenter + transform.TransformPoint( objects[i]));
			distanceToPlayer = Vector3.Distance(playerpostion, transform.TransformPoint( objects[i] ));
			
			Debug.DrawLine(splashCenter,transform.TransformPoint( objects[i]),Color.red);
			
			//distanceToPlayer = Vector3.Distance(playerpostion,transform.InverseTransformPoint( objects[i]));
			

			if(distanceToPlayer < disctanceFromPlayerTreshold)
            {
				Debug.Log(distanceToPlayer.ToString());
				if(isCloseToPolayer)
				{
					isCloseToPolayer = true;
					continue;
					//break;
				}
				//StartBulletForce.Invoke();
				playerFollow.playerSmoothDampValue = 1f;
				//playerMaterial.SetColor("_Color",Color.blue);
				//Debug.Log("COLSE!!!!!");
				//Debug.Log(player.GetComponentInParent<FollowTarget>().playerSmoothDampValue.ToString());
				//isCloseToPolayer = true;
				
				
            }
			
			else if(isCloseToPolayer)
			{
				
				
				//playerMaterial.SetColor("_Color",Color.red);
					Debug.Log("FAR!!!!!");
					isCloseToPolayer = false;
				
				
			}

        }
	    
	    
	    
			
    }
    
	void CalculateDistranceToPlayerSimple()
	{
		Vector3 playerpostion = player.transform.position;
		distanceToPlayer = Vector3.Distance(playerpostion, transform.position );
		if(distanceToPlayer < disctanceFromPlayerTreshold)
		{
			//Debug.Log(distanceToPlayer.ToString());
			//Debug.Log(disctanceFromPlayerTreshold.ToString());
			if(isCloseToPolayer)
			{
				isCloseToPolayer = true;
				//continue;
				//break;
			}
			//StartBulletForce.Invoke();
			playerFollow.playerSmoothDampValue = 1f;
			//playerMaterial.SetColor("_Color",Color.blue);
			//Debug.Log("COLSE!!!!!");
			//Debug.Log(player.GetComponentInParent<FollowTarget>().playerSmoothDampValue.ToString());
			//isCloseToPolayer = true;
				
				
		}
			
		else if(isCloseToPolayer)
		{
				
				
			//playerMaterial.SetColor("_Color",Color.red);
			//Debug.Log("FAR!!!!!");
			isCloseToPolayer = false;
				
				
		}
	}
    
	void IsColseToPlayer(bool isColse)
	{
		isCloseToPolayer = isColse;
	}
    
	// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
	protected void OnDrawGizmos()
	{
		//for (int i = 0; i < 10; i++){Gizmos.DrawSphere(transform.TransformPoint( objects[i]),0.05f);}
		
	}
    
	void CreateNewMaterialInstance()
	{
		//Material newMaterial = new Material(shaderBullet);
		//material = GetComponent<MeshRenderer>().sharedMaterial;
		//material.SetTexture("_BulletTexture", renderTexture);
		//meshRenderer.material = newMaterial;
		//if (renderTexture != null)
		//{
		//	material.SetTexture("_BulletTexture", renderTexture);
		//}
		//newMaterial.SetColor("_Color", Random.ColorHSV());
	}
    
    
	void SetRandomProperites(int objectCount)
    {
	    randomDirection = new Vector3[objectCount];
	    randomDistance = new float[objectCount];
	    //splashSize = new float[objectCount*4];
	    
	    for(int i =0; i< objectCount;i++)
	    {
			
			//Random.seed = i;
		    randomDirection[i] = Vector3.Normalize( new Vector3(Random.Range(-1f, 1f), Random.Range(-1f,splashDirectionTreshold),0));
		    randomDistance[i] = Random.Range(0f, splashDistanceTeshold);
		    //splashSize[i*4] = Random.Range(0.01f, splashSizeTreshold);
		    
		    
		    //Debug.Log(randomDirection[i].ToString());
		    //Debug.Log(randomDistance[i].ToString());
		    //Debug.Log(splashSize[i].ToString());
	    }
		 
	    //computeTextureDrawing.SetFloats(bushSize,splashSize);
	    computeTextureDrawing.SetFloat("brushSizeTreshhold",splashSizeTreshold);
        
    }
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		//playerFollow.playerSmoothDampValue = 1f;
		coliderColor = material.GetColor("_EdgeColor");
		//playerMaterial.SetColor("_Color",Color.blue);
		vFXHandler.VFXEffectEnterColisiion();
		//Debug.Log(coliderColor.ToColorf());
	}
	// OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
	protected void OnTriggerStay(Collider other)
	{
		//playerFollow.playerSmoothDampValue += 0.01f;
		
		vFXHandler.VFXEffectStayColisiion(coliderColor);
		//Debug.Log(playerFollow.playerSmoothDampValue.ToString());
	}
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit(Collider other)
	{
		//playerFollow.playerSmoothDampValue = 0.5f;
		//playerMaterial.SetColor("_Color",Color.red);
		vFXHandler.VFXEffectExitColisiion();
	}
	// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
	//protected void OnCollisionEnter(Collision collisionInfo)
	//{
	//	Debug.Log("Colide");
	//}
    
    
    
    
    
    
    
    
	
	private void OnDisable()
    {
        
	    //playerMaterial.SetColor("_Color",Color.red);
	    //renderTexture?.Release();
	    //renderTexture = null;
	    //paintingObjectsBuffer?.Dispose();
	    //paintingObjectsBuffer = null;
  //      if (material != null)
		//{
		//	DestroyImmediate(material);
		//}
	    //StopBulletForce.Invoke();
	    playerFollow.playerSmoothDampValue  = 0.5f;
		interpolationScalar.startInter = false;
	    interpolationScalar.interpolationValue = 0;
		interpolationScalar.startValue = 0;


	}
    
	
}
