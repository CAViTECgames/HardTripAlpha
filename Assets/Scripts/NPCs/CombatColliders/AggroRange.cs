using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Unit colliderOwner = gameObject.GetComponentInParent<Unit>();
        Unit victim = other.gameObject.GetComponent<Unit>();

        // Lo que ha entrado en el área no es una unidad
        if (victim == null)
            return;

        if (colliderOwner.getCombatStatus() == Unit.CombatStatus.COMBAT_STATUS_CORPSE)
            return;

        Debug.LogWarning("Entered aggro range " + other.gameObject.name);

        // Si es el jugador o la caravana...
        if (victim.getFaction() == Unit.Faction.FACTION_FRIENDLY)
        {
            colliderOwner.addUnitInAggroRange(victim);
            colliderOwner.enterInCombatWith(victim);
            victim.enterInCombatWith(colliderOwner);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Unit colliderOwner = gameObject.GetComponentInParent<Unit>();
        Unit victim = other.gameObject.GetComponent<Unit>();

        // Lo que ha salido del área no es una unidad
        if (victim == null)
            return;

        Debug.LogWarning("exited aggro range " + other.gameObject.name);

        // Eliminamos a la unidad de la lista
        colliderOwner.removeUnitInAggroRange(victim);

        // Ambas unidades salen de sus mutuos combates
        colliderOwner.exitCombatWith(victim);
        victim.exitCombatWith(colliderOwner);

        // Si el que sale era nuestro target, hay que elegir otro
        if (victim.Equals(colliderOwner.getTarget()))
        {
            Debug.LogWarning(other.gameObject.name + " ha salido de nuestro área de aggro y era nuestro target");
            colliderOwner.handleLosingTarget();
        }
       
    }
}
