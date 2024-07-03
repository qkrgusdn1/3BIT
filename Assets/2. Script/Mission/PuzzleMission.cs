using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMission : Mission
{
    public override void StartMission()
    {
        base.StartMission();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void EndMission()
    {
        base.EndMission();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
