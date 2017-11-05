using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Unit colliderOwner = gameObject.GetComponentInParent<Unit>();
        Unit victim = other.gameObject.GetComponent<Unit>();

        colliderOwner.addUnitInAttackRange(victim);
    }

    private void OnTriggerExit(Collider other)
    {
        Unit colliderOwner = gameObject.GetComponentInParent<Unit>();
        Unit victim = other.gameObject.GetComponent<Unit>();

        colliderOwner.removeUnitInAttackRange(victim);
    }
}