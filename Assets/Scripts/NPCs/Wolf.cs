using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Unit {

    // Combat template characterisitcss
    private const int  attackDelay = 90;

    // Incializamos el lobo
    void Start ()
    {
        // General characteristics
        id = 1;
        type = CreatureType.CREATURE_TYPE_BEAST;
        faction = Faction.FACTION_ENEMY;
        movementSpeed = ConfigController.wolfSpeed;
        scale = 8;
        setScale();

        // Combat template characterisitcs
        attackDamage = 15;
        healthPoints = ConfigController.wolfMaxLife;
        armor = 5;

        // Current CombatStatus
        combatStatus = CombatStatus.COMBAT_STATUS_IDLE;
        combatAttackDelay = attackDelay;
        unitsInAggroRange = new ArrayList();
        unitsInAttackRange = new ArrayList();
        unitsInCombatWith = new ArrayList();
        currentCombats = new ArrayList();
        target = null;

        // Misc
        player = false;
    }

    // Getters
    protected override int getAttackDelay()
    {
        return attackDelay;
    }

}
