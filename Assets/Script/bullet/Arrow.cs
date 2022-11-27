using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
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
        return true;
    }

    protected override void OnCollideTarget(RaycastHit hit, ProjectileData projectileData, LivingEntity entity)
    {
        //Vector3 ps = transform.position;
        //Vector3 scale = transform.localScale;
        //Vector3 originAngle = transform.eulerAngles;
        //Vector3 rotate = new Vector3(originAngle.x, originAngle.y, originAngle.z);

        //transform.parent = hit.collider.gameObject.transform;
        //transform.position = ps;
        //transform.localScale = scale;
        //transform.rotation = Quaternion.Euler(rotate);
        transform.parent = entity.gameObject.transform;
        entity.OnHit(projectileData);
    }

    protected override void OnCollideShiled(RaycastHit hit, ProjectileData projectileData, LivingEntity entity)
    {
        OnCollideTarget(hit, projectileData, entity);
    }
}
