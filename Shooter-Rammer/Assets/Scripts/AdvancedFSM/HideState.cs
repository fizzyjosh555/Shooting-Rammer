using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : FSMState
{
    public HideState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.Hiding;

        curRotSpeed = 1.0f;        curSpeed = 100.0f;
       
        //find next Waypoint position
        FindNextPoint();
    }
    public override void Reason(Transform player, Transform npc)
    {
        //Set the target position to the npc position
        destPos = npc.position;


        //Check the distance with player tank

        if (Vector3.Distance(npc.position, player.position) >= 350.0f)
        {

            FindNextPoint();
            npc.GetComponent<NPCTankController>().touch_player = false;
            npc.GetComponent<NPCTankController>().see_player = false;
            npc.GetComponent<NPCTankController>().SetTransition(Transition.doneHiding);
            Debug.Log("switching to patrol state");
            npc.GetComponent<NPCTankController>().m_Material.color = Color.green;


        }
        else if (npc.GetComponent<NPCTankController>().health <= 50)
        {
            Debug.Log("Switch to Healing state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LowHealth);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.red;
        } else
        {
            
        }

    }

    public override void Act(Transform player, Transform npc)
    {
        //has enemy run away
        destPos = player.position * -1.0f;
        //Transform turret = npc.GetComponent<NPCTankController>().turret;
        //Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        //turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);
        //2. Rotate to the target point
        //Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        //npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);

        npc.GetComponent<NPCTankController>().navagent.destination = destPos;
        


    }


}
