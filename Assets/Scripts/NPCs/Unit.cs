using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 *  Hard Trip
 *  Cavitec Games (2017-2018)
 *  Developed by Defu
 * 
 *  En esta clase se modela cualquier unidad o entidad que pueda entrar en combate.
 *  El combate se modela en la función Update de aquí, es lo mejor para evitar problemas de escalabilidad
 *  Los métodos dentro de cada grupo están ordenados alfabeticamente
 *  
 */

public abstract class Unit : MonoBehaviour
{
    // Faction types
    public enum Faction
    {
        FACTION_FRIENDLY = 0,           // Tomamos siempre como referencia el jugador, por lo que un ejemplo de friendly sería la caravan
        FACTION_ENEMY,                  // Enemigos agresivos, bandidos o lobos
        FACTION_NEUTRAL,                // Los bandidos negociadores serían neutrales en un principio.
    }

    // Creature type, puede ser útil para los looteos
    public enum CreatureType
    {
        CREATURE_TYPE_BEAST,            // Lobos
        CRETAURE_TYPE_HUMANOID,         // Bandidos
        CREATURE_TYPE_MISC              // Caravana
    }

    // Diferentes estados durante el combate
    public enum CombatStatus
    {
        COMBAT_STATUS_IDLE = 0,
        COMBAT_STATUS_ATTACKING,
        COMBAT_STATUS_ATTACK_READY,
        COMBAT_STATUS_WAITING_FOR_ATTACK,
        COMBAT_STATUS_DEFENDING,
        COMBAT_STATUS_STUNNED,
        COMBAT_STATUS_DEAD
    }

    // General characteristics
    protected Faction faction;
    protected int id;
    protected float movementSpeed;
    protected float scale;
    protected CreatureType type;

    // Combat template characterisitcs
    protected int attackDamage;           // Daño base de nuestros atques
    protected int healthPoints;           // Nuestros puntos de vida
    protected int armor;                  // Reducción de daño al recibir un golpe
    
    // Current Combat characteristics
    protected CombatStatus combatStatus;
    protected int combatAttackDelay;      // Este valor cambia durante el combate, al llegar a cero se ataca y se resetea al valor de plantilla
    protected ArrayList unitsInAggroRange;
    protected ArrayList unitsInCombatWith;
    protected ArrayList unitsInAttackRange;
    protected ArrayList currentCombats;
    protected bool inCombat;
    protected Unit target;
    
    // Misc
    protected bool player;
    protected int updateFrames;

    // Getters

    public int getArmor()
    {
        return armor;
    }

    // Con este método cogemos el tiempo entre ataques, definido en la clase hija
    protected abstract int getAttackDelay();  

    public Unit getClosestUnitInAggroRange()
    {
        // Aqui deberiamos pillar el más cercano

        return (Unit)unitsInAggroRange[0];
    }

    public CombatStatus getCombatStatus()
    {
        return combatStatus;
    }

    public Faction getFaction()
    {
        return faction;
    }

    public int getHealthPoints()
    {
        return healthPoints;
    }

    public Unit getTarget()
    {
        return target;
    }

    public ArrayList getUnitsInAttackRange()
    {
        return unitsInAttackRange;
    }

    public ArrayList getUnitsInAggroRange()
    {
        return unitsInAggroRange;
    }

    // Setters

    public void setCombatStatus(CombatStatus combatStatus)
    {
        this.combatStatus = combatStatus;
    }

    public void setScale()
    {
        float scale_x = gameObject.transform.localScale.x * scale;
        float scale_y = gameObject.transform.localScale.y * scale;
        float scale_z = gameObject.transform.localScale.z * scale;
        gameObject.transform.localScale.Set(scale_x, scale_y, scale_z);
    }

    public void setTarget(Unit target)
    {
        this.target = target;
    }

    // Combat

    public void addUnitInAttackRange(Unit victim)
    {
        unitsInAttackRange.Add(victim);
    }

    public void addUnitInAggroRange(Unit victim)
    {
        unitsInAggroRange.Add(victim);
    }

    public void attackUnit(Unit victim)
    {
        Debug.LogWarning(gameObject.name + " Ataca a " + victim.gameObject.name);
        // Si no se está defencdiendo, le dañamos
        if(victim.getCombatStatus() != Unit.CombatStatus.COMBAT_STATUS_DEFENDING)
            dealDamageToUnit(victim);
        // Tenemos que volver a esperar por nuestro timer
        setCombatStatus(Unit.CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK);
        resetCombatAttackDelay();
    }

    public void chaseUnit(Unit victim)
    {
        if (player)
            return;

        NavMeshAgent nav = gameObject.GetComponentInParent<NavMeshAgent>();

        nav.isStopped = false;
        nav.SetDestination(victim.gameObject.transform.position);
    }

