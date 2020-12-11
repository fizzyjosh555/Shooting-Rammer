using UnityEngine;
using System.Collections;

public class ChaseState : FSMState
{
    public ChaseState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Chasing;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //Set the target position as the player position
        destPos = player.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 200.0f)
        {
            npc.GetComponent<NPCTankController>().fasttest = Random.Range(0, 11);
            if (npc.GetComponent<NPCTankController>().fastat)
            {

                if (npc.GetComponent<NPCTankController>().fasttest <= 3)
                {
                    npc.GetComponent<NPCTankController>().shootRate = 2.0f;
                }
                else
                {
                    npc.GetComponent<NPCTankController>().shootRate = 0.5f;
                }
            }
            else
            {

                if (npc.GetComponent<NPCTankController>().fasttest <= 7)
                {
                    npc.GetComponent<NPCTankController>().shootRate = 2.0f;
                }
                else
                {
                    npc.GetComponent<NPCTankController>().shootRate = 0.5f;
                }
            }
            Debug.Log("Switch to Attack state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.startRaming);
        }
        //Go back to patrol is it become too far
        else if (dist >= 300.0f)
        {
            npc.GetComponent<NPCTankController>().touch_player = false;
            npc.GetComponent<NPCTankController>().see_player = false;
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        destPos = player.position;
        npc.GetComponent<NPCTankController>().navagent.destination = destPos;

        npc.GetComponent<NPCTankController>().seen.enabled = true;
        npc.GetComponent<NPCTankController>().received.enabled = false;
        npc.GetComponent<NPCTankController>().hasreceived = false;
        npc.GetComponent<NPCTankController>().hasheard = false;
        npc.GetComponent<NPCTankController>().heard.enabled = false;
        //Rotate to the target point
        destPos = player.position;
        //npc.GetComponent<NPCTankController>().navagent.destination = destPos;
        //Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        //npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        ////Go Forward
        //npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
        
    }
}
