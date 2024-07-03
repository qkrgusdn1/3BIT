using UnityEngine;

public class LinkLineMinssion : Mission
{
    public LayerMask startPointMask;
    LinkStartPoint startPoint;
    LinkEndPoint endPoint;
    public LayerMask endPointMask;
    LinkStartPoint[] linkStartPoints;
    private void Awake()
    {
        linkStartPoints = GetComponentsInChildren<LinkStartPoint>();
    }
    public override void StartMission()
    {
        base.StartMission();
    }
    public override void EndMission()
    {
        base.EndMission();
    }

    
    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {

            Vector3 worldPoint = missionCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;
            //현재마우스가 클릭한 좌표
            Collider2D col = Physics2D.OverlapPoint(worldPoint, startPointMask);

            if (col != null)
            {
                startPoint = col.GetComponent<LinkStartPoint>();
                if (startPoint.isLink)
                {
                    startPoint = null;
                }
            }

        }
        else if (Input.GetMouseButton(0))
        {
            if (startPoint == null)
                return;
            Vector3 worldPoint = missionCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 2028;
            startPoint.lineRenderer.positionCount = 2;
            startPoint.lineRenderer.SetPosition(0, startPoint.transform.position);
            startPoint.lineRenderer.SetPosition(1, worldPoint);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (startPoint == null)
                return;
            startPoint.lineRenderer.positionCount = 0;
            Vector2 worldPoint = missionCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPoint, endPointMask);
            if (col != null)
            {
                endPoint = col.GetComponent<LinkEndPoint>();
                if(startPoint.linkColor == endPoint.linkColor)
                {
                    startPoint.lineRenderer.positionCount = 2;
                    startPoint.lineRenderer.SetPosition(0, startPoint.transform.position);
                    startPoint.lineRenderer.SetPosition(1, endPoint.transform.position);
                    startPoint.isLink = true;
                    CheckClear();
                }
            } 
            startPoint = null;

        }
    }

    public override void CheckClear()
    {
        for(int i = 0; i < linkStartPoints.Length; i++)
        {
            if (!linkStartPoints[i].isLink)
            {
                return;
            }
        }
        base.CheckClear();
    }
}
