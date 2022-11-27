using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;

    PlayerArcher[] archers;

    private void Start()
    {
        PlayerController.OnTouchRealese += OnShoot;
        PropManager.OnBombSelect += OnBombSelect;
        archers = FindObjectsOfType<PlayerArcher>();
    }

    private void OnBombSelect(Projectile prop)
    {
        foreach (PlayerArcher archer in archers)
        {
            archer.SetOnceProjectile(prop);
        }
    }

    private void OnShoot(AimParam aimParam)
    {
        foreach (PlayerArcher ar in archers)
        {
            ar.OnShoot(aimParam, targetMask);
        }
    }
}
