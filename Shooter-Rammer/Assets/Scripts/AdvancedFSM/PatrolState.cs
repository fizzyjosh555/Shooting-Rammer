using UnityEngine;using System.Collections;public class PatrolState : FSMState{        public PatrolState(Transform[] wp)     {         waypoints = wp;        stateID = FSMStateID.Patrolling;


        timer = 0.0f;        curRotSpeed = 1.0f;        curSpeed = 100.0f;        FindNextPoint();    }    public override void Reason(Transform player, Transform npc)    {
        //1. Check the distance with player
        //2. Since the distance is near, transition to chase state
        //if (Vector3.Distance(npc.position, player.position) <= 300.0f)

        //{
            
            if(npc.GetComponent<NPCTankController>().fastat)
            {
                if(npc.GetComponent<NPCTankController>().see_player)
                {
                npc.GetComponent<NPCTankController>().heard.enabled = false;
                float willHide = Random.Range(0, npc.GetComponent<NPCTankController>().Tankcheck.aliveran());
                    if (willHide <= 1)
                    {


                        npc.GetComponent<NPCTankController>().SetTransition(Transition.startHiding);
                    }
                    else
                    {

                        timer = 0.0f;
                        timerduty = 0.0f;
                        npc.GetComponent<NPCTankController>().m_Material.color = Color.yellow;
                        Debug.Log("Switch to Chase State");
                        npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);




                    }

                }
            }
            else
            {
                if(npc.GetComponent<NPCTankController>().touch_player)
                {
                    float willHide = Random.Range(0, npc.GetComponent<NPCTankController>().Tankcheck.aliveran());
                    if (willHide <= 1)
                    {


                        npc.GetComponent<NPCTankController>().SetTransition(Transition.startHiding);
                    }
                    else
                    {

                        timer = 0.0f;
                        timerduty = 0.0f;
                        npc.GetComponent<NPCTankController>().m_Material.color = Color.yellow;
                        Debug.Log("Switch to Chase State");
                        npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);




                    }
                }
            }
                    

                    
              
   
        //}
     if (timer >= 15.0f)
        {
            npc.GetComponent<NPCTankController>().heard.enabled = false;
            timer = 0.0f;
            Debug.Log("Switch to dance State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.Bored);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.magenta;

        }
        else if (npc.GetComponent<NPCTankController>().health <= 50)
        {
            npc.GetComponent<NPCTankController>().heard.enabled = false;
            Debug.Log("Switch to Healing state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LowHealth);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.red;
        }
        else if (timerduty >= npc.GetComponent<NPCTankController>().worktime && npc.GetComponent<NPCTankController>().Tankcheck.tankreceive() > 0 )
        {
            npc.GetComponent<NPCTankController>().heard.enabled = false;
            npc.GetComponent<NPCTankController>().Tankcheck.tankdown();
            destPos = new Vector3(0, 0, 0);
            timerduty = 0.0f;
            curSpeed = 0.0f;
            npc.GetComponent<NPCTankController>().timecheckdutyoff = Random.Range(10.0f, 15.0f); 
            Debug.Log("Switch to offduty state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.dayend);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.clear;
        }
        else
        {
            npc.GetComponent<NPCTankController>().heard.enabled = false;
            timer += Time.deltaTime;
            timerduty += Time.deltaTime;
            // Debug.Log("timer: " + timer.ToString());
           //Debug.Log("timerduty: " + timerduty.ToString());
            npc.GetComponent<NPCTankController>().m_Material.color = Color.green;
        }
                    }    public override void Act(Transform player, Transform npc)    {                //1. Find another random patrol point if the current point is reached		if (Vector3.Distance(npc.position, destPos) <= 100.0f)
        {
            FindNextPoint();
            
        }        else
        {
            
        }
        if(npc.GetComponent<NPCTankController>().hasheard && npc.GetComponent<NPCTankController>().activetimer >= 5.0f)
        {
                npc.GetComponent<NPCTankController>().hasheard = false;
                npc.GetComponent<NPCTankController>().heard.enabled = false;
            
            
            npc.GetComponent<NPCTankController>().activetimer = 0.0f;
        }
        else if(npc.GetComponent<NPCTankController>().hasreceived && npc.GetComponent<NPCTankController>().activetimer >= 5.0f)
        {
            npc.GetComponent<NPCTankController>().received.enabled = false;
            npc.GetComponent<NPCTankController>().hasreceived = false;
            npc.GetComponent<NPCTankController>().activetimer = 0.0f;
        }
        else if (!npc.GetComponent<NPCTankController>().hasheard && !npc.GetComponent<NPCTankController>().hasreceived)
        {
            npc.GetComponent<NPCTankController>().activetimer = 0.0f;
        }
        else if(npc.GetComponent<NPCTankController>().hasheard )
        {
           
            
                npc.GetComponent<NPCTankController>().heard.enabled = true;
                
            
            
        }
        else if (npc.GetComponent<NPCTankController>().hasreceived)
        {
            npc.GetComponent<NPCTankController>().received.enabled = true;

        }
        else
        {
            npc.GetComponent<NPCTankController>().activetimer = npc.GetComponent<NPCTankController>().activetimer + Time.deltaTime;
        }
        npc.GetComponent<NPCTankController>().seen.enabled = false;


        //Transform turret = npc.GetComponent<NPCTankController>().turret;
        //Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        //turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);
        ////2. Rotate to the target point
        //Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        //npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
        ////3. Go Forward
        ///
        npc.GetComponent<NPCTankController>().navagent.destination = destPos;
        
    }    /*    private void SwitchToDance()
    {
        Debug.Log("Switch to Dance State");
        npc.GetComponent<NPCTankController>().SetTransition(Transition.Bored);
    }    */}