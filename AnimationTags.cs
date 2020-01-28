using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTags : MonoBehaviour
{
   


}

public class Tags
{
    public const string LEFT_PUNCH_TRIGGER = "LeftPunch";
    public const string RIGHT_PUNCH_TRIGGER = "RightPunch";
    public const string STRONG_PUNCH_TRIGGER = "StrongPunch";

    public const string MOVEMENT = "Movement";

    public const string HIT_ANIMATION = "Hit";
    public const string DIZZY_ANIMATION = "Dizzy";
    public const string IDLE_ANIMATION = "Idle";

    public const string LEFT_PUNCH_TAG = "LeftPunchTag";
    public const string RIGHT_PUNCH_TAG = "RightPunchTag";
    public const string STRONG_PUNCH_TAG = "StrongPunchTag";
    public const string UNTAGGED_TAG = "UnTag";

}

public class Axis
{
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string VERTICAL_AXIS = "Vertical";
}
