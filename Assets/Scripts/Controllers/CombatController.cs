using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Units in combat
    private Unit attacker;
    private Unit victim;

    private int updateFrames;

    // Constructor
    public Combat(Unit attacker, Unit b)
    {
        this.attacker = attacker;
        this.victim = b;
        updateFrames = 0;
    }

    void Update()
    {
        updateFrames++;
        if (updateFrames == ConfigController.combatUpdateFrames)
        {
            updateCombat(ConfigController.combatUpdateFrames);
            updateFrames = 0;
        }
    }

    public void updateCombat(int framesOffset)
    {
        // Comprobaciones por si alguno de los dos ha muerto
        if (victim.getCombatStatus() == Unit.CombatStatus.COMBAT_STATUS_DEAD || attacker.getCombatStatus() == Unit.CombatStatus.COMBAT_STATUS_DEAD)
           // attacker.finishCombat(this);

        // Actualizamos el estado de combate
        attacker.updateAttackStatus(framesOffset);

        // Si nuestro objetivo está dentro del rango de ataque 
        if (attacker.isInAttackRangeWith(victim))
        {
            // Si no somos un jugador y estamos a rango para atacar, dejamos de perseguir
            if (attacker.isPlayer())
                attacker.stopChasing();
            // Si nuestro ataque está listo y la víctma de este combate es nuestro objetivo, atacamos
            if (attacker.getCombatStatus() == Unit.CombatStatus.COMBAT_STATUS_ATTACK_READY && attacker.getTarget().Equals(victim))
                attacker.attackUnit(victim);
        }
        // Si el attacker no es un jugador, persigue a su objetivo
        else if (!attacker.isPlayer())
            attacker.chaseUnit(victim);

    }

    // Getters
    public Unit getVictim()
    {
        return victim;
    }

}
