using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    [SerializeField] GameObject BombLooklike;
    [SerializeField] ParticleSystem bombEffect;
    [SerializeField] float boomArea = 1f;

    protected override bool DealwithGround()
    {
        return true;
    }

    protected override bool DealwithShiled()
    {
        return true;
    }

    protected override bool DealwithTarget()
    {
        return false;
    }

    protected override void OnCollideGround(RaycastHit hit, ProjectileData projectileData)
    {
        Boom(hit, projectileData);
    }

    protected override void OnCollideShiled(RaycastHit hit, ProjectileData projectileData, LivingEntity entity)
    {
        Boom(hit, projectileData);
    }

    private void Boom(RaycastHit hit, ProjectileData projectileData)
    {
        BombLooklike.SetActive(false);

        ParticleSystem effect = Instantiate(bombEffect, hit.collider.gameObject.transform);
        Destroy(effect, effect.startLifetime);
        List<LivingEntity> enemy = EnemyControl.Instance.GetEnmey();
        float x = 0;
        float y = 0;
        float z = 0;
        foreach (LivingEntity entity in enemy)
        {
            if (entity.Death())
            {
                continue;
            }

            x = entity.gameObject.transform.position.x - gameObject.transform.position.x;
            y = entity.gameObject.transform.position.y - gameObject.transform.position.y;
            z = entity.gameObject.transform.position.z - gameObject.transform.position.z;
            var distanceJudge = x * x + y * y + z * z;
            var boomJudege = boomArea * boomArea;
            Debug.Log("distancePower=" + distanceJudge + "  bombPower=" + boomJudege);
            if (distanceJudge <= boomJudege)
            {
                entity.OnHit(projectileData);
            }
        }

        Destroy(gameObject, effect.startLifetime);
    }
}
