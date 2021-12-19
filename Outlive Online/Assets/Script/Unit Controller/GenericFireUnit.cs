using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;

public class GenericFireUnit : UnitBehaviour
{

    public int fireRange;
    // public int 
    public GenericFireUnit(Player player) : base(player)
    {}
    
    protected override void ExecuteAttackCommand(AttackCommand attackCommand){
        navMeshAgent.SetDestination(attackCommand.getCoordinates());
        navMeshAgent.isStopped = false;
        navMeshAgent.avoidancePriority = ATTACK_PRIORITY;

    }

    protected override void UpdateAttackCommand(){
        AttackCommand attackCommand = (AttackCommand) standCommand;
        foreach (GameObject u in player.GetUnitsInScene())
        {
            ICommandableUnit commandable;

            if(u.TryGetComponent<ICommandableUnit>(out commandable))
            // navMeshAgent = null;
                if (commandable.player != player && Vector3.SqrMagnitude(transform.position - u.transform.position) <= Mathf.Pow(fireRange, 2))
                {
                    navMeshAgent.avoidancePriority = ATTACK_PRIORITY;
                    navMeshAgent.isStopped = true;
                    break;
                }
                else
                {
                    navMeshAgent.avoidancePriority = MOVIMENT_PRIORITY;
                    navMeshAgent.isStopped = false;
                    base.UpdateAttackCommand();
                    // this.UpdateAttackCommand();
                }
        }
    }

    
}
