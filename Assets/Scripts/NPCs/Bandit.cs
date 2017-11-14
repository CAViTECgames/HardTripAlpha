using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Unit
{

    // Combat template characterisitcss
    private const int attackDelay = 150;
    private const int attackWindow = 5;

    // Incializamos el bandido
    void Start()
    {
        // General characteristics
        id = 1;
        type = CreatureType.CRETAURE_TYPE_HUMANOID;
        faction = Faction.FACTION_ENEMY;
        movementSpeed = ConfigController.banditAgressiveSpeed;
        scale = 8;
        setScale(scale);

        // Combat template characterisitcs
        attackDamage = 15;
        healthPoints = ConfigController.banditAgressiveMaxLife;
        armor = 5;

        // Current CombatStatus
        combatStatus = CombatStatus.COMBAT_STATUS_IDLE;
        combatAttackDelay = attackDelay;
        unitsInAggroRange = new ArrayList();
        unitsInAttackRange = new ArrayList();
        unitsInCombatWith = new ArrayList();
        currentCombats = new ArrayList();
        target = null;
        inCombat = false;

        // Misc
        player = false;
    }

    // Getters
    protected override int getAttackDelay()
    {
        return attackDelay;
    }

    protected override int getCombatAttackWindow()
    {
        return attackWindow;
    }
}
