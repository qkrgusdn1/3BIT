using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMission : Mission
{
    public int clickedCount;
    public List<GameObject> numberBtns = new List<GameObject>();
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = missionCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;

            Collider2D clickedCollider = Physics2D.OverlapPoint(worldPoint);

            GameObject firstActiveBtn = null;
            foreach (GameObject btn in numberBtns)
            {
                if (btn.activeSelf)
                {
                    firstActiveBtn = btn;
                    break;
                }
            }

            if (clickedCollider != null && clickedCollider.CompareTag("NumberBtn") && clickedCollider.gameObject == firstActiveBtn)
            {
                clickedCount++;
                clickedCollider.gameObject.SetActive(false);
                if (clickedCount == numberBtns.Count)
                {
                    CheckClear();
                }
            }
        }


    }
}
