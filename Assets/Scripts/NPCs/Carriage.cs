using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carriage : Unit
{

    // Combat template characterisitcss
    private const int attackDelay = 150;

    // Use this for initialization
    void Start()
    {
        // General characteristics
        id = 1;
        type = CreatureType.CREATURE_TYPE_MISC;
        faction = Faction.FACTION_FRIENDLY;
        movementSpeed = ConfigController.carriageSpeed;
        scale = 8;
        //setScale();

        // Combat template characterisitcs
        attackDamage = 0;
        healthPoints = ConfigController.carriageMaxLife;
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
        player = true;
    }

    // Getters

    protected override int getAttackDelay()
    {
        return attackDelay;
    }
}
