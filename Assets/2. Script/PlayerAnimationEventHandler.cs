using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public Action startAttackListener;
    public Action endAttackListener;
    public Action finishAttackListener;

    public void StartAttack()
    {
        startAttackListener?.Invoke();
    }
    public void EndAttack()
    {
        endAttackListener?.Invoke();
    }
    public void FinishAttack()
    {
        finishAttackListener?.Invoke();
    }
}
