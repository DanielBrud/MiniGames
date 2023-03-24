using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Oculus.Interaction;
using UnityEngine.Events;
using UnityEngine.VFX;

public class RayEventHandler : MonoBehaviour
{
    [SerializeField] RayInteractor LeftrayInteractor;
    [SerializeField] IndexPinchSelector LerftindexPinch;
    [SerializeField] RayInteractor RightrayInteractor;
    [SerializeField] IndexPinchSelector RightindexPinch;
    [SerializeField] RichPoint richPoint;
    [SerializeField] GameObject prefabLine;
    [SerializeField] GameObject LeftPointer;
	[SerializeField] GameObject RightPointer;
	[SerializeField] Transform firstTarget;
	[SerializeField] Transform secondTarget;
	
    
	
    public RaycastHit righthit;
    public RaycastHit lefthit;
    public bool isleftHit;
    public bool isrightHit;
    public bool stateLeftHit;
    public bool stateRightHit;
    public UnityEvent  Select;
    public UnityEvent Unselect;
	public UnityEvent NotRich;
    
    
    
	int mask;
	private GameObject LeftLine;
	private GameObject RightLine;
	private Material materialLeftPointer;
	private Material materialRightPointer;
	
	private Vector3 leftTarget;
	private Vector3 rightTarget;



    private void OnEnable()
    {
        RightindexPinch.WhenSelected += RightSelected;
        RightindexPinch.WhenUnselected += RightUnSelected;
        LerftindexPinch.WhenSelected += LeftSelected;
        LerftindexPinch.WhenUnselected += LeftUnSelected;

		SetDafualtTargetsForPointers();
        
    }
    private void OnDisable()
    {
        RightindexPinch.WhenSelected -= RightSelected;
        RightindexPinch.WhenUnselected -= RightUnSelected;
        LerftindexPinch.WhenSelected -= LeftSelected;
        LerftindexPinch.WhenUnselected -= LeftUnSelected;
        ResteProperites();
    }

    // Start is called before the first frame update
    void Start()
    {
	    mask = LayerMask.GetMask("Collide");
	    materialLeftPointer = LeftPointer.GetComponent<MeshRenderer>().material;
	    materialRightPointer = RightPointer.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(rayInteractor.Ray.origin, rayInteractor.Ray.direction, Color.red);
        //rayInteractorDebugPolylineGizmos.InjectRayInteractor(rayInteractor);
        
        if(Physics.Raycast(LeftrayInteractor.Ray,out lefthit, 30f, mask))
        {
	        Vector3 leftPointerPosition = new Vector3(lefthit.point.x,0, lefthit.point.z);
	        LeftPointer.transform.position = leftPointerPosition;
	        
	        Vector3 targetDirection = Vector3.Normalize(leftTarget - leftPointerPosition);
	        LeftPointer.transform.rotation = Quaternion.LookRotation(lefthit.normal,targetDirection);
            
            if (!stateLeftHit) stateLeftHit = true;
        }
        else if(stateLeftHit) stateLeftHit = false;

        if (Physics.Raycast(RightrayInteractor.Ray, out righthit, 30f, mask))
        {
	        Vector3 rightPointerPosition = new Vector3(righthit.point.x, 0, righthit.point.z);
	        RightPointer.transform.position = rightPointerPosition;
	        
	        Vector3 targetDirection = Vector3.Normalize(rightTarget - rightPointerPosition);
	        RightPointer.transform.rotation = Quaternion.LookRotation(righthit.normal,targetDirection);
	        

            
            if (!stateRightHit) stateRightHit = true;
        }
        else if (stateRightHit) { stateRightHit = false; }

        // sphere.transform.position = rayInteractor.End;
    }
	void SetOculusMaterialPointerProperties(Material pointerMaterial, Color innerColor, Color outlineColor,float alpha = 1f, float radialGradientIntensity = 1f,float radialGradienScale = 0.263f, float radialGradientBackgroundOpacity = 0.3f, float radialGradientOpacity = 1f)
	{
		
		pointerMaterial.SetColor("_Color", innerColor);
		pointerMaterial.SetColor("_OutlineColor", outlineColor);
		
		pointerMaterial.SetFloat("_Alpha", alpha);
		pointerMaterial.SetFloat("_RadialGradientIntensity;", radialGradientIntensity);
		pointerMaterial.SetFloat("_RadialGradientScale", radialGradienScale);
		pointerMaterial.SetFloat("_RadialGradientBackgroundOpacity", radialGradientBackgroundOpacity);
		pointerMaterial.SetFloat("_RadialGradientOpacity", radialGradientOpacity);
	}

