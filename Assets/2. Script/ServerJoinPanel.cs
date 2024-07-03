using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ServerJoinPanel : MonoBehaviour
{
    public TMP_Text loadingText;

    public string originalText;

    void OnEnable()
    {
        StartCoroutine(CoLoadingText());
    }

    

    IEnumerator CoLoadingText()
    {
        while (true)
        {
            loadingText.text = originalText + ".";
            yield return new WaitForSeconds(0.5f);

            loadingText.text = originalText + "..";
            yield return new WaitForSeconds(0.5f);

            loadingText.text = originalText + "...";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
