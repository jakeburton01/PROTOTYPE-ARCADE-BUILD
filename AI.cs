using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;


public class AI : MonoBehaviour
{
    [SerializeField]
    Transform Target;   //Used in search algorithm to find position of target

    GameObject TargetObj;  //Finds the game object of the target

    public GameObject Find;  //Not Currently Used

    public int Type; //Decides what type of search algorithm is used   (Type 1 = Closest, Type 2 = random,  Type 3 = lowest HP [not implemented])

    private enum States { Idle, Search, Move, RunAway, Hit, Damaged, Dizzy }         //Should represent the drawn state chart      (Dizzy/Damaged not currently implemented)

    [SerializeField]
    private States CurrentState; //To dictate which state the script should move to

    public int hitRandom = 0; //Random number generated to decide between normal attack and charge attack
    public float Distance = 0; //Distance between the AI unit and its target
    public bool isMoving = false; //Bool to find whether its moving or not
    public float moveTimer = 0; //NOT IMPLEMENTED   #########
    private Vector3 moveDirection; //Used to dictate what direction the knockback throws units in Hit()
    public float Health = 120; //Health value of the unit
    public float previousHealth; //Used in the HealthBar function ###Not actively being used in current iteration###
    public float thrust = 20; //NOT IMPLEMENTED    ##########

    /*public Material red;
    public Material blue;
    public Material yellow;
    public Material current; //The current Material for shifting between a charge attack material and the HP material*/
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

    NavMeshAgent enemy_AI;  //Loads in the navmesh component of this unit

    public GameObject wayPrefab; //Prefab for the waypoint block

    public float hpSearchcheck; //Used for a set value within the lowest HP search function

    public float waypointToEnemyDistance; //Distance between current target and current waypoint, used in runAway (unsure if actually used due to it being old math I was testing)


    public bool HPBarSet; //If hit this is set to stop it from calling a hit every frame
	public int HPBarHitValue; //No idea

    public Healthblocks HPUIScript; //This finds the script for the healthbar that will be attached to this unit
    private GameObject HPUIReference; //Finds the actual health bar object connected to this AI
    private CharacterAnimation player_Anim;   //Finds the component inside the AI prefab for animation

