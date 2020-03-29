Director AI must be attached to "GameManager" game object in the scene. This attempts to watch the state of each fighter and find a high threat/priority target for the AI to attack. Currently half implemented for finding players.


###Current powerup pathfinding requires objects to be tagged with "Powerup", the spawning script spawns them with different tags vs just using different object names###





EasyAI Prefab (currently named AI) -
Transform
Animator
Capsule collider (Deactivated)
Character animation   Script   (one active one non active)
Player attack FINAL (Should be deactive)
Navmesh Agent (Speed and distances will be tweaked as gameplay dictates)
Rigidbody    (Set to default values)
Easy AI Script     (Health - 120, Wander radius 10, Wander Timer 5, AI number 1-4, Waypoint prefab will need to be set from waypoint prefab)      (Includes a callable initialization script, but not implemented for startup, needs to be called via change scene script)
HealthBlocks Script (Element0 = HealthBar   Element1 = Healthbar1 etc)

Hit has 20% chance to hit, 80% chance to miss (animation will play for both)


Medium AI same as above:
Behaviours added: Search for random enemy/different attacks (left hand + Strong hit, Although strong damage needs to be checked on implementation)
Search and hit type is decided by random number.

0-2 are hits 3-5 misses    50% chance to hit    50% chance to miss    Can be changed

Working on powerup searching, likely will be priority based.



Current Script only uses basic punch as don't want to mess around with extra animations and collision hitboxes before we have our final/close to final models, for sake of not spending time bugfixing something that wont be used.
Higher difficulty AI will use different search methods (Random, LowestHP, Closest) which are already coded. 
AI priority for any in game drops needs to be coded in still. 
Player should be able to hit the AI     /     AI should be able to hit the Player.


WayPoint Prefab
Transform
Cube MeshFilter
MeshRenderer
BoxCollider (IsTrigger)
WayPointer Script


Canvas Prefab is included but setup of that will vary
