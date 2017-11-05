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

        Debug.LogWarning("Entered aggro range " + other.gameObject.name);

        // Si ya está en combate, añadimos a la lista de dentro del rango
        if(colliderOwner.isInCombat())
        {
            // Si es el jugador o la caravana...
            if (victim.getFaction() == Unit.Faction.FACTION_FRIENDLY)
                colliderOwner.addUnitInAggroRange(victim);
        }
        else
        {
            // Si no estamos en combate, lo añadimos a lista y marcamos a ese como target.
            if(victim.getFaction() == Unit.Faction.FACTION_FRIENDLY)
            {
                colliderOwner.addUnitInAggroRange(victim);
                colliderOwner.setTarget(victim);
                colliderOwner.enterInCombatWith(victim);
            }
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

        // Si el que sale era nuestro target, hay que elegir otro
        if (victim.Equals(colliderOwner.getTarget()))
        {
            Debug.LogWarning(other.gameObject.name + " ha salido de nuestro área de aggro y era nuestro target");
            colliderOwner.handleLosingTarget(victim);
        }
       
    }
}