    void Start()
    {
        //Loading in all the basic components and declaring variables

        enemy_AI = GetComponent<NavMeshAgent>();
        CurrentState = States.Search;
        previousHealth = Health;
        m_renderer = this.GetComponent<Renderer>();
        dizzy = false;
		dizzyCheck = 0;
        dizzyTimer = 0;
        hasHit = false;
        hitCooldownTimer = 1;
        hitTimer = 0;
        wayPoint = false;
        hpSearchcheck = 300;
        HPBarSet = false;
		HPBarHitValue = 0;
        player_Anim = GetComponent<CharacterAnimation>();
        if (AINumber == 1) //AI number defined in inspector, but will be defined through instantiation later.
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
        }
    }

    // Update is called once per frame
    void Update()    //Probably need to move things out of update cause its a cluttered mess of constant checking
    {
        /*GameObject[] finder;
        finder = GameObject.FindGameObjectsWithTag("Enemy");
        Find = finder[0];*/             //Literally no idea what this is

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
            if(HPUIScript.AIRegen == true)
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


            if (Input.GetKeyDown("q"))
            {
                Health -= 10;
                HPBarSet = true;//DEBUG, need to remove probably
            }


            if (hasHit == true && hitCooldownTimer <= 0)
            {
                hasHit = false;
                hitCooldownTimer = 1;
            }//This is for after a unit has hit, after the hit timer runs down to 0 it will reset to the original values.  (Hit cooldown)


            if (Health <= 75 && Health > 50)
            {
                //m_renderer.material = blue;
            }
            if (Health <= 50 && Health > 20)
            {
                //m_renderer.material = red;
            }   //Visual aids   ###TO BE REMOVED###

            if (Health <= 20)
            {
                CurrentState = States.RunAway;
            }

            if (Health <= 0)
            {
                Destroy(this.gameObject);
                // enemy_AI.enabled = false;
                // this.gameObject.tag = "Untagged";

            } //This destroys the game object once reaching 0 HP
            if (Target != null)
            {

                Distance = Vector3.Distance(transform.position, Target.position);
            } //If its found a target this sets the Distance to the target each frame
            else if (Target == null)
            {
                Distance = 0;
                CurrentState = States.Idle;

            } //If no target is found then it remains in idle, reset statement

            switch (CurrentState)//Overall start of the state machine
            {
                case States.Idle:
                    {


                        if (Health > 20)
                        {
                            timer += Time.deltaTime; //Starts a frame timer for how long the AI can be in idle before switching out
                            RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);   //Creates a sphere around the AI position and spawns a waypoint [custom function]
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
                                }
                            }//Ensuring the AI units path towards the correct waypoints, rather than pathing towards the same waypoint

                            if (timer <= wanderTimer && wayPointGO != null)
                            {

                                enemy_AI.SetDestination(wayPointGO.transform.position); //Sets destination towards the spawned waypoint
                                NavMeshPath path = new NavMeshPath(); //Creates a temporary path in memory that can be queried
                                print("Path Found");
                                enemy_AI.CalculatePath(wayPointGO.transform.position, path); //This calculates whether the path the AI has created is Valid, Invalid or Partial
                                if (path.status == NavMeshPathStatus.PathInvalid) //If the AI can't complete the path its created (Off navmesh/blocked)
                                {

                                    Destroy(wayPointGO);//Destroys the currently unreachable waypoint 
                                    print("invalid path");
                                    wayPoint = false;//States that no waypoint is active so another is to be spawned    [Inside RandomNavSphereWaypoint function]
                                }
                                else if (wayDistance <= enemy_AI.stoppingDistance)
                                {
                                    Destroy(wayPointGO);
                                    print("Path concluded");
                                    wayPoint = false;  //If the AI unit reaches the waypoint, destroy it and create another
                                }
                                //Vector3 newPos = RandomNavSphere(this.transform.position, wanderRadius, -1);

                                //enemy_AI.SetDestination();

                            }

                            if (timer >= wanderTimer)
                            {
                                timer = 0;
                                Search();//Once the timer reaches its max allowed value, move into search
                            }

                            if (Health <= 50 && dizzy == false && dizzyCheck == 0)
                            {
                                CurrentState = States.Dizzy;

                                dizzy = true; //If hit below 50% hp for first time, become dizzy    ###BUGGY###
                                dizzyCheck = dizzyCheck + 1; //Not used currently
                            }
                            if (dizzy) //Ignore for now
                            {
                                dizzyTimer += Time.deltaTime;

                                if (dizzyTimer >= 5)
                                {
                                    enemy_AI.enabled = true;
                                    dizzy = false;
                                }
                            }
                            if (hasHit == true)
                            {
                                hitCooldownTimer -= Time.deltaTime; //If the unit has hit the enemy a timer will count down to 0, when at 0 it can hit again.  Functions as a hit cooldown.
                            }
                        }

                        break;
                    }

                case States.Search: //State machine, unsure if it's required to be inside the update function but its there now
                    {
                        Search();
                        break;
                    }

                case States.Move:
                    {

                        Move();
                        break;
                    }

                case States.Hit:
                    {
                        Hit();
                        hitTimer += Time.deltaTime; //Literally no idea what this does
                        break;
                    }

                case States.RunAway:
                    {
                        print("Switchstates");
                        runAway();
                        break;
                    }
                case States.Damaged:
                    {

                        break;
                    }

                case States.Dizzy:
                    {
                        runAway();
                        break;
                    }
            }



        }
    }

       







    private void Search()
    {

        Destroy(wayPointGO);
		//Uses an integer to define what search function it follows, can be set in code and editor
        if (Type == 1)
        {
            Target = FindClosestEnemy().transform;
            TargetObj = FindClosestEnemy(); //Finds the game object and the transform location of the nearest enemy   [Custom functions]
            if (Target != null)
            {
                CurrentState = States.Move; //If a target is found then switch into Move state
            }
        }
        else if (Type == 2)
        {

            TargetObj = FindRandomEnemy();
            Target = TargetObj.transform;//Finds the game object and the transform location of a random  [Custom functions]
            if (Target != null)
            {
                CurrentState = States.Move;
            }
        }

        else if (Type == 3)
        {
            TargetObj = FindLowestHPEnemy();
            Target = TargetObj.transform; //Finds the game object and the transform location of the lowest Health enemy  [Custom functions]
            if (Target != null)
            {
                CurrentState = States.Move;
            }
            else if (Target == null)
            {
                CurrentState = States.Idle;
            }
        }
    }


    private void Move()
    {
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


    public void Hit()
    {
        //if(TargetObj.tag == "Enemy")
        //{
              //Finds the AI script on the hit enemy, in order to find and manipulate AI variables
        //}
             
        Rigidbody hitRB = TargetObj.GetComponent<Rigidbody>();  //Finds the rigid body attatched to the target, used for knockback

        if (hasHit == false) //If the AI hasn't hit recently
        {
          
            this.gameObject.transform.LookAt(hitRB.transform);
            hitRandom = Random.Range(0, 20); //Creates a random number between 0 and 20
            if (TargetObj.tag == "Enemy") //Checks whether its an AI or Player fighter
            {
                AI hitTarget = TargetObj.GetComponent<AI>();
                if (hitRandom >= 0 && hitRandom <= 5) //If the random number is between 0 and 10, do a normal attack
                {
					Healthblocks hitHPScript = TargetObj.GetComponent<Healthblocks>();
                    player_Anim.Right_Punch(); //Plays a punch animation
                    hitHPScript.NormalDamage(); //Calls the damage script in the enemy's hp script       !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!CURRENTLY REFERENCING ITS OWN SCRIPT!!!!!!!!!!!!!!!!!!!!!!!!
                    moveDirection = TargetObj.transform.position - this.transform.position; //Direction away from the attacker, backwards for the target
                    hitRB.AddForce(moveDirection.normalized * +4000f); //Adds a force to the target rigid body, using the above defined direction
                    hitTarget.previousHealth = Health;
                    hitTarget.Health -= 10;  //Current target health is reduced by 10 on a normal hit
                    hasHit = true; //This AI is now considered to have hit and will have to wait for the hit cooldown
                    hitTarget.HPBarSet = true;
                    hitTarget.HPBarHitValue = 10; //Depreciated
                    hitTarget.enemy_AI.enabled = false;

                }


            }

            if(TargetObj.tag == "Player")
            {
              Healthblocks hitHPScript = TargetObj.GetComponent<Healthblocks>();
			  player_Anim.Right_Punch();
              hitHPScript.NormalDamage();
            }
            
        }


        else
        {
            CurrentState = States.Idle;
            timer = 0; //If hasHit is true then switch back into the idle state
        }


        
    }



    public void Dizzy()
    {
        CurrentState = States.RunAway;
		
    }


    public void runAway()
    {
		print("HEllo");
		
        Target = FindClosestEnemy().transform;
        TargetObj = FindClosestEnemy(); //Finds the closest tagged enemy unit
		AI hitTarget = TargetObj.GetComponent<AI>();
        if (Target != null) //If an enemy is found in the scene then continue
        {
			if(hitTarget.Health <= Health)
			{
				CurrentState = States.Move;
                print("Chasing");
			}
			
			if(hitTarget.Health > Health)
			{
                print("Running");
				RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);
				
				
                print("Waypointed");
				if (AINumber == 1)
				{
					try
					{
						wayPointGO = GameObject.Find("Waypoint_AI1");
                        print("Found run way");
                        print(wayPointGO.name);
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
                    print("Collider found");

                    if (way.Reset == true)
                    {
                        Destroy(wayPointGO);
                        wayPoint = false;
                        RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);
                        print("Col");


                    }
                    else if (way.Reset == false)
                    {
                        print("WTFGUYS");

                        enemy_AI.isStopped = false;
                        enemy_AI.speed = 5;
                        NavMeshPath path = new NavMeshPath();
                        enemy_AI.CalculatePath(wayPointGO.transform.position, path); //This calculates whether the path the AI has created is Valid, Invalid or Partial
                        print("LMAO");
                        if (path.status == NavMeshPathStatus.PathInvalid) //If the AI can't complete the path its created (Off navmesh/blocked)
                        {
                            print("Hellotehre;");
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
		
            /*RandomNavSphereWaypoint(this.transform.position, wanderRadius, -1);
            if (AINumber == 1)
            {
                wayPointGO = GameObject.Find("Waypoint_AI1");
            }
            else if (AINumber == 2)
            {
                wayPointGO = GameObject.Find("Waypoint_AI2");
            }
            else if (AINumber == 3)
            {
                wayPointGO = GameObject.Find("Waypoint_AI3");
            }
            else if (AINumber == 4)
            {
                wayPointGO = GameObject.Find("Waypoint_AI4");
            }//Same as in idle 
            wayDistance = Vector3.Distance(transform.position, wayPointGO.transform.position);  //Defines the distance between the AI unit and the newly created Waypoint

            waypointToEnemyDistance = Vector3.Distance(wayPointGO.transform.position, Target.transform.position); //Defines the distance between the closest enemy unit and the Waypoint

            if (wayDistance < waypointToEnemyDistance) //If the waypoint is closer to the AI unit than it is to the enemy target unit, then continue
            {
                enemy_AI.SetDestination(wayPointGO.transform.position); //Sets navmesh destination

                if (wayDistance <= enemy_AI.stoppingDistance)
                {
					Destroy(wayPointGO);
                    wayPoint = false;    //If the waypoint is reached destroy it and set waypoint to not active
                    CurrentState = States.Idle; //Moves back into the idle state, which should move straight back into the runaway state
                    

                }
                else if (wayDistance >= enemy_AI.stoppingDistance)
                {
                    if (wayDistance > 2 * enemy_AI.stoppingDistance)
                    {
                        enemy_AI.speed = 2;
                    }
                    else
                    {
                        enemy_AI.speed = 1;
                    }

                    enemy_AI.isStopped = false;
                    enemy_AI.SetDestination(wayPointGO.transform.position); //Same as in idle and move
                }
            }
        }*/
       /* else if (Target == null)
        {
            Target = FindClosestEnemy().transform;
            TargetObj = FindClosestEnemy();
			print ("Breaking");			//If no target is found, keep searching, assumed to be a halting state
        }*/
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




        /* randDirection += origin;
         NavMeshHit navHit;
         NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
         return navHit.position;*/
    }








    public void SetTarget(Transform t)
    {
        Target = t; //Literally no idea if this is still being used but I don't have access to unity to test

    }


    public GameObject FindClosestEnemy()
    {
		print("Into closest");
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

        return closest; //Returns the closest game object
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



    public GameObject FindLowestHPEnemy()
    {
        List<GameObject> randomList = new List<GameObject>();

		print("In search");

        GameObject lowestHPEnemy = null;
		//Same as above but no array for some reason, presumably cleaner code that didn't get changed in random enemy
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (go.Equals(this.gameObject))
			{
                continue;
			}

            else
            {
                AI hitTarget = go.GetComponent<AI>(); //This looks for the AI script on the targets found to see its variables

                if (hitTarget.Health <= hpSearchcheck) //If the health value of the target is lower than the previously set 
                {
                    lowestHPEnemy = go; //Sets the return game object
                    hpSearchcheck = hitTarget.Health; //Sets this to the new lowest hp value to check against
                }
            }

        }


        randomList.Clear();
        hpSearchcheck = 300;
        return lowestHPEnemy; //Resets the function for its next run and returns the object


    }
}


/*
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}*/
