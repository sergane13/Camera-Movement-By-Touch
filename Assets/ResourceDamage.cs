using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDamage : MonoBehaviour
{
    public int damage = 5;

    public bool Mine(Collider2D id)
    {
        if (id != null && id.GetComponent<ResourceHealth>() != null)
            return id.GetComponent<ResourceHealth>().HealthDecrease(damage, id.GetInstanceID());

        return false;
    }
}