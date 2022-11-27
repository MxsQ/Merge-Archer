using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class Archer : LivingEntity
{
    [SerializeField] public Projectile arrow;
    [SerializeField] public Transform spawn;
    [SerializeField] public int dps = 1;
    [SerializeField] public int perProtileDps = 1;

    Projectile curProjectile;

    int protileCountOnceShoot;
    int curShotCount;
    AimParam aimParam;

    LayerMask targetMask;

    protected override void OnStart()
    {
        curProjectile = arrow;
    }

    public void SetOnceProjectile(Projectile projectile)
    {
        curProjectile = projectile;
    }

    public void OnShoot(AimParam param, LayerMask _targetMask)
    {
        protileCountOnceShoot = dps / perProtileDps;
        curShotCount = 0;
        targetMask = _targetMask;
        aimParam = param;
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        while (curShotCount < protileCountOnceShoot)
        {
            curShotCount++;
            Projectile pj = Instantiate(curProjectile, spawn.position, spawn.rotation, spawn)
                       .GetComponent<Projectile>();

            pj.Shoot(aimParam, targetMask);

            yield return new WaitForSeconds(0.2f);
        }
        curProjectile = arrow;
    }
}
