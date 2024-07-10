using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    public List<GameObject> helpList = new List<GameObject>();
    int currentIndex;

    private void OnEnable()
    {
        helpList[0].SetActive(true);
        for (int i = 1; i < helpList.Count; i++)
        {
            helpList[i].SetActive(false);
        }
        currentIndex = 0;
        UpdateHelpList();
    }
    public void OnClickedBackBtn()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateHelpList();
        }
    }

    public void OnClickedNextBtn()
    {
        if (currentIndex < helpList.Count - 1)
        {
            currentIndex++;
            UpdateHelpList();
        }
    }

    void UpdateHelpList()
    {
        for (int i = 0; i < helpList.Count; i++)
        {
            helpList[i].SetActive(i == currentIndex);
        }
    }
}
