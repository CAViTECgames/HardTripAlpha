using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    // Combat template characterisitcss
    private const int attackDelay = 150;

    // Use this for initialization
    void Start()
    {
        // General characteristics
        id = 1;
        type = CreatureType.CRETAURE_TYPE_HUMANOID;
        faction = Faction.FACTION_FRIENDLY;
        movementSpeed = 2;
        scale = 8;
        setScale();

        // Combat template characterisitcs
        attackDamage = 15;
        healthPoints = 100;
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
