using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public float loadingSpeed;
    public Image loadingBar;

    void Start()
    {
        loadingBar.fillAmount = 0;
        StartCoroutine(CoLoding());
    }
    IEnumerator CoLoding()
    {
        while(loadingBar.fillAmount < 1)
        {
            loadingBar.fillAmount += loadingSpeed * Time.deltaTime;
            yield return null;
        }
        
        SceneManager.LoadScene("SampleScene");
        SoundMgr.Instance.lobbyMusic.gameObject.SetActive(true);
    }
}
