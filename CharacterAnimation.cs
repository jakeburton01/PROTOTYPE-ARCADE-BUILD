using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk (bool move)
    {
        anim.SetBool(Tags.MOVEMENT, move);
    }

    public void Left_Punch()
    {
        anim.SetTrigger(Tags.LEFT_PUNCH_TRIGGER);
    }

    public void Right_Punch()
    {
        anim.SetTrigger(Tags.RIGHT_PUNCH_TRIGGER);
    }

    public void Strong_Punch()
    {
        anim.SetTrigger(Tags.STRONG_PUNCH_TRIGGER);
    }

    public void Dizzy_State()
    {
        anim.SetTrigger(Tags.DIZZY_ANIMATION);
    }

    public void Idle(bool idle)
    {
        anim.SetBool(Tags.IDLE, idle);
    }
}
