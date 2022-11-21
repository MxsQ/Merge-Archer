using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class Projectile : MonoBehaviour
{
    FlyParam param;
    float shootTime;
    float gravity;
    Vector3 startPs;

    public void Shoot(FlyParam _param)
    {
        param = _param;
        shootTime = Time.time;
        gravity = GameManagers.Instance.config.gravity;
        startPs = transform.position;
    }


    private void Update()
    {
        if (param == null)
        {
            return;
        }

        if (transform.position.y < -1)
        {
            param = null;
            return;
        }

        var detTime = Time.time - shootTime;
        var xOffset = param.startXspeed * detTime;
        var yOffest = param.startYspeed * detTime + (-gravity) * detTime * detTime / 2;
        Vector3 newPosition = new Vector3(startPs.x + xOffset, startPs.y + yOffest, startPs.z);
        transform.position = newPosition;

        var ySpeed = param.startYspeed + detTime * (-gravity);
        var rotateAnlge = Mathf.Atan(ySpeed / param.startXspeed) / Mathf.PI * 180;
        transform.rotation = Quaternion.Euler(0, 0, rotateAnlge);
    }
}
