using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text numberText;
    Vector3 currentPosition;
    public Image image;
    public int number;

    public Vector2Int index;

    
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void Number(int number)
    {
        this.number = number;

        numberText.text = number.ToString();
        if(number == 0)
        {
            image.enabled = false;
            numberText.enabled = false;
        }
        else
        {
            image.enabled = true;
            numberText.enabled = true;
        }
    }

    public void ChangeTile()
    {
        currentPosition = GetComponent<RectTransform>().localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void Move()
    {

    }

   

}
