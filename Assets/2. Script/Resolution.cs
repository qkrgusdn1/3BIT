using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public TMP_Dropdown dropDown;

    private void Awake()
    {
        dropDown = GetComponent<TMP_Dropdown>();
        List<string> options = new List<string>();
        for(int i = 0; i < resolutionInfos.Length; i++)
        {
            options.Add(resolutionInfos[i].title);
        }
        dropDown.ClearOptions();
        dropDown.AddOptions(options);
    }

    private void Start()
    {
        dropDown.value = 3;
        Screen.SetResolution(resolutionInfos[dropDown.value].width, resolutionInfos[dropDown.value].height, true);
    }
    public ResolutionInfo[] resolutionInfos;

    public void OnClickedChange()
    {
        Screen.SetResolution(resolutionInfos[dropDown.value].width, resolutionInfos[dropDown.value].height, true);
    }
    private void Update()
    {
        
    }
}

[System.Serializable]
public class ResolutionInfo
{
    public string title;
    public int width;
    public int height;
}
