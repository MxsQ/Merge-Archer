using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;
using static Projectile;

public class LivingEntity : MonoBehaviour
{
    [SerializeField] public int health = 1;

    public void OnHit(ProjectileData projectileData)
    {
        health -= projectileData.damage;
        if (health <= 0)
        {
            OnDeath(projectileData);
        }
    }

    protected void OnDeath(ProjectileData projectileData)
    {

        // give a pop up power
        var hitDir = projectileData.hitDir;
        Vector3 nagativeDire = Quaternion.Euler(0, 0, projectileData.hitAngle * -2) * projectileData.hitDir;
        //Debug.Log("ange=" + angle + " oDir=" + hitDirection + "  nDir=" + nagativeDire);

        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(nagativeDire * GameManagers.Instance.config.forceWhenDeath, ForceMode.Impulse);

        // no longer recive collider check
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

}
