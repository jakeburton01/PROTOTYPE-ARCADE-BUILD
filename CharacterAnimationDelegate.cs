﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationDelegate : MonoBehaviour {

    public GameObject left_Arm_Attack_Point, right_Arm_Attack_Point;


    void Left_Arm_Attack_On()
    {
        left_Arm_Attack_Point.SetActive(true);
    }

    void Left_Arm_Attack_Off()
    {
        if (left_Arm_Attack_Point.activeInHierarchy)
        {
            left_Arm_Attack_Point.SetActive(false);
        }
    }

    void Right_Arm_Attack_On()
    {
        right_Arm_Attack_Point.SetActive(true);
    }

    void Right_Arm_Attack_Off()
    {
        if (right_Arm_Attack_Point.activeInHierarchy)
        {
            right_Arm_Attack_Point.SetActive(false);
        }
    }

    void Left_Arm_Tag_On()
    {
        left_Arm_Attack_Point.tag = Tags.LEFT_PUNCH_TAG;
    }

    void Left_Arm_Tag_Off()
    {
        left_Arm_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }

    void Right_Arm_Tag_On()
    {
        right_Arm_Attack_Point.tag = Tags.RIGHT_PUNCH_TAG;
    }

    void Right_Arm_Tag_Off()
    {
        right_Arm_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
}
