using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Unit colliderOwner = gameObject.GetComponentInParent<Unit>();
        Unit victim = other.gameObject.GetComponent<Unit>();

        if (victim == null)
            return;

        if (victim.getFaction() == Unit.Faction.FACTION_FRIENDLY)
            colliderOwner.addUnitInAttackRange(victim);
        else if (colliderOwner.isPlayer() && victim.getFaction() == Unit.Faction.FACTION_ENEMY)
            colliderOwner.addUnitInAttackRange(victim);

        Debug.LogWarning(colliderOwner.name + ": Entered attack range " + victim.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Unit colliderOwner = gameObject.GetComponentInParent<Unit>();
        Unit victim = other.gameObject.GetComponent<Unit>();

        colliderOwner.removeUnitInAttackRange(victim);
    }

}