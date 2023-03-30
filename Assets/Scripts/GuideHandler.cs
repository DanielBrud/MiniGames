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
    
    public bool IsShowGuidActive;


    private void OnEnable()
    {
        material = guideScreen.GetComponent<MeshRenderer>().material;
        
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
            gameFlowHandler.PauseGame(0);
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


}