    public void dealDamageToUnit(Unit victim)
    {
        int dmg = attackDamage - victim.getArmor();

        Debug.LogWarning("El daño del ataque es" + dmg);
        Debug.LogWarning("La salud restante de " + victim.gameObject.name + " es " + victim.getHealthPoints());

        if (victim.reduceHealthPointsBy(dmg) <= 0)
            victim.setCombatStatus(Unit.CombatStatus.COMBAT_STATUS_DEAD);
    }

    public void enterInCombatWith(Unit victim)
    {
        inCombat = true;
        unitsInCombatWith.Add(victim);
        Debug.LogWarning(gameObject.name + ": Acabo de entrar en combate, preparando mi ataque");
        setCombatStatus(CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK);
    }

    public void finishCombat()
    {
        inCombat = false;
        target = null;
        stopChasing();
        Debug.LogWarning(gameObject.name + ": Salgo de combate, esperando a más enemigos");
        setCombatStatus(CombatStatus.COMBAT_STATUS_IDLE);
    }

    public void handleLosingTarget(Unit target)
    {
        // Eliminamos a la unidad de la lista
        removeUnitInAggroRange(target);

        // Si era la última unidad, salimos de combate y volvemos
        if (getUnitsInAggroRange().Count == 0)
        {
            finishCombat();
            return;
        }
        // Si aún hay gente en nuestro rango, elegimos el más cercano
        else
            setTarget(getClosestUnitInAggroRange());
    }

    public bool isInAttackRangeWith(Unit unit)
    {
        if (unitsInAttackRange.Contains(unit))
            return true;
        return false;
    }

    public int reduceHealthPointsBy(int damage)
    {
       return healthPoints -= damage;
    }

    public void reduceCombatAttackDelayBy(int frames)
    {
        combatAttackDelay = combatAttackDelay -  frames;
    }

    public void removeUnitInAttackRange(Unit victim)
    {
        unitsInAttackRange.Remove(victim);
    }

    public void removeUnitInAggroRange(Unit victim)
    {
        unitsInAggroRange.Remove(victim);
    }

    public void resetCombatAttackDelay()
    {
        combatAttackDelay = getAttackDelay();
    }

    public void stopChasing()
    {
        if (player)
            return;

        NavMeshAgent nav = gameObject.GetComponentInParent<NavMeshAgent>();
        nav.isStopped = true;
    }

    public void updateAttackStatus(int framesOffset)
    {
        // Según los frames que han pasado desde la última vez, reducimos el delay que queda para atacar
        if(combatStatus == Unit.CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK)
        {
            reduceCombatAttackDelayBy(framesOffset);
            Debug.LogWarning(gameObject.name + ": Mi timer se reduce en " + framesOffset + ", me quedan " + combatAttackDelay + " para poder volver a atacar");
        }


        // Si es menor o igual que 0, nuestro ataque está listo. Reseteamos el timer
        if (combatAttackDelay <= 0)
        {
            combatStatus = CombatStatus.COMBAT_STATUS_ATTACK_READY;
            Debug.LogWarning(gameObject.name + ": Mi siguiente ataque está listo!");
            resetCombatAttackDelay();
        }

    }

    // Misc

    public bool isPlayer()
    {
        return player;
    }

    public bool isInCombat()
    {
        return inCombat;
    }

    public void Update()
    {
        updateFrames++;
        if (updateFrames == ConfigController.combatUpdateFrames)
        {
            updateUnit(updateFrames);
            updateFrames = 0;
        }
    }

    public void updateUnit(int frameOffset)
    {
        // Si estamos muertos, desaparecemos
        if (combatStatus == CombatStatus.COMBAT_STATUS_DEAD)
        {
            Debug.LogWarning(this.gameObject.name + "Ha muerto");
            // Animación de muerte y tras X tiempo desaparecer
            gameObject.SetActive(false);
            return;
        }

        // Si somos la caravana, no hacemos nada más
        if (type == CreatureType.CREATURE_TYPE_MISC)
            return;

        // Sin target ya hemos acabado
        if (target == null)
        {
            return;
        }

        // Si nuestro target está muerto, cambiamos al más cercano
        if (target.getCombatStatus() == CombatStatus.COMBAT_STATUS_DEAD && !isPlayer())
            handleLosingTarget(target);
        
        // Si estamos en combate, comprobamos como va
        if(inCombat)
        {
            // Actualizamos nuestro timer
            updateAttackStatus(frameOffset);

            // Si estamos dentro del rango nos detenemos
            if (isInAttackRangeWith(target))
            {
                if (!isPlayer())
                    stopChasing();

                // Si nuestro ataque está listo, atacamos
                if (combatStatus == CombatStatus.COMBAT_STATUS_ATTACK_READY)
                    attackUnit(target);
            }
            // Si no lo estamos, perseguimos al target
            else if (!isPlayer())
                chaseUnit(target);
        }
    }
}
