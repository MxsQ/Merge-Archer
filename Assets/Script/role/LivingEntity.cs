using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;


public class LivingEntity : MonoBehaviour
{
    [SerializeField] public GameObject projectile;
    [SerializeField] public Transform spawn;

    private void Start()
    {
        PlayerController.OnTouchRealese += OnShoot;
    }

    private void OnShoot(FlyParam param)
    {
        Projectile pj = Instantiate(projectile, spawn.position, spawn.rotation, spawn)
            .GetComponent<Projectile>();

        pj.Shoot(param);
    }
}
