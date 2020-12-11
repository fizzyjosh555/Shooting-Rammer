using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class NPCTankController : AdvancedFSM
{
   // public GameObject Bullet;
    public int health;
    public static int score;
    public Text youWin;
    public float worktime;
    public GameObject healpoint;
    public bool nothealing = true;
    public bool timeoff = false;
    public Slider sliderEnemy;
    public Camera main_camera;
    private Quaternion rotation;
    // We overwrite the deprecated built-in `rigidbody` variable.
    new private Rigidbody rigidbody;

    public tanksonduty Tankcheck;

    public float timecheckdutyoff;

    public Material m_Material;
    public Material m_Material_fastat;
    public GameObject default_m;


    public int fasttest;
    public bool fastat;
    public GameObject fastdet;

    public GameObject[] othertanks;

    public int FieldOfView = 45;
    public int ViewDistance = 100;

    private Transform playerTrans;
    private Vector3 rayDirection;

    public bool levelrestart = false;
    public float timerestart = 0.0f;

    public Image heard;
    public Image seen;
    public Image received;

    public float activetimer = 0.0f;

    public bool touch_player = false;
    public bool see_player = false;

    public bool hasheard = false;
    public bool hasreceived = false;

    public UnityEngine.AI.NavMeshAgent navagent;

    public float speed;

    public GameObject parent;

    public GameObject navagentholder;

    //public bool inattack = false;
    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize()
    {
        health = 100;
        timecheckdutyoff = Random.Range(10.0f, 15.0f);
        elapsedTime = 0.0f;
        
        rotation = sliderEnemy.transform.rotation;
        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        worktime = Random.Range(45.0f, 60.0f);
        //Get the rigidbody
        rigidbody = GetComponent<Rigidbody>();
        m_Material = GetComponent<Renderer>().material;
        m_Material.color = Color.green;
        seen.enabled = false;
        heard.enabled = false;
        received.enabled = false;
        seen.IsActive();
        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
        fasttest =Random.Range(0,11);
        if (fasttest <= 5)
        {
            fastat = false;
            fastdet.SetActive(false);
            GetComponent<Renderer>().material = default_m.GetComponent<Renderer>().material;
            shootRate = 2.0f;
        }
        else
        {
            fastat = true;
            fastdet.SetActive(true);
            GetComponent<Renderer>().material = m_Material_fastat;
            shootRate = 0.5f;
        }
        navagent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        speed = navagent.speed;
        
        //Get the turret of the tank
        //turret = gameObject.transform.GetChild(18).transform;
        //bulletSpawnPoint = turret.GetChild(18).transform;
        
        //Start Doing the Finite State Machine
        ConstructFSM();
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        //Check for health
        elapsedTime += Time.deltaTime;
    }

    protected override void FSMFixedUpdate()
    {
        CurrentState.Reason(playerTransform, transform);
        CurrentState.Act(playerTransform, transform);
        sliderEnemy.value = health;
        

        sliderEnemy.transform.LookAt(transform.position + main_camera.transform.rotation * Vector3.back,
                                     main_camera.transform.rotation * Vector3.up);

        sliderEnemy.transform.rotation = rotation;
        if(levelrestart)
        {
            timerestart += Time.deltaTime;
            if (timerestart >= 5.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
                
            }
        }
        
        
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    private void ConstructFSM()
    {
        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("WandarPoint");

        Transform[] waypoints = new Transform[pointList.Length];
        int i = 0;
        foreach(GameObject obj in pointList)
        {
            waypoints[i] = obj.transform;
            i++;
        }

        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        patrol.AddTransition(Transition.LowHealth, FSMStateID.Healing);
        patrol.AddTransition(Transition.Bored, FSMStateID.Dance);
        patrol.AddTransition(Transition.dayend, FSMStateID.dayover);
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        patrol.AddTransition(Transition.startHiding, FSMStateID.Hiding);

        ChaseState chase = new ChaseState(waypoints);
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
        chase.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AttackState attack = new AttackState(waypoints);
        attack.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        attack.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        attack.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        DanceState dance = new DanceState(waypoints);
        dance.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        dance.AddTransition(Transition.dancedone, FSMStateID.Patrolling);
        dance.AddTransition(Transition.LowHealth, FSMStateID.Healing);
        dance.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        HealState heal = new HealState(waypoints);
        heal.AddTransition(Transition.ExitPad, FSMStateID.Patrolling);
        HideState Hide = new HideState(waypoints);
        Hide.AddTransition(Transition.doneHiding, FSMStateID.Patrolling);
        Hide.AddTransition(Transition.LowHealth, FSMStateID.Healing);

        RamState Ram = new RamState(waypoints);
        Ram.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);



        OffState od = new OffState(waypoints);


        od.AddTransition(Transition.daystart, FSMStateID.Patrolling);

        DeadState dead = new DeadState();
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);
        AddFSMState(dance);
	    AddFSMState(heal);
        AddFSMState(Hide);
        AddFSMState(od);
        AddFSMState(Ram);
       //AddFSMState(coolDown);
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            
            if (nothealing && !timeoff)
            {
                health -= 50;
                m_Material.color = Color.red;
            }

            if (health <= 0)
            {
                Debug.Log("Switch to Dead State");
                

                SetTransition(Transition.NoHealth);
                m_Material.color = Color.black;
                Explode();
            }
        }
    }

    protected void Explode()
    {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            rigidbody.AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            rigidbody.velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }
        
        Destroy(gameObject, 1.5f);
        Destroy(parent, 1.5f);
    }

    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    public void ShootBullet()
    {
        
       
        if (elapsedTime >= shootRate)
        {
            
           // Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }
}
