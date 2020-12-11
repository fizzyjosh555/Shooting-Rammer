using UnityEngine;
using System.Collections;

public class HealState : FSMState
{
	public HealState(Transform[] wp)
	{
	waypoints = wp;
	stateID = FSMStateID.Healing;
	curRotSpeed = 1.0f;
        curSpeed = 100.0f;
		
	FindNextPoint();
	}

	public override void Reason(Transform player, Transform npc)
	{
		destPos = npc.GetComponent<NPCTankController>().healpoint.transform.position;

		//if (npc.GetComponent<NPCTankController>().health <= 50)
		//{
		//	Debug.Log("Switch to Healing state");
		//	npc.GetComponent<NPCTankController>().SetTransition(Transition.ExitPad);
		//}

		//else if (npc.position == destPos)
		//{
		//	Debug.Log("Switch to Impervious state");
		//	npc.GetComponent<NPCTankController>().SetTransition(Transition.ExitPad);
		//}

		if (npc.GetComponent<NPCTankController>().health == 100)
		{
			timer = 0.0f;
			curSpeed = 100.0f;
			npc.GetComponent<NPCTankController>().navagent.speed = npc.GetComponent<NPCTankController>().speed;
			npc.GetComponent<NPCTankController>().nothealing = true;
			Debug.Log("Switch to Patrolling State");
			npc.GetComponent<NPCTankController>().SetTransition(Transition.ExitPad);
			npc.GetComponent<NPCTankController>().m_Material.color = Color.green;

		}

	}

	public override void Act(Transform player, Transform npc)
	{
		if (timer >= 0.1f && Vector3.Distance(npc.position, destPos) <= 100.0f)
		{
			timer = 0.0f;



			npc.GetComponent<NPCTankController>().navagent.speed = 0.0f;
			npc.GetComponent<NPCTankController>().health += 5;
			npc.GetComponent<NPCTankController>().nothealing = false;
			npc.GetComponent<NPCTankController>().m_Material.color = Color.blue;
		}
		else
		{

			timer += Time.deltaTime;
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
			npc.GetComponent<NPCTankController>().navagent.speed = npc.GetComponent<NPCTankController>().speed;
			
		}

	}
}