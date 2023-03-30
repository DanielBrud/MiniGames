using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class GuideHandler : MonoBehaviour
{
    [SerializeField] private GameFlowHandler gameFlowHandler;
    [SerializeField] private GameObject guideScreen;
    [SerializeField] private GuideListSO guideListSO;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI textMesh;
    private Material material;
    private Pose guideScreenInitialLocalTransform;
    public bool IsShowGuidActive;


    private void OnEnable()
    {
        material = guideScreen.GetComponent<MeshRenderer>().material;
        guideScreenInitialLocalTransform = new Pose(guideScreen.transform.localPosition, guideScreen.transform.localRotation);
    }
    // Set appropriate guide settings for current  mini game
    private void SetCurrentGiudeProperties()
    {
        int guideID = MagicBallsSpawner.gameID;
        videoPlayer.clip = guideListSO.guideSOs[guideID].videoClip;
        videoPlayer.targetTexture = guideListSO.guideSOs[guideID].renderTexture;
        material.SetTexture("_RenderTexture", guideListSO.guideSOs[guideID].renderTexture);
        textMesh.text = guideListSO.guideSOs[guideID].description;
        videoPlayer.Play();
    }


    // Show guide screen with description
    public void ShowGuide(bool state)
    {
        
        guideScreen.SetActive(state);
        if (state) 
        {
            gameFlowHandler.PauseGame(0.02f);
            SetCurrentGiudeProperties();
        }

    }
    // Show guide screen with description if enabel
    public void ShowGuideAfterColide()
    {
        if (IsShowGuidActive) 
        {
            ShowGuide(true);
            
        }
    }
    private void OnDisable()
    {
        videoPlayer.Stop();
    }

    public void SetShowGuideActive() 
    {
        IsShowGuidActive = !IsShowGuidActive;
    }

    public void SetGuideScreenInitialPose() 
    {
        guideScreen.transform.localPosition = guideScreenInitialLocalTransform.position;
        guideScreen.transform.localRotation = guideScreenInitialLocalTransform.rotation;
    }

    public void SetGuideScreenPoseForwardCamera(float distance) 
    {
        if (IsShowGuidActive) 
        {
            guideScreen.transform.position = gameFlowHandler.myCamera.transform.position + gameFlowHandler.myCamera.transform.forward * distance;
            guideScreen.transform.forward = gameFlowHandler.myCamera.transform.forward;
        }
            
    }

}
