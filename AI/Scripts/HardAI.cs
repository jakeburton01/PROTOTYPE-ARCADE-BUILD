using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class HardAI : MonoBehaviour
{

    Transform Target;   //Used in search algorithm to find position of target

    GameObject TargetObj;  //Finds the game object of the target

    public GameObject Find;  //Not Currently Used

    public int Type; //Decides what type of search algorithm is used   (Type 1 = Closest, Type 2 = random,  Type 3 = lowest HP [not implemented])

    private enum States { Idle, Search, Move, RunAway, Hit }         //Should represent the drawn state chart      (Dizzy/Damaged not currently implemented)

    [SerializeField]
    private States CurrentState; //To dictate which state the script should move to

    public int hitRandom = 0; //Random number generated to decide between normal attack and charge attack
    public float Distance = 0; //Distance between the AI unit and its target
    public bool isMoving = false; //Bool to find whether its moving or not

    private Vector3 moveDirection; //Used to dictate what direction the knockback throws units in Hit()
    public float Health = 120; //Health value of the unit
    public float previousHealth; //Used in the HealthBar function ###Not actively being used in current iteration###

    public Renderer m_renderer; //The renderer for  this specific unit

    public float wanderRadius; //Radius of the sphere that the AI can chose a destination within
    public float wanderTimer; //How long the AI can wander in the radius
    public float timer; //General timer used for wandering


    public bool dizzy; //Bool for if the Unit is considered dizzy
    public float dizzyTimer; //Timer to dictate how long the AI is dizzy for
    public int dizzyCheck;

    public bool hasHit; //Bool for after a unit has hit to stop it hitting again
    public float hitCooldownTimer; //The cooldown before the unit can hit again
    public float hitTimer; //Charge hit timer HALF IMPLEMENTED #####

    public bool wayPoint; //Bool for if a waypoint is currently active in the scene for this instance
    public GameObject wayPointGO; //GameObject for the waypoint related to this AI
    public float wayDistance; //Distance between the AI unit and the waypoint gameObject
    public int AINumber; //Designates what player number this AI is (defined in editor)

    public NavMeshAgent enemy_AI;  //Loads in the navmesh component of this unit

    public GameObject wayPrefab; //Prefab for the waypoint block

    public float hpSearchcheck; //Used for a set value within the lowest HP search function

    public float waypointToEnemyDistance; //Distance between current target and current waypoint, used in runAway (unsure if actually used due to it being old math I was testing)


    public bool HPBarSet; //If hit this is set to stop it from calling a hit every frame
    public int HPBarHitValue; //No idea

    public Healthblocks HPUIScript; //This finds the script for the healthbar that will be attached to this unit
    private GameObject HPUIReference; //Finds the actual health bar object connected to this AI
    private CharacterAnimation player_Anim;   //Finds the component inside the AI prefab for animation

    public bool aistopped;

    public float gametimer;
    public float randomint;

    public int HitsGiven;
    public int HitsTaken;

    public DirectorAI Director;

    // Start is called before the first frame update
    void Start()
    {
        Director = GameObject.Find("GameManager").GetComponent<DirectorAI>();
        gametimer = 0;
        enemy_AI = GetComponent<NavMeshAgent>();
        CurrentState = States.Search;
        previousHealth = Health;
        m_renderer = this.GetComponent<Renderer>();
        aistopped = enemy_AI.isStopped;
        hasHit = false;
        hitCooldownTimer = 1;
        hitTimer = 0;
        wayPoint = false;
        hpSearchcheck = 300;

        HPUIScript = this.gameObject.GetComponentInParent<Healthblocks>();
        HPBarSet = false;
        HPBarHitValue = 0;
        player_Anim = GetComponent<CharacterAnimation>();

        HitsGiven = 0;
        HitsTaken = 0;
        /* if (AINumber == 1) //AI number defined in inspector, but will be defined through instantiation later.
         {
             HPUIReference = GameObject.Find("AI1Hold");
             HPUIScript = HPUIReference.GetComponent<Healthblocks>();
         }
         if (AINumber == 2)
         {
             HPUIReference = GameObject.Find("AI2Hold");
             HPUIScript = HPUIReference.GetComponent<Healthblocks>();
         }
         if (AINumber == 3)
         {
             HPUIReference = GameObject.Find("AI3Hold");
             HPUIScript = HPUIReference.GetComponent<Healthblocks>();
         }
         if (AINumber == 4)
         {
             HPUIReference = GameObject.Find("AI4Hold");
             HPUIScript = HPUIReference.GetComponent<Healthblocks>();
         }*/
    }

    // Update is called once per frame
    void Update()
    {

        gametimer += Time.deltaTime;
        if (enemy_AI.enabled == false)
        {

            timer += Time.deltaTime;
            if (timer >= 2)
            {
                enemy_AI.enabled = true;
                timer = 0;
            } //If the NavMeshAgent is turned off (generally from being hit) it spends 2 seconds before it turns itself back on   (Old part of dizzy function)
        }

        else if (enemy_AI.enabled == true)
        {

            if (HPUIScript.AIRegen == true)
            {
                Health = Health + 10; //If the connected hp script variable is set to true then regen 10 points
                HPUIScript.AIRegen = false; //Resets the variable
            }

            if (enemy_AI.isStopped == false)
            {
                player_Anim.Walk(true); //If the NavMeshAgent is moving then set the animation to walking
            }
            if (enemy_AI.isStopped)
            {
                player_Anim.Walk(false); //If the NavMeshAgent isn't moving, then stop the walking animation (likely leading to either a hit or idle animation)
            }

            if (hasHit == true)
            {
                hitCooldownTimer -= Time.deltaTime; //If the unit has hit the enemy a timer will count down to 0, when at 0 it can hit again.  Functions as a hit cooldown.
            }

            if (hasHit == true && hitCooldownTimer <= 0)
            {
                hasHit = false;
                hitCooldownTimer = 1;
            }

            if (Health <= 10 && Health > 0)
            {
                CurrentState = States.RunAway;
                RunAway();
            }

            else if (Health <= 0)
            {
                Destroy(this.gameObject);
                // enemy_AI.enabled = false;
                // this.gameObject.tag = "Untagged";

            }


            if (Target != null)
            {
                print("Distance is being measured");
                Distance = Vector3.Distance(transform.position, Target.position);
            } //If its found a target this sets the Distance to the target each frame
            else if (Target == null)
            {
                Distance = 0;
                print("Resetting to idle");
                CurrentState = States.Idle;

            }



            switch (CurrentState)//Overall start of the state machine
            {
                case States.Idle:
                    {
                        IdleWander();
                        timer += Time.deltaTime;
                        break;
                    }
                case States.Search:
                    {
                        timer = 0;
                        Search();

                        break;
                    }
                case States.Move:
                    {
                        print("Switch to move");
                        Move();
                        break;
                    }
                case States.Hit:
                    {
                        hitCooldownTimer -= Time.deltaTime;
                        int randomhit;
                        randomhit = Random.Range(1, 5);
                        Hit(randomhit);
                        break;
                    }

                case States.RunAway:
                    {
                        RunAway();
                        break;
                    }

            }
        }


    }


    public void IdleWander()
    {
        enemy_AI.isStopped = false;

        RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);
        if (AINumber == 1)
        {

            try //Try catch used to stop having null reference errors
            {
                wayPointGO = GameObject.Find("Waypoint_AI1");
                wayDistance = Vector3.Distance(transform.position, wayPointGO.transform.position);
            }
            catch
            {
                print("NO WAYPOINT FOUND");
                IdleWander();
            }
        }
        else if (AINumber == 2)
        {
            try
            {
                wayPointGO = GameObject.Find("Waypoint_AI2");
                wayDistance = Vector3.Distance(transform.position, wayPointGO.transform.position);
            }
            catch
            {
                print("NO WAYPOINT FOUND");
                IdleWander();
            }
        }
        else if (AINumber == 3)
        {
            try
            {
                wayPointGO = GameObject.Find("Waypoint_AI3");
                wayDistance = Vector3.Distance(transform.position, wayPointGO.transform.position);
            }
            catch
            {
                print("NO WAYPOINT FOUND");
                IdleWander();
            }
        }
        else if (AINumber == 4)
        {
            try
            {
                wayPointGO = GameObject.Find("Waypoint_AI4");
                wayDistance = Vector3.Distance(transform.position, wayPointGO.transform.position);
            }
            catch
            {
                print("NO WAYPOINT FOUND");
                IdleWander();
            }
        }
        if (timer <= wanderTimer && wayPointGO != null)
        {
            print("Setting Path");
            enemy_AI.SetDestination(wayPointGO.transform.position); //Sets destination towards the spawned waypoint
            NavMeshPath path = new NavMeshPath(); //Creates a temporary path in memory that can be queried
                                                  //print("Path Found");
            enemy_AI.CalculatePath(wayPointGO.transform.position, path); //This calculates whether the path the AI has created is Valid, Invalid or Partial
            if (path.status == NavMeshPathStatus.PathInvalid) //If the AI can't complete the path its created (Off navmesh/blocked)
            {

                Destroy(wayPointGO);//Destroys the currently unreachable waypoint 
                                    //print("invalid path");
                wayPoint = false;//States that no waypoint is active so another is to be spawned    [Inside RandomNavSphereWaypoint function]
            }
            else if (wayDistance <= enemy_AI.stoppingDistance)
            {
                Destroy(wayPointGO);
                // print("Path concluded");
                wayPoint = false;  //If the AI unit reaches the waypoint, destroy it and create another
            }
            //Vector3 newPos = RandomNavSphere(this.transform.position, wanderRadius, -1);

            //enemy_AI.SetDestination();

        }

        if (timer >= wanderTimer)
        {
            if (CurrentState == States.Idle)
            {
                print("Needs to get here");
                Search();
                CurrentState = States.Search;//Once the timer reaches its max allowed value, move into search
            }
        }

    }

    public void Search()
    {
        print("into Search");
        Destroy(wayPointGO);
        wayPoint = false;

        if (Director.PriorityTarget == 0|| Director.PriorityTarget == 5)
        {
            if (gametimer < 40)
            {
                randomint = 1;
            }
            if (gametimer >= 40 && gametimer < 120)
            {
                randomint = Random.Range(0, 6);
            }
            if (gametimer >= 120 && gametimer < 240)
            {
                randomint = Random.Range(0, 10);
            }

            if (randomint < 5)
            {
                Target = FindRandomEnemy().transform;
                TargetObj = FindRandomEnemy();
            }
            if (randomint > 5)
            {
                if (FindPowerup() != null)
                {
                    Target = FindPowerup().transform;
                    TargetObj = FindPowerup();
                }
                else
                {
                    Target = FindClosestEnemy().transform;
                    TargetObj = FindClosestEnemy();
                }

            }
        }

       else if(Director.PriorityTarget == 1 && this.gameObject.name != "AI1")
        {
            Target = FindPriority(1).transform;
            TargetObj = FindPriority(1);
        }

       else if (Director.PriorityTarget == 2 && this.gameObject.name != "AI2")
        {
            Target = FindPriority(2).transform;
            TargetObj = FindPriority(2);
        }

      else  if (Director.PriorityTarget == 3 && this.gameObject.name != "AI3")
        {
            Target = FindPriority(3).transform;
            TargetObj = FindPriority(3);
        }
        else if (Director.PriorityTarget == 4 && this.gameObject.name != "AI4")
        {
            Target = FindPriority(4).transform;
            TargetObj = FindPriority(4);
        }

        //Finds the game object and the transform location of the nearest enemy   [Custom functions]
        if (Target != null)
        {
            print("Target != null");
            CurrentState = States.Move; //If a target is found then switch into Move state
        }

        else if (Target == null)
        {
            print("No Enemy Found");
            CurrentState = States.Idle;
        }
    }

    public void Move()
    {
        print("Tableflip");
        if (Distance > enemy_AI.stoppingDistance)   //If the target is at least a certain distance away, move towards it
        {
            if (Distance > 2 * enemy_AI.stoppingDistance)
            {
                enemy_AI.speed = 2; //If the enemy is far away then move faster
            }
            else
            {
                enemy_AI.speed = 1; //If the enemy is close then move slower
            }

            enemy_AI.isStopped = false;    //Sets the navmesh bool 
            //this.gameObject.transform.LookAt(Target.transform);
            enemy_AI.SetDestination(Target.position); //Sets the navmesh destination
        }

        //if (Distance < 2)
        // {
        //    CurrentState = States.Hit;
        //}
        else
        {
            enemy_AI.isStopped = true; //If the target is within a certain distance to the AI Unit then the navmesh agent should stop moving
            CurrentState = States.Hit; //Once in range switch into the hit state
        }
    }

    public void Hit(int rand)
    {
        try
        {
            Rigidbody hitRB = TargetObj.GetComponent<Rigidbody>();  //Finds the rigid body attatched to the target, used for knockback
            if (hasHit == false) //If the AI hasn't hit recently
            {
                if (rand == 0)
                {
                    this.gameObject.transform.LookAt(hitRB.transform);
                    hitRandom = Random.Range(0, 20); //Creates a random number between 0 and 20
                    if (TargetObj.tag == "Enemy") //Checks whether its an AI or Player fighter
                    {
                        HardAI hitTarget = TargetObj.GetComponent<HardAI>();
                        Healthblocks hitHPBarScript = TargetObj.GetComponent<Healthblocks>();
                        if (hitRandom >= 0 && hitRandom <= 21) //If the random number is between 0 and 10, do a normal attack
                        {
                            HitsGiven += 1;
                            hitTarget.HitsTaken +=1;
                            player_Anim.Right_Punch(); //Plays a punch animation
                            hitHPBarScript.NormalDamage(); //Calls the damage script in the enemy's hp script       !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!CURRENTLY REFERENCING ITS OWN SCRIPT!!!!!!!!!!!!!!!!!!!!!!!!
                                                           //moveDirection = TargetObj.transform.position - this.transform.position; //Direction away from the attacker, backwards for the target
                                                           //hitRB.AddForce(moveDirection.normalized * +4000f); //Adds a force to the target rigid body, using the above defined direction
                            hitTarget.previousHealth = Health;
                            hitTarget.Health -= 10;  //Current target health is reduced by 10 on a normal hit
                            hasHit = true; //This AI is now considered to have hit and will have to wait for the hit cooldown
                            hitTarget.HPBarSet = true;
                            hitTarget.HPBarHitValue = 10; //Depreciated
                            print("normalhitright");            //hitTarget.enemy_AI.enabled = false;

                        }


                    }

                    if (TargetObj.tag == "Player")
                    {
                        Healthblocks hitHPScript = TargetObj.GetComponent<Healthblocks>();
                        player_Anim.Right_Punch();
                        hitHPScript.NormalDamage();
                        hasHit = true;
                    }
                }

                if (rand == 1)
                {
                    this.gameObject.transform.LookAt(hitRB.transform);
                    hitRandom = Random.Range(0, 20); //Creates a random number between 0 and 20
                    if (TargetObj.tag == "Enemy") //Checks whether its an AI or Player fighter
                    {
                        HardAI hitTarget = TargetObj.GetComponent<HardAI>();
                        Healthblocks hitHPBarScript = TargetObj.GetComponent<Healthblocks>();
                        if (hitRandom >= 0 && hitRandom <= 21) //If the random number is between 0 and 10, do a normal attack
                        {
                            HitsGiven += 1;
                            hitTarget.HitsTaken += 1;
                            player_Anim.Left_Punch(); //Plays a punch animation
                            hitHPBarScript.NormalDamage(); //Calls the damage script in the enemy's hp script       !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!CURRENTLY REFERENCING ITS OWN SCRIPT!!!!!!!!!!!!!!!!!!!!!!!!
                                                           //moveDirection = TargetObj.transform.position - this.transform.position; //Direction away from the attacker, backwards for the target
                                                           //hitRB.AddForce(moveDirection.normalized * +4000f); //Adds a force to the target rigid body, using the above defined direction
                                                           //hitTarget.previousHealth = Health;
                            hitTarget.Health -= 10;  //Current target health is reduced by 10 on a normal hit
                            hasHit = true; //This AI is now considered to have hit and will have to wait for the hit cooldown
                            hitTarget.HPBarSet = true;
                            // hitTarget.HPBarHitValue = 10; //Depreciated
                            print("normal hit");                      //hitTarget.enemy_AI.enabled = false;

                        }


                    }

                    if (TargetObj.tag == "Player")
                    {
                        Healthblocks hitHPScript = TargetObj.GetComponent<Healthblocks>();
                        player_Anim.Left_Punch();
                        hitHPScript.NormalDamage();
                        hasHit = true;
                    }
                }

                if (rand == 2)
                {
                    this.gameObject.transform.LookAt(hitRB.transform);
                    hitRandom = Random.Range(0, 20); //Creates a random number between 0 and 20
                    if (TargetObj.tag == "Enemy") //Checks whether its an AI or Player fighter
                    {
                        HardAI hitTarget = TargetObj.GetComponent<HardAI>();
                        Healthblocks hitHPBarScript = TargetObj.GetComponent<Healthblocks>();
                        if (hitRandom >= 0 && hitRandom <= 21) //If the random number is between 0 and 10, do a normal attack
                        {
                            HitsGiven += 1;
                            hitTarget.HitsTaken += 1;
                            print("strong hit");
                            player_Anim.Strong_Punch(); //Plays a punch animation
                            hitHPBarScript.chargedTimer = 3;
                            hitHPBarScript.ChargedDamage(); //Calls the damage script in the enemy's hp script       !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!CURRENTLY REFERENCING ITS OWN SCRIPT!!!!!!!!!!!!!!!!!!!!!!!!
                                                            //moveDirection = TargetObj.transform.position - this.transform.position; //Direction away from the attacker, backwards for the target
                                                            //hitRB.AddForce(moveDirection.normalized * +4000f); //Adds a force to the target rigid body, using the above defined direction
                                                            //hitTarget.previousHealth = hitTarget.Health;
                            hitTarget.Health -= 30;  //Current target health is reduced by 10 on a normal hit
                            hasHit = true; //This AI is now considered to have hit and will have to wait for the hit cooldown
                            hitTarget.HPBarSet = true;
                            hitTarget.HPBarHitValue = 10; //Depreciated
                                                          //hitTarget.enemy_AI.enabled = false;

                        }


                    }

                    if (TargetObj.tag == "Player")
                    {
                        Healthblocks hitHPScript = TargetObj.GetComponent<Healthblocks>();
                        player_Anim.Strong_Punch();
                        hitHPScript.NormalDamage();
                        hasHit = true;
                    }
                }

                if (rand == 3)
                {
                    print("miss");
                    player_Anim.Right_Punch();
                    hasHit = true;
                }

                if (rand == 4)
                {
                    print("miss");
                    player_Anim.Left_Punch();
                    hasHit = true;
                }

                if (rand == 5)
                {
                    print("miss");
                    player_Anim.Strong_Punch();
                    hasHit = true;
                }


            }


            else
            {

                CurrentState = States.Idle;
                IdleWander();
                timer = 0; //If hasHit is true then switch back into the idle state
            }
        }
        catch
        {
            CurrentState = States.Idle;
            IdleWander();
            timer = 0;
        }


    }

    public void RunAway()
    {
        Target = FindClosestEnemy().transform;
        TargetObj = FindClosestEnemy(); //Finds the closest tagged enemy unit
        HardAI hitTarget = TargetObj.GetComponent<HardAI>();
        if (Target != null) //If an enemy is found in the scene then continue
        {
            if (hitTarget.Health <= Health)
            {
                CurrentState = States.Move;
                print("Chasing");
            }

            if (hitTarget.Health > Health)
            {
                print("Running");
                RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);


                //print("Waypointed");
                if (AINumber == 1)
                {
                    try
                    {
                        wayPointGO = GameObject.Find("Waypoint_AI1");
                        // print("Found run way");
                        //print(wayPointGO.name);
                    }
                    catch
                    {
                        print("NO RUN WAYPOINT FOUND");
                    }
                }
                else if (AINumber == 2)
                {
                    try
                    {
                        wayPointGO = GameObject.Find("Waypoint_AI2");
                    }
                    catch
                    {
                        print("NO WAYPOINT FOUND");
                    }
                }
                else if (AINumber == 3)
                {
                    try
                    {
                        wayPointGO = GameObject.Find("Waypoint_AI3");
                    }
                    catch
                    {
                        print("NO WAYPOINT FOUND");
                    }
                }
                else if (AINumber == 4)
                {
                    try
                    {
                        wayPointGO = GameObject.Find("Waypoint_AI4");
                    }
                    catch
                    {
                        print("NO WAYPOINT FOUND");
                    }
                }


                if (wayPointGO != null)
                {
                    Waypointer way = wayPointGO.GetComponent<Waypointer>();
                    BoxCollider waybox = wayPointGO.GetComponent<BoxCollider>();
                    waybox.enabled = true;
                    //print("Collider found");

                    if (way.Reset == true)
                    {
                        Destroy(wayPointGO);
                        wayPoint = false;
                        RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);
                        // print("Col");


                    }
                    else if (way.Reset == false)
                    {
                        // print("WTFGUYS");

                        enemy_AI.isStopped = false;
                        enemy_AI.speed = 5;
                        NavMeshPath path = new NavMeshPath();
                        enemy_AI.CalculatePath(wayPointGO.transform.position, path); //This calculates whether the path the AI has created is Valid, Invalid or Partial
                        //print("LMAO");
                        if (path.status == NavMeshPathStatus.PathInvalid) //If the AI can't complete the path its created (Off navmesh/blocked)
                        {
                            // print("Hellotehre;");
                            Destroy(wayPointGO);//Destroys the currently unreachable waypoint 
                            wayPoint = false;
                        }
                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            enemy_AI.SetDestination(wayPointGO.transform.position); //Sets destination towards the spawned waypoint
                        }
                    }



                }

            }

        }
    }






    public void InitializeVariables(int x, int y, int a, int b, string name)
    {
        AINumber = x;
        Type = y;
        wanderRadius = a;
        wanderTimer = b;
        this.gameObject.name = name;
    }


    public void RandomNavSphereWaypoint(Vector3 origin, float dist, int layermask)     //Used in creating a randomly positioned Waypoint around the AI units
    {
        Vector3 randDirection = this.gameObject.transform.position + Random.insideUnitSphere * dist; // Origin + (Random point * Distance allowed)    Takes the AI unit as an origin point for the circle, then finds a random point within a set distance and sets it to the vector3
        if (randDirection.y < 0 || randDirection.y >= 3)   //If the random point is below or above the arena floor then create a new point
        {
            randDirection = this.gameObject.transform.position + Random.insideUnitSphere * dist;
        }
        else if (randDirection.y >= 0.5 || randDirection.y <= 1) //If within allowed bounds then continue
        {
            if (wayPoint == false) //If no linked waypoint is active on the scene
            {

                wayPointGO = Instantiate(wayPrefab, randDirection, Quaternion.identity); //Spawns in a Waypoint into the scene, using the editor defined prefab
                if (AINumber == 1)
                {
                    wayPointGO.name = "Waypoint_AI1";
                }
                else if (AINumber == 2)
                {
                    wayPointGO.name = "Waypoint_AI2";
                }
                else if (AINumber == 3)
                {
                    wayPointGO.name = "Waypoint_AI3";
                }
                else if (AINumber == 4)
                {
                    wayPointGO.name = "Waypoint_AI4";
                } //Names the Waypoint in the scene appropriately
                wayPoint = true; //A linked waypoint is now active
            }

            if (wayPoint == true)
            {
                CurrentState = States.Idle; //If a waypoint has been created, move back up to the idle state
            }
        }

    }





    public GameObject FindPowerup()
    {
        GameObject[] PowerupGo;

        PowerupGo = GameObject.FindGameObjectsWithTag("Powerup");
        if (PowerupGo[1] != null)
        {
            return PowerupGo[1];
        }
        else
        {
            return null;
        }

    }


    public GameObject FindClosestEnemy()
    {
        GameObject[] AIGo;
        GameObject[] PlayerGO;
        GameObject[] gos;//Creates an empty array for game objects to be placed into 
        AIGo = GameObject.FindGameObjectsWithTag("Enemy"); //Fills the array with gameobjects that are tagged as "Enemy"
        PlayerGO = GameObject.FindGameObjectsWithTag("Player");
        gos = AIGo.Concat(PlayerGO).ToArray(); //Joins two GameObject arrays to one single array
        GameObject closest = null; //Initialises the return game object
        float distance = Mathf.Infinity; //Initialises a temporary float to measure distance
        Vector3 position = transform.position; //Initialises a temporary vector 3, starts as the vector 3 of this AI unit
        foreach (GameObject go in gos) //Runs the code through each individual element in the array
        {

            Vector3 diff = go.transform.position - position; //Initialises a new vector 3 for the difference in positions, distance measurement
            float curDistance = diff.sqrMagnitude; //Sets the vector 3 value back into a float value
            if (curDistance < distance && go.transform.position.y < 500 && go != this.gameObject) //If it is closer and not out of the arena range and is not itself, then continue
            {
                closest = go;
                distance = curDistance; //Sets the game object to the return variable and sets the new distance to beat
            }

        }

        return closest;
    }


    public GameObject FindRandomEnemy()
    {
        List<GameObject> randomList = new List<GameObject>(); //Creates a new list that fits in gameobjects
        GameObject[] re; //Creates an Array that allows for gameobjects
        GameObject[] AIGo;
        GameObject[] PlayerGO;

        AIGo = GameObject.FindGameObjectsWithTag("Enemy"); //Fills the array with gameobjects that are tagged as "Enemy"
        PlayerGO = GameObject.FindGameObjectsWithTag("Player");

        re = AIGo.Concat(PlayerGO).ToArray();//Joins two GameObject arrays to one single array
        GameObject randomEnemy = null; //Initialises an empty game object variable

        foreach (GameObject go in re) //Fill the array with all gameobjects tagged as "enemy", then move through each in this array
        {
            if (go.Equals(this.gameObject)) //If the game object is the one linked to this instance of code, then don't add it to the list
                continue;
            randomList.Add(go); //If its not the linked gameobject then add to the list
        }

        re = randomList.ToArray(); //I'm confused why i've taken multiple arrays and lists but whatever i guess it works
        int seed = Random.Range(0, re.Length); //Finds a random number between 0 and the lenght of the array
        randomEnemy = re[seed]; //Sets the randomEnemy game object to the randomly chosen array element

        randomList.Clear(); //Empties the list 

        return randomEnemy;


    }



    public GameObject FindPriority(int x)
    {
        GameObject returnobject;
        try
        {
            returnobject = GameObject.Find("AI" + x);
        }
        catch
        {
            returnobject = GameObject.Find("Player" + x);
        }
        

        return returnobject;
    }


    /* public void OnTriggerEnter(Collider other)
     {
         if(other.gameObject.tag == "SpeedPowerup")
         {
             HPUIScript.SpeedPickup();
             HPUIScript.pwrups.SpawnAfterPickup();
         }
         else if(other.gameObject.tag == "HealthPickup")
         {
             HPUIScript.HealthPickup();
             Health = Health + 10;
             HPUIScript.pwrups.SpawnAfterPickup();
         }
     }
     */


}
