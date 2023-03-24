using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;
using UnityEngine.Events;

public class RichPoint : MonoBehaviour
{

    [SerializeField] GameObject[] targets;
    [SerializeField] Transform[] disttoTarget;
    Material[] targetMaterial;
	[SerializeField] RayEventHandler rayEvent;
	[SerializeField] GameObject vfxPointers;
    Material material;
    public bool[] closeTarget;
    public bool isConnected;
    public float all = 0;
    float previousFill;
    public float partialFillvalue;
    public float distanceTresholdToTarget;
    public float filSpeed;
    public Vector3 spawnSpaceBound;

	[SerializeField] UnityEvent TargetAchived;
    [SerializeField] UnityEvent FinishExperience;
    public delegate void targetConnected();
    public event targetConnected Connection;
	
	VisualEffect vfxPointersGraph;



    private void OnEnable()
    {

        material = GetComponent<MeshRenderer>().material;
        targetMaterial = new Material[targets.Length];
        closeTarget = new bool[targets.Length];

	    vfxPointersGraph = vfxPointers.GetComponent<VisualEffect>();
        for (int i = 0; i < targets.Length ; i++)
        {
            targetMaterial[i] = targets[i].GetComponent<MeshRenderer>().material;
            closeTarget[i] = false;


        }

        RandomPosRestMat();
        
    }

    private void OnDisable()
    {
        all = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if(rayEvent.isleftHit)  { Distcalc(disttoTarget[0].position);vfxPointers.transform.position = disttoTarget[0].position; }
	    if(rayEvent.isrightHit) { Distcalc(disttoTarget[1].position);vfxPointers.transform.position = disttoTarget[1].position; }
	    FillMaterialObject();
	    //PointerVFXpostion();
    }

    void Distcalc(Vector3 point )
    {
        TargetAchive();

        for (int y = 0; y < targets.Length; y++)
        {
            if (Vector3.Distance(targets[y].transform.position, point) < distanceTresholdToTarget && !closeTarget[y])
            {
                //targetMaterial.SetColor("BaseColor",Color.red);
                targetMaterial[y].color = Color.red;
                closeTarget[y] = true;
            }
        }
        

        

        GoalAchived();

    }

    public void RandomPosRestMat()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targetMaterial[i].color = Color.white;
            targets[i].transform.SetPositionAndRotation(new Vector3(Random.Range(-spawnSpaceBound.x, spawnSpaceBound.x), -0.01f, Random.Range(-spawnSpaceBound.z, spawnSpaceBound.x)), Quaternion.identity);

            //targets[i].transform.localPosition = new Vector3(Random.Range(-10, 10), -3.2f, Random.Range(0, 10));
            //targets[i].transform.position = Vector3.zero;
           // targets[i].transform.position += transform.TransformPoint(new Vector3(Random.Range(-3, 3), 0, Random.Range(0, 3)));
        }
    }
    void TargetAchive()
    {


        //for (int i = 0; i < targets.Length; i++)
        //{
        //     //closeTarget[i] ;
        //    if (closeTarget[i] && closeTarget[ Mathf.Min(closeTarget.Length -1,  i +1)]) { RandomPosRestMat(); closeTarget[i] = false; material.SetFloat("_Fill", all += partialFillvalue/2f); rayEvent.isleftHit = false; rayEvent.isrightHit = false; };
        //}

        if (closeTarget[0] && closeTarget[1]) 
        {
            //material.SetFloat("_Fill", all += partialFillvalue); 
            all += partialFillvalue;
            rayEvent.isleftHit = false; 
            rayEvent.isrightHit = false;
            closeTarget[0] = false;
            closeTarget[1] = false;
            
            RandomPosRestMat();
            isConnected = true;
            //targetConnection();
	        //Connection.Invoke();
	        TargetAchived.Invoke();
        }


    }

    public void resetTargetValue()
    {
        for(int i = 0; i < targets.Length; i++)
        
        {
            closeTarget[i] = false;
            
            targetMaterial[i].color = Color.white;
        }
        
    }

    void GoalAchived()
    {
        if (all >= 1f) 
        { 
            
            all = 0;
            previousFill = 0;
            material.SetFloat("_Fill", all);
            closeTarget[0] = false;
            closeTarget[1] = false;
            isConnected = false;
            FinishExperience.Invoke();
            gameObject.SetActive(false);
        }
    }
    void FillMaterialObject()
    {
        if (previousFill <= all)
        {
            previousFill += Time.deltaTime * filSpeed;
            material.SetFloat("_Fill", previousFill);
        }
    }
    
	//void PointerVFXpostion()
	//{
	//	if(rayEvent.isleftHit)
	//	{
			
			
	//	}
	//	if(rayEvent.isrightHit)
	//	{
			
	//		vfxPointers.transform.position = disttoTarget[1].position;
	//	}
		
		
	//}
	public void PointerVFXStop()
	{
		if(!rayEvent.isleftHit && !rayEvent.isrightHit)
		{
			vfxPointersGraph.Stop();
		}
	}
	public void PointerVFXStart()
	{
		if(rayEvent.isleftHit || rayEvent.isrightHit)
		{
			vfxPointersGraph.Play();
		}
	}
    //void targetConnection() { }
}
