using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcher : MonoBehaviour
{
    [SerializeField] Archer[] archers;
    [SerializeField] LayerMask targetMask;

    private void Start()
    {
        PlayerController.OnTouchRealese += OnShoot;
        Debug.Log(targetMask.value);
    }

    private void OnShoot(PlayerController.AimParam aimParam)
    {
        foreach (Archer ar in archers)
        {
            ar.OnShoot(aimParam, targetMask);
        }
    }

}
