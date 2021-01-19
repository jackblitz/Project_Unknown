using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
    public WeaponItem KillBy;

    public AIStateId GetId()
    {
        return AIStateId.Death;
    }

    public void Enter(AIAgent agent)
    {
        agent.Ragdoll.OnActivateRagdoll();
        direction.y = 1;
        agent.Ragdoll.ApplyForce(direction * KillBy.HitForce);
        agent.UI.gameObject.SetActive(false);
       // agent.mesh.updateWhenOffscreen = true;
       // agent.weapons.DropWeapon();
    }

    public void Update(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }

    public void OnFOVEvent(AIAgent agent, int state, FieldOfView.VisibleObject visibleObject)
    {
        throw new System.NotImplementedException();
    }
}