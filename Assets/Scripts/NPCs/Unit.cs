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
        COMBAT_STATUS_DEAD,
        COMBAT_STATUS_CORPSE
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
    protected int corpseDuration;
    protected int combatAttackDelay;      // Este valor cambia durante el combate, al llegar a cero se ataca y se resetea al valor de plantilla
    protected int combatAttackWindow;   // Tiempo que dura el ataque
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

    protected abstract int getCombatAttackWindow();

    public Unit getClosestUnitInAggroRange()
    {
        // Si está vacío devolvemos null
        if (unitsInAggroRange.Count == 0)
            return null;

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

    public void setScale(float scale_f)
    {
        this.scale = scale_f;
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
        Debug.Log(gameObject.name + " Ataca a " + victim.gameObject.name);

        // Si no se está defencdiendo, le dañamos
        if (victim.getCombatStatus() != Unit.CombatStatus.COMBAT_STATUS_DEFENDING)
            dealDamageToUnit(victim);

        resetCombatAttackDelay();
        resetAttackWindow();
        combatStatus = CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK;
    }

    public void chaseUnit(Unit victim)
    {
        if (player)
            return;

        Debug.Log(name + ": Persiguiendo a " + victim.name);

        setCurrentAnimation("run");

        NavMeshAgent nav = gameObject.GetComponentInParent<NavMeshAgent>();

        nav.enabled = true;
        nav.SetDestination(victim.gameObject.transform.position);
    }

    public void dealDamageToUnit(Unit victim)
    {
        if(victim.getCombatStatus() == CombatStatus.COMBAT_STATUS_DEAD)
        {
            Debug.Log("Unidad muerta: " + victim.name + "No se le puede dañar");
            return;
        }

        int dmg = attackDamage - victim.getArmor();

        Debug.Log("El daño del ataque es" + dmg);

        if (victim.reduceHealthPointsBy(dmg) <= 0)
            victim.setCombatStatus(Unit.CombatStatus.COMBAT_STATUS_DEAD);

        Debug.Log("La salud restante de " + victim.gameObject.name + " es " + victim.getHealthPoints());
    }

    public void enterInCombatWith(Unit victim)
    {
        if (unitsInCombatWith.Contains(victim))
        {
            Debug.Log(gameObject.name + ": Ya estoy en combate con: " + victim.name) ;
            return;
        }
            
        if(!inCombat && !player)
        {
            Debug.Log(gameObject.name + ": Mi nuevo target es " + victim.name);
            setTarget(victim);
        }


        inCombat = true;
        unitsInCombatWith.Add(victim);
        Debug.Log(gameObject.name + ": Acabo de entrar en combate, preparando mi ataque");
        setCombatStatus(CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK);
    }

    public void exitCombatWith(Unit victim)
    {
        unitsInCombatWith.Remove(victim);
        Debug.Log(gameObject.name + ": Acabo de salir de combate con " + victim.name);

        // No quedan más enemigos contra los que combatir, salimos de combate
        if (unitsInCombatWith.Count == 0)
            finishCombat();
    }

    public void finishCombat()
    {
        inCombat = false;
        target = null;
        stopChasing();
        Debug.Log(gameObject.name + ": Salgo de combate, esperando a más enemigos");

        if(combatStatus != CombatStatus.COMBAT_STATUS_CORPSE)
            setCombatStatus(CombatStatus.COMBAT_STATUS_IDLE);

        setCurrentAnimation("idle");
    }

    public void handleDeath()
    {
        combatStatus = CombatStatus.COMBAT_STATUS_CORPSE;
        setCurrentAnimation("dead");

        // Desactivamos el icono de target
        if (gameObject.transform.Find("TargetIndicator"))
            gameObject.transform.Find("TargetIndicator").gameObject.SetActive(false);
    }

    public void handleLosingTarget()
    {
        // Si seguimos en combate tras perder el target, elegimos otro nuevo
        if (inCombat)
            setTarget(getClosestUnitInAggroRange());
    }

    public void handleUnitDeath(Unit victim)
    {
        // Eliminamos a esa unidad de todas nuestras listas
        exitCombatWith(victim);
        removeUnitInAggroRange(victim);
        removeUnitInAttackRange(victim);
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

    public void resetAttackWindow()
    {
        combatAttackWindow = getCombatAttackWindow();
    }

    public void stopChasing()
    {
        if (player)
            return;

        NavMeshAgent nav = gameObject.GetComponentInParent<NavMeshAgent>();

        if (nav.enabled)
        {
            Debug.Log("Parando la persecución");
            nav.enabled = false;
            setCurrentAnimation("idle");
        }


    }

    public void updateAttackStatus(int framesOffset)
    {
        switch(combatStatus)
        {
            case CombatStatus.COMBAT_STATUS_ATTACKING:

                Debug.Log(gameObject.name + ": En ventana de ataque!");

                setCurrentAnimation("attack");

                combatAttackWindow -= framesOffset;
                if (combatAttackWindow <= 0)
                    attackUnit(target);

                break;

            case CombatStatus.COMBAT_STATUS_WAITING_FOR_ATTACK:

                setCurrentAnimation("idle");
                reduceCombatAttackDelayBy(framesOffset);
                //Debug.Log(gameObject.name + ": Mi timer se reduce en " + framesOffset + ", me quedan " + combatAttackDelay + " para poder volver a atacar");

                // Si es menor o igual que 0, nuestro ataque está listo. Reseteamos el timer
                if (combatAttackDelay <= 0)
                {
                    combatStatus = CombatStatus.COMBAT_STATUS_ATTACK_READY;
                    Debug.Log(gameObject.name + ": Mi siguiente ataque está listo!");
                }

                break;

            case CombatStatus.COMBAT_STATUS_ATTACK_READY:

                break;

            default:
                break;
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

    // Handling animations
    public void setCurrentAnimation(string animation)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator)
        {
            // cancelamos todas las demás
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                animator.SetBool(parameter.name, false);
            }
            Debug.Log(name + ": Poniendo la animación " + animation);
            animator.SetBool(animation, true);
        }
            
        else
            Debug.Log("No animator found for unit " + name);
    }

    // Funcion princiapl
    public void Update()
    {
        updateFrames++;
        //if (updateFrames == ConfigController.combatUpdateFrames)
        //{
        OldupdateUnit(updateFrames);
        updateFrames = 0;
        //}
    }

    public void OldupdateUnit(int frameOffset)
    {

        // Si estamos muertos, desaparecemos
        if (combatStatus == CombatStatus.COMBAT_STATUS_DEAD)
        {
            Debug.Log(this.gameObject.name + "Ha muerto");
            // Animación de muerte y tras X tiempo desaparecer
            handleDeath();
            return;
        }

        // Manejamos el tiempo de los cadáveres
        if(combatStatus == CombatStatus.COMBAT_STATUS_CORPSE)
        {
            corpseDuration-= frameOffset;
            if (corpseDuration <= 0)
            {
                gameObject.SetActive(false);
                Debug.Log("Se ha desactivado + " + name);
            }
                
            return;
        }

        // Si somos la caravana, no hacemos nada más
        if (type == CreatureType.CREATURE_TYPE_MISC)
            return;
        
        // Si estamos en combate, comprobamos como va
        if(inCombat)
        {
            // Actualizamos nuestro timer
            updateAttackStatus(frameOffset);

            if (!target)
                return;

            if (combatStatus == CombatStatus.COMBAT_STATUS_ATTACKING)
                return;

            // Si nuestro target está muerto, cambiamos al más cercano
            if (target.getCombatStatus() == CombatStatus.COMBAT_STATUS_DEAD ||
                target.getCombatStatus() == CombatStatus.COMBAT_STATUS_CORPSE)
            {
                handleUnitDeath(target);
                handleLosingTarget();
            }

            // Si estamos dentro del rango nos detenemos
            if (isInAttackRangeWith(target))
            {
                if (!isPlayer())
                    stopChasing();

                // Si nuestro ataque está listo, atacamos
                if (combatStatus == CombatStatus.COMBAT_STATUS_ATTACK_READY && !player)
                    combatStatus = CombatStatus.COMBAT_STATUS_ATTACKING;
            }
            // Si no lo estamos, perseguimos al target
            else if (!isPlayer())
                chaseUnit(target);
        }
    }

    public void updateUnit(int frameOffset)
    {
        // Si estamos muertos, desaparecemos
        if (combatStatus == CombatStatus.COMBAT_STATUS_DEAD)
        {
            Debug.Log(this.gameObject.name + "Ha muerto");
            // Animación de muerte y tras X tiempo desaparecer
            gameObject.SetActive(false);
            return;
        }







    }
}