    void LeftSelected()
    {
        if (stateLeftHit) 
        {
            isleftHit = true;
            LeftLine = Instantiate(prefabLine, lefthit.point,Quaternion.identity, gameObject.transform);
            //LeftLine.transform.SetParent(gameObject.transform);
            LeftLine.tag = "Left";

        }
	    SetOculusMaterialPointerProperties(materialLeftPointer,Color.red,Color.black,1f,1f,0.21f,1f,1f);
        
	    float distanceFirstTarget = Vector3.Distance(firstTarget.position,LeftPointer.transform.position);
	    float distanceSecondTarget = Vector3.Distance(secondTarget.position,LeftPointer.transform.position);
		
		
	    leftTarget = distanceSecondTarget > distanceFirstTarget ? secondTarget.position : firstTarget.position;
        //Debug.Log("Select");
        Select.Invoke();
    }
    void LeftUnSelected()
    {
        isleftHit = false;

        if(!richPoint.isConnected)
        {
            if (LeftLine != null) { Object.Destroy(LeftLine); }
            NotRich.Invoke();
        }
	    richPoint.isConnected = false;
	    SetOculusMaterialPointerProperties(materialLeftPointer,Color.white,Color.black,1f,1f,0.5f,0.3f,1f);
	    leftTarget = secondTarget.transform.position;
        //Debug.Log("UNSelect");
        Unselect.Invoke();
    }
    void RightSelected()
    {
        if(stateRightHit) 
        {
            isrightHit = true;

            RightLine = Instantiate(prefabLine, righthit.point,Quaternion.identity, gameObject.transform);
           //RightLine.transform.SetParent(gameObject.transform);
            RightLine.tag = "Right";
            
        }
	    SetOculusMaterialPointerProperties(materialRightPointer,Color.red,Color.black,1f,1f,0.21f,1f,1f);
	    
	    float distanceFirstTarget = Vector3.Distance(firstTarget.position,RightPointer.transform.position);
	    float distanceSecondTarget = Vector3.Distance(secondTarget.position,RightPointer.transform.position);
	    
	    rightTarget = distanceSecondTarget > distanceFirstTarget ? secondTarget.position : firstTarget.position;
        //Debug.Log("Select");
       Select.Invoke();
    }
    void RightUnSelected()
    {
        isrightHit = false;

        if(!richPoint.isConnected)
        {
            if(RightLine != null) { Object.Destroy(RightLine); }
            NotRich.Invoke();

        }
	    richPoint.isConnected = false;
	    SetOculusMaterialPointerProperties(materialRightPointer,Color.white,Color.black,1f,1f,0.5f,0.3f,1f);
	    //Debug.Log("UNSelect");
	    rightTarget = firstTarget.transform.position;
	    Unselect.Invoke();
    }
    
	public void SetDafualtTargetsForPointers()
	{
		rightTarget = firstTarget.transform.position;
		leftTarget = secondTarget.transform.position;
	}
	public void SetDafualtMaterialPointerProperties()
	{
		SetOculusMaterialPointerProperties(materialLeftPointer,Color.white,Color.black,1f,1f,0.5f,0.3f,1f);
		SetOculusMaterialPointerProperties(materialRightPointer,Color.white,Color.black,1f,1f,0.5f,0.3f,1f);
	}

    void ResteProperites()
    {

        transform.position = Vector3.zero;
        int childerCount = transform.childCount;

        GameObject[] childs = new GameObject[childerCount];

        for (int i = 0; i < childerCount; i++) 
        { 
            childs[i] = transform.GetChild(i).gameObject; 
            childs[i].transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity); 
            if(childs[i].GetComponent<LineRenderer>() != null)
            {
                 Object.Destroy(childs[i]);
            }
        
        }

        
        
        
        //levelUp = 0;
    }
}

