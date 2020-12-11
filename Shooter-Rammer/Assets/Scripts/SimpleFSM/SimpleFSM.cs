using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleFSM : FSM 
{
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Heal,
        Dead,
        Dance,
    }

    //Current state that the NPC is reaching
    public FSMState curState;

    //Speed of the tank
    private float curSpeed;

    //Tank Rotation Speed
    private float curRotSpeed;
    bool healtrue = false;
    //Bullet
    public GameObject Bullet;

    //Whether the NPC is destroyed or not
    private bool bDead;
    private int health;

    Material m_Material;
    
    bool done = false;
    bool called = false;
    public Slider sliderEnemy;
    public Camera main_camera;
    private Quaternion rotation;
    public GameObject healarea;

    public float time;
    float timeStore;

    public float dancetime;

    private int randnum;

    public GameObject[] Dancepoints;

    int runtimes;
    int runtimesstore;

    int randpoit;

    float timer;
    //Initialize the Finite state machine for the NPC tank


    protected override void Initialize () 
    {
        curState = FSMState.Patrol;
        curSpeed = 150.0f;
        curRotSpeed = 2.0f;
        bDead = false;
        elapsedTime = 0.0f;
        shootRate = 3.0f;
        health = 100;
        timeStore = time;
        sliderEnemy.maxValue = health;
        sliderEnemy.value = health;
        runtimes = Random.Range(1, 5);
        rotation = sliderEnemy.transform.rotation;



        
        

        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("WandarPoint");

        //Set Random destination point first
        FindNextPoint();

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        m_Material = GetComponent<Renderer>().material;
        print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

        //Get the turret of the tank
        turret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).transform;
	}

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Heal: UpdateHealState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
            case FSMState.Dance: UpdateDanceState(); break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;
        sliderEnemy.value = health;

        sliderEnemy.transform.LookAt(transform.position + main_camera.transform.rotation * Vector3.back,
                                     main_camera.transform.rotation * Vector3.up);

        sliderEnemy.transform.rotation = rotation;

        if(health>= 100)
        {
            m_Material.color = Color.green;
             
        }

        if (health != 100 && healtrue)
        {
            m_Material.color = Color.yellow;
            health++;
        }
        //Go to dead state is no health left
        if (health <= 0)
            curState = FSMState.Dead;
        
        

        if (curState == FSMState.Patrol && time >= 0.0f)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = timeStore;
            randnum = Random.Range(0, 10);
        }
    }
    
    

    /// <summary>
    /// Patrol state
    /// </summary>
    protected void UpdatePatrolState()
    {
        curSpeed = 150;
        curRotSpeed = 2;
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        if (destPos == null)
        {
            FindNextPoint();
            turretRotation = Quaternion.LookRotation(destPos - turret.position);
        }

        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 100.0f)
        {
            print("Reached to the destination point\ncalculating the next point");
            FindNextPoint();
            turretRotation = Quaternion.LookRotation(destPos - turret.position);
        }
        //Check the distance with player tank
        //When the distance is near, transition to chase state
        else if (Vector3.Distance(transform.position, playerTransform.position) <= 300.0f)
        {
            print("Switch to Chase Position");
            curState = FSMState.Chase;
        }
        else if (health < 100)
        {
            curState = FSMState.Heal;
        }
        else if (randnum == 5)
        {
            runtimes = Random.Range(3 , 12);
            randnum = 0;
            print("Switch to Dance");
            curState = FSMState.Dance;
        }
        else if (called)
        {
            FindNextPoint();
            turretRotation = Quaternion.LookRotation(destPos - turret.position);
            called = false;
        }
        else
        {

        }

        //StartCoroutine(SwitchToDance());

        //Rotate to the target point
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);  

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    protected void UpdateDanceState()
    {
        float waitime = 5.0f;
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        
        if (dancetime > 0.0f)
        {
            waitime = dancetime;
        }

        curSpeed = 0;
        curRotSpeed = 15;
        if (Vector3.Distance(transform.position, playerTransform.position) <= 300.0f )
        {
            curSpeed = 150;
            curRotSpeed = 2;
            print("Switch to Chase Position");
            curState = FSMState.Chase;
        }
        else if (runtimes <= 0)
        {
            curSpeed = 150;
            curRotSpeed = 2;
            runtimes = 20;
            print("Switch to Patrol");
            curState = FSMState.Patrol;
        }
        else if (!called)
        {
            timer = 0.0f;
            randpoit = Random.Range(0, Dancepoints.Length);
            
            destPos = Dancepoints[randpoit].transform.position;
            turretRotation = Quaternion.LookRotation(destPos - turret.position);
            called = true;
        }
        else 
        {

            print("running dance");
        }

        if(waitime > timer)
        {
            timer += Time.deltaTime;
        }
        
        else if (waitime <= timer)
        {
            randpoit = Random.Range(0, Dancepoints.Length);
            destPos = Dancepoints[randpoit].transform.position;
            timer = 0.0f;
            runtimes--;
            turretRotation = Quaternion.LookRotation(destPos - turret.position);


        }
        else
        {
            
        }
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);
        
    }

    

    /// <summary>
    /// Chase state
    /// </summary>
    protected void UpdateChaseState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= 200.0f)
        {
            curState = FSMState.Attack;
        }
        //Go back to patrol is it become too far
        else if (dist >= 300.0f)
        {
            curState = FSMState.Patrol;
        }
        else if (health < 100)
        {
            curState = FSMState.Heal;
        }

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    /// <summary>
    /// Attack state
    /// </summary>
    protected void UpdateAttackState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;

        //Check the distance with the player tank
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= 200.0f && dist < 300.0f)
        {
            //Rotate to the target point
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);  

            //Go Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            curState = FSMState.Attack;
        }
        //Transition to patrol is the tank become too far
        else if (dist >= 300.0f)
        {
            curState = FSMState.Patrol;
        }
        else if (health < 100)
        {
            curState = FSMState.Heal;
        }


        //Always Turn the turret towards the player
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed); 

        //Shoot the bullets
        ShootBullet();
    }

    protected void UpdateHealState()
    {
        //Set the target position as the player position
        destPos = healarea.transform.position;


        if(health >= 100)
        {
            curState = FSMState.Patrol;
        }
        
        if(transform.position != destPos)
        {
            //Rotate to the target point
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
            
            //Go Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
        }
        else
        {

        }
        
        
        
    }

    /// <summary>
    /// Dead state
    /// </summary>
    protected void UpdateDeadState()
    {
        //Show the dead animation with some physics effects
        if (!bDead)
        {
            m_Material.color = Color.black;
            bDead = true;
            Explode();
        }

    }

    

    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            //Shoot the bullet
            Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if(collision.gameObject.tag == "Bullet" && !healtrue)
        {
            m_Material.color = Color.red;
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
        else if(collision.gameObject.tag == "healpad")
        {
            m_Material.color = Color.red + Color.green + Color.yellow;
            healtrue = true;
            
        }
           
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "healpad")
        {
            m_Material.color = Color.red + Color.green + Color.yellow;
            healtrue = false;

        }

    }



    /// <summary>
    /// Find the next semi-random patrol point
    /// </summary>
    protected void FindNextPoint()
    {
        print("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;
        
        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;

        //Check Range
        //Prevent to decide the random point as the same as before
        if (IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }
    }

    /// <summary>
    /// Check whether the next random position is the same as current tank position
    /// </summary>
    /// <param name="pos">position to check</param>
    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if (xPos <= 50 && zPos <= 50)
            return true;

        return false;
    }

    protected void Explode()
    {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }

        Destroy(gameObject, 1.5f);
    }

}
