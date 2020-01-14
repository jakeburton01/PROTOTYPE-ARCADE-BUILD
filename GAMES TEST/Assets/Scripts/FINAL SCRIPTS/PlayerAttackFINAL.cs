using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackFINAL : MonoBehaviour {

    private CharacterAnimation player_Anim;

    private bool activateTimerToReset;

    private float default_Combo_Timer = 1f;
    private float current_Combo_Timer;

    private ComboState current_Combo_State;


    public enum ComboState
    {
        NONE,
        LEFT_PUNCH,
        RIGHT_PUNCH,
        STRONG_PUNCH
    }

    private void Awake()
    {
        player_Anim = GetComponentInChildren<CharacterAnimation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        current_Combo_Timer = default_Combo_Timer;
        current_Combo_State = ComboState.NONE;
        current_Combo_State = (ComboState)1;
    }

    // Update is called once per frame
    void Update()
    {
        ComboAttacks();
        ResetComboState();
    }

    void ComboAttacks()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (current_Combo_State == ComboState.RIGHT_PUNCH ||
               current_Combo_State == ComboState.STRONG_PUNCH)
                return; //Returns to Left_Punch

            current_Combo_State++;
            activateTimerToReset = true;
            current_Combo_Timer = default_Combo_Timer;

            if(current_Combo_State == ComboState.LEFT_PUNCH)
            {
                player_Anim.Left_Punch();
            }

            if (current_Combo_State == ComboState.RIGHT_PUNCH)
            {
                player_Anim.Right_Punch();
            }

            
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (current_Combo_State == ComboState.STRONG_PUNCH ||
               current_Combo_State == ComboState.RIGHT_PUNCH)
                return;



            if(current_Combo_State == ComboState.NONE ||
               current_Combo_State == ComboState.LEFT_PUNCH ||
               current_Combo_State == ComboState.RIGHT_PUNCH)
            {
                current_Combo_State = ComboState.STRONG_PUNCH;
            } 
            
            activateTimerToReset = true;
            current_Combo_Timer = default_Combo_Timer;

            if(current_Combo_State == ComboState.STRONG_PUNCH)
            {
                player_Anim.Strong_Punch();
               
            }
        }
    } // Attack Combos


    void ResetComboState()
    {
        if (activateTimerToReset){
            current_Combo_Timer -= Time.deltaTime;

        if (current_Combo_Timer <= 0f)
            {
                current_Combo_State = ComboState.NONE;

                activateTimerToReset = false;
                current_Combo_Timer = default_Combo_Timer;
            }
        }
    }





}
