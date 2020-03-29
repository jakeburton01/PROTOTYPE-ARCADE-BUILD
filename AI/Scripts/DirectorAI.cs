using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirectorAI : MonoBehaviour
{
    public GameObject Temp;
    public GameObject Fighter1;
    public HardAI FighterAI1;
    public float Fighter1HP;
    public int Fighter1HitsGiven;
    public int Fighter1HitsTaken;
    public float Fighter1Priority;


    public GameObject Fighter2;
    public HardAI FighterAI2;
    public float Fighter2HP;
    public int Fighter2HitsGiven;
    public int Fighter2HitsTaken;
    public float Fighter2Priority;


    public GameObject Fighter3;
    public HardAI FighterAI3;
    public float Fighter3HP;
    public int Fighter3HitsGiven;
    public int Fighter3HitsTaken;
    public float Fighter3Priority;


    public GameObject Fighter4;
    public HardAI FighterAI4;
    public float Fighter4HP;
    public int Fighter4HitsGiven;
    public int Fighter4HitsTaken;
    public float Fighter4Priority;

    public int PriorityTarget;


    public Healthblocks test1;
    public float test1hp;


    public float[] priorities;

    // Start is called before the first frame update
    void Start()
    {
        
        priorities = new float[5];
        priorities[0] = 1;

        Fighter1 = FindFighter(1);
        print("set1");
        Fighter2 = FindFighter(2);
        print("set2");
        Fighter3 = FindFighter(3);
        print("set3");
        Fighter4 = FindFighter(4);
        print("set4");
        test1 = GameObject.Find("AI1").GetComponent<Healthblocks>();

        Fighter1Priority = 0;
        Fighter1HP = 120;
        Fighter2HP = 120;
        Fighter3HP = 120;
        Fighter4HP = 120;
    }

    // Update is called once per frame
    void Update()
    {
        AICheck(FighterAI1, 1);
        AICheck(FighterAI2, 2);
        AICheck(FighterAI3, 3);
        AICheck(FighterAI4, 4);
        test1hp = test1.segments.Count();
        CheckPriority();
    }



    public void CheckPriority()
    {
        

        
        priorities[1] = Fighter1Priority;
        priorities[2] = Fighter2Priority;
        priorities[3] = Fighter3Priority;
        priorities[4] = Fighter4Priority;
        
        
        float maxValue = priorities.Max();
        int maxIndex = priorities.ToList().IndexOf(maxValue);

        PriorityTarget = maxIndex;

        priorities[0] = -200;



    }


    public void AICheck(HardAI check, int x)
    {
        if (x == 1)
        {
            if (check.Health != Fighter1HP)
            {
                if (check.Health > Fighter1HP)
                {
                    Fighter1HP = check.Health;
                    Fighter1Priority += 2;
                }
                else if (check.Health < Fighter1HP)
                {
                    Fighter1HP = check.Health;
                    Fighter1Priority -= 2;
                }
            }

            if (check.HitsGiven != Fighter1HitsGiven)
            {
                Fighter1HitsGiven = check.HitsGiven;
                Fighter1Priority += 1;
            }

            if (check.HitsTaken != Fighter1HitsTaken)
            {
                Fighter1HitsTaken = check.HitsTaken;
                Fighter1Priority -= 1;
            }
        }
        else if (x == 2)
        {
            if (check.Health != Fighter2HP)
            {
                if (check.Health > Fighter2HP)
                {
                    Fighter2HP = check.Health;
                    Fighter2Priority += 2;
                }
                else if (check.Health < Fighter2HP)
                {
                    Fighter2HP = check.Health;
                    Fighter2Priority -= 2;
                }
            }

            if (check.HitsGiven != Fighter2HitsGiven)
            {
                Fighter2HitsGiven = check.HitsGiven;
                Fighter2Priority += 1;
            }

            if (check.HitsTaken != Fighter2HitsTaken)
            {
                Fighter2HitsTaken = check.HitsTaken;
                Fighter2Priority -= 1;
            }
        }

       else if (x == 3)
        {
            if (check.Health != Fighter3HP)
            {
                if (check.Health > Fighter3HP)
                {
                    Fighter3HP = check.Health;
                    Fighter3Priority += 2;
                }
                else if (check.Health < Fighter3HP)
                {
                    Fighter3HP = check.Health;
                    Fighter3Priority -= 2;
                }
            }

            if (check.HitsGiven != Fighter3HitsGiven)
            {
                Fighter3HitsGiven = check.HitsGiven;
                Fighter3Priority += 1;
            }

            if (check.HitsTaken != Fighter3HitsTaken)
            {
                Fighter3HitsTaken = check.HitsTaken;
                Fighter3Priority -= 1;
            }
        }

       else if (x == 4)
        {
            if (check.Health != Fighter4HP)
            {
                if (check.Health > Fighter4HP)
                {
                    Fighter4HP = check.Health;
                    Fighter4Priority += 2;
                }
                else if (check.Health < Fighter4HP)
                {
                    Fighter4HP = check.Health;
                    Fighter4Priority -= 2;
                }
            }

            if (check.HitsGiven != Fighter4HitsGiven)
            {
                Fighter4HitsGiven = check.HitsGiven;
                Fighter4Priority += 1;
            }

            if (check.HitsTaken != Fighter4HitsTaken)
            {
                Fighter4HitsTaken = check.HitsTaken;
                Fighter4Priority -= 1;
            }
        }

    }

    public GameObject FindFighter(int x)
    {
        print("set start");
        GameObject returnobject;
        returnobject = null;
        try
        {
            returnobject = GameObject.Find("AI" + x);
            if(x == 1)
            {
                FighterAI1 = returnobject.GetComponent<HardAI>();
            }
            else if(x == 2)
            {
                FighterAI2 = returnobject.GetComponent<HardAI>();
            }
            else if (x == 3)
            {
                FighterAI3 = returnobject.GetComponent<HardAI>();
            }
            else if (x == 4)
            {
                FighterAI4 = returnobject.GetComponent<HardAI>();
            }
        }
        catch
        {
            returnobject = null;
        }
        
        if(returnobject == null)
        {
            try
            {
                returnobject = GameObject.Find("Player" + x);
            }
            catch
            {
                returnobject = null;
            }
            
        }
        


        return returnobject;
    }
}
