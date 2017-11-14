using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    // Combat template characterisitcss
    private const int attackDelay = 60;
    private const int attackWindow = 5;
    private int currentTargetIndex = 0;
    // Use this for initialization
    void Start()
    {
        // General characteristics
        id = 1;
        type = CreatureType.CRETAURE_TYPE_HUMANOID;
        faction = Faction.FACTION_FRIENDLY;
        movementSpeed = 2;
        scale = 8;
        setScale(scale);

        // Combat template characterisitcs
        attackDamage = 60;
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

    protected override int getCombatAttackWindow()
    {
        return attackWindow;
    }

    private void FixedUpdate()
    {
        playerSwapTargets();
        checkPlayerDefend();
        checkPlayerAttack();
    }

    // Misc

    protected void checkPlayerAttack()
    {
        if (!inCombat)
            return;

        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        if (combatStatus != CombatStatus.COMBAT_STATUS_ATTACK_READY)
            return;

        if (!getTarget())
            return;

        if (!isInAttackRangeWith(getTarget()))
            return;

        combatStatus = CombatStatus.COMBAT_STATUS_ATTACKING;
    }

    protected void checkPlayerDefend()
    {
        if (!inCombat)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            if(combatStatus != CombatStatus.COMBAT_STATUS_ATTACKING &&
               combatStatus != CombatStatus.COMBAT_STATUS_ATTACK_READY)
            {
                combatStatus = CombatStatus.COMBAT_STATUS_DEFENDING;
                Debug.Log("Player is defending!");
            }

            return;
        }

        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            combatStatus = CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK;
            Debug.Log("Player is not defending anymore!");
            return;
        }

    }

    protected void playerSwapTargets()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
            return;

            Debug.LogWarning("Buscando targets");
        if (!inCombat)
            return;

        Debug.LogWarning("Hay algo conmigo en combate");
        if (getTarget())
        {
            Debug.LogWarning("Quitando el anterior");
            if (getTarget().gameObject.transform.Find("TargetIndicator"))
                getTarget().gameObject.transform.Find("TargetIndicator").gameObject.SetActive(false);
        }

        Debug.LogWarning("Poneindo nuevo target");
        try
        {
            setTarget((Unit)unitsInCombatWith[currentTargetIndex]);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            currentTargetIndex = 0;
            return;
        }
        // Activamos el icono de target
        getTarget().gameObject.transform.Find("TargetIndicator").gameObject.SetActive(true);
        currentTargetIndex++;

        // Si es el ultimo de la lista reseteamos el target
        if (currentTargetIndex == (unitsInCombatWith.Count))
            currentTargetIndex = 0;
    }
        

    

}
