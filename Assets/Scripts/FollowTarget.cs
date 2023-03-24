using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.Events;
using UnityEngine.Audio;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private RayInteractor LeftrayInteractor;
    [SerializeField] private IndexPinchSelector LerftindexPinch;
    [SerializeField] private RayInteractor RightrayInteractor;
    [SerializeField] private IndexPinchSelector RightindexPinch;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject LeftPointer;
    [SerializeField] private GameObject RightPointer;
	[SerializeField] private GameObject Hole;
	[SerializeField] private GameObject BulletOne;
	[SerializeField] private GameObject BulletTwo;
    [SerializeField] private LayerMask mask;
	[SerializeField] private float playerrotationSpeed;
	[SerializeField] private float playerAudioVolumeMultiplier;
    [SerializeField] private float playerEmsisionScalar; 
    [SerializeField] private AudioSource playerAudio;
	[SerializeField] private AudioMixer playerMixer;
    
    public UnityEvent HoleAchived;
    public UnityEvent Select;
    public UnityEvent Unselect;
    public UnityEvent LeftHover;
    public UnityEvent LeftUnHover;
    public UnityEvent RightHover;
    public UnityEvent RightUnHover;
	public UnityEvent FinishExperience;
    public UnityEvent StartNewRound;

    
	public float playerSmoothDampValue;
	public float distTreshhold;
    public float playerAboveGround;
    public Vector2 spawHoleBounds;
    public int maxLevels;
    
    public RaycastHit righthit;
    public RaycastHit lefthit;
    
    public bool isleftHit;
    public bool isrightHit;
    public bool stateLeftHit;
    public bool stateRightHit;

	 
	
	private PointerVisual materialLeftPointer;
	private PointerVisual materialRightPointer;
	private Material playerMaterial;
    private float tLeftpointer;
    private float tRightpointer;
    private Vector3 targetPos;
    private Vector3 playerOffset;
    private Vector3 velocityPlayer;
    private Vector3 leftPointerPosition;
    private Vector3 rightPointerPosition;
    private Vector3 currentPlayerPostion;
    private Vector3 previousPlayerPostion;
    private float currentPlayerRotation;
    private int levelUp;
    private Rigidbody playerRigidBody;
	private float distanceFromPortalTreshhold;
	private bool isGameActive;
	private bool canPlayerMove;
	
    



    private void OnEnable()
    {
        RightindexPinch.WhenSelected += RightSelected;
        RightindexPinch.WhenUnselected += RightUnSelected;
        LerftindexPinch.WhenSelected += LeftSelected;
        LerftindexPinch.WhenUnselected += LeftUnSelected;
        playerRigidBody = PlayerObject.GetComponent<Rigidbody>();
	    PlayerRandomPos();
	    canPlayerMove = true;
	    isGameActive = true;
	    distanceFromPortalTreshhold = distTreshhold;
	    levelUp = 0;
	    // HoleRandomPos();
    }
    private void OnDisable()
    {
        RightindexPinch.WhenSelected -= RightSelected;
        RightindexPinch.WhenUnselected -= RightUnSelected;
        LerftindexPinch.WhenSelected -= LeftSelected;
        LerftindexPinch.WhenUnselected -= LeftUnSelected;
        ResteProperites();
	    isGameActive  = false;


    }
    // Start is called before the first frame update
    void Start()
    {
	    
	    playerMaterial = PlayerObject.GetComponent<MeshRenderer>().material;
        materialLeftPointer = LeftPointer.GetComponent<PointerVisual>();
        materialRightPointer = RightPointer.GetComponent<PointerVisual>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(LeftrayInteractor.Ray, out lefthit, 30f, mask))
        {
            leftPointerPosition = new Vector3(lefthit.point.x,0, lefthit.point.z);
            LeftPointer.transform.position = leftPointerPosition;
	        
	        Vector3 holeDirection = Vector3.Normalize(Hole.transform.position - leftPointerPosition);
	        LeftPointer.transform.rotation = Quaternion.LookRotation(lefthit.normal,holeDirection);

            if (!stateLeftHit) { stateLeftHit = true; LeftHover.Invoke(); }
        }
        else if (stateLeftHit) { stateLeftHit = false; LeftUnHover.Invoke(); }

        if (Physics.Raycast(RightrayInteractor.Ray, out righthit, 30f, mask))
        {
            rightPointerPosition = new Vector3(righthit.point.x, 0, righthit.point.z);
            RightPointer.transform.position = rightPointerPosition;
	        
	        Vector3 holeDirection = Vector3.Normalize(Hole.transform.position - rightPointerPosition);
	        
	        RightPointer.transform.rotation = Quaternion.LookRotation(righthit.normal,holeDirection);
	        
	        
	        

            if (!stateRightHit) { stateRightHit = true; RightHover.Invoke(); }
        }
        else if (stateRightHit) { stateRightHit = false; RightUnHover.Invoke(); }

	    if(distanceFromPortalTreshhold > 0 && canPlayerMove)
        {
            FollowPointer();
        }
        
        CheckDistance();
    }

    void LeftSelected()
    {
        isleftHit = true;
        materialLeftPointer.SelectedOculusMaterialPointerProperties();
	  
    }
    
	void LeftUnSelected()
    {
        isleftHit = false;
        materialLeftPointer.UnSelectedOculusMaterialPointerProperties();

    }
    
	void RightSelected()
    {
        isrightHit = true;
        materialRightPointer.SelectedOculusMaterialPointerProperties();

    }
    
	void RightUnSelected()
    {
        isrightHit = false;
        materialRightPointer.UnSelectedOculusMaterialPointerProperties();
    }
	
	
	// Move player postion in pointer direction with some dump 
    void FollowPointer()
    {
        if (stateLeftHit && isleftHit && !isrightHit) { targetPos = leftPointerPosition; }
        else if(stateRightHit && isrightHit && !isleftHit) { targetPos = rightPointerPosition; }

        Vector3 playerDirection = Vector3.Normalize( PlayerObject.transform.position - (targetPos + playerOffset));

        float playerVelocity = Vector3.Distance(currentPlayerPostion, previousPlayerPostion);
	    
        previousPlayerPostion = PlayerObject.transform.position;
		
        PlayerObject.transform.right = Vector3.Cross(playerDirection, Vector3.up);
		
	    
	    
	    
	    playerMixer.SetFloat("BaseSpeedVolume",Mathf.Max(-20f,Mathf.Log10(playerVelocity)*playerAudioVolumeMultiplier));
	    playerMixer.SetFloat("MoreSpeedVolume",Mathf.Log10(playerVelocity)*playerAudioVolumeMultiplier + Mathf.InverseLerp(0f,0.1f,playerVelocity) * playerAudioVolumeMultiplier);
	    
	    
        currentPlayerRotation +=  playerVelocity * playerrotationSpeed;
	    
	    
	    
	    // GPU Rotation on shader ( expensive??)
	    playerMaterial.SetFloat("_RotateSpeed",currentPlayerRotation);
	    playerMaterial.SetFloat("_EmmisionStrength",playerVelocity * playerEmsisionScalar);
	    //playerMaterial.SetVector("_RotateAxis",PlayerObject.transform.right);
	    
	    // CPU Rotation by Quaternion (not godd enought)
	    //PlayerObject.transform.rotation = Quaternion.AngleAxis(currentPlayerRotation, PlayerObject.transform.right);
        
        

        PlayerObject.transform.position = Vector3.SmoothDamp(PlayerObject.transform.position, targetPos + playerOffset, ref velocityPlayer , playerSmoothDampValue);
         
        currentPlayerPostion = PlayerObject.transform.position;
        
        
    }
    
	// Set player random position inside boundries
	void PlayerRandomPos()
    {
        
	    PlayerObject.transform.position = transform.position + new Vector3(Random.Range(-spawHoleBounds.x, spawHoleBounds.x), playerAboveGround, Random.Range(-spawHoleBounds.y, spawHoleBounds.y));
        velocityPlayer = Vector3.zero;
	    playerOffset = new Vector3(0, playerAboveGround, 0f);
	    
    }

     
	public void HoleRandomPos()
    {
        Hole.transform.SetPositionAndRotation(new Vector3(Random.Range(-spawHoleBounds.x, spawHoleBounds.x), 0.001f, Random.Range(-spawHoleBounds.y, spawHoleBounds.y)), Quaternion.LookRotation(-Vector3.up));
    }

	// Check distance between player and hole
	void CheckDistance()
    {
	    if (Vector3.Distance(PlayerObject.transform.position, Hole.transform.position) <= distanceFromPortalTreshhold && canPlayerMove)
        {
            levelUp += 1;
            distanceFromPortalTreshhold = 0;


            HoleAchived.Invoke();

        }
        

    }
    

	public void IncreaseDampPlayerValue(float amount)
	{
		playerSmoothDampValue += amount;
	}
	
	public void DecreaseDampPlayerValue(float amount)
	{
		playerSmoothDampValue -= amount;
	}
	
	public void SetDampPlayerValue(float amount)
	{
		playerSmoothDampValue = amount;
	}
	
	IEnumerator NextRoundStartDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		//isGameActive = true;
		PlayerRandomPos();
		HoleRandomPos();
		distanceFromPortalTreshhold = distTreshhold;
		BulletOne.SetActive(true);
		BulletTwo.SetActive(true);
		canPlayerMove = true;
		StartNewRound.Invoke();
	}

	// Start new round
	public void NextRound()
    {
	    Debug.Log("NestRound");
	    if (levelUp >= maxLevels)
        {
		    Debug.Log("FinishExp");
	        isGameActive = false;
		    SetPlayerAndHolePositionBeyondGame();
		    FinishExperience.Invoke();
		    //
		    
	        
        }
	    if(isGameActive)
	    {
	    	
	    	canPlayerMove = false;
	    	BulletOne.SetActive(false);
	    	BulletTwo.SetActive(false);
	    	SetPlayerAndHolePositionBeyondGame();
	    	StartCoroutine(NextRoundStartDelay(1.5f));
	    }
	    
        
    }
    
	public void SetPlayerAndHolePositionBeyondGame()
	{
		PlayerObject.transform.position = new Vector3(-10,-10,0);
		Hole.transform.position = new Vector3(-10,-10,0);
	}

	// Reset transform properties for all children 
    void ResteProperites()
    {

        transform.position = Vector3.zero;
        int childerCount = transform.childCount;

        GameObject[] childs = new GameObject[childerCount];

        for (int i = 0; i < childerCount; i++) { childs[i] = transform.GetChild(i).gameObject; childs[i].transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity); }

        levelUp = 0;
    }
    
}
