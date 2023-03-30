using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(menuName ="Guide/GuideSO")]
public class GuideSO : ScriptableObject
{
    public RenderTexture renderTexture;
    public VideoClip videoClip;
    public string description;
}
