using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;
using static Projectile;

public abstract class LivingEntity : MonoBehaviour
{
    [SerializeField] public int health = 1;

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart() { }

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
        //var hitDir = projectileData.hitDir.normalized;
        //Vector3 power = new Vector3(hitDir.x, -hitDir.y, hitDir.z);
        //Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        //rigidbody.useGravity = true;
        //rigidbody.AddForce(power * GameManagers.Instance.config.forceWhenDeath, ForceMode.Impulse);

        var renders = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renders)
        {
            r.material.color = Color.gray;
        }

        Debug.Log("die");
        // no longer recive collider check
        var colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }


    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("bomb");
    }

    public bool Death()
    {
        return health <= 0;
    }
}
