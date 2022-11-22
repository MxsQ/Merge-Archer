using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class Projectile : MonoBehaviour
{
    //[SerializeField] LayerMask groundMask;
    LayerMask targetMask;
    int groundMask = 1 << 9;

    AimParam param;
    float shootTime;
    float gravity;
    Vector3 startPs;
    bool flying = false;

    public void Shoot(AimParam _param, LayerMask _targetMask)
    {
        targetMask = _targetMask;
        param = _param;
        shootTime = Time.time;
        gravity = GameManagers.Instance.config.gravity;
        startPs = transform.position;
        flying = true;
        Debug.Log("ground mask=" + groundMask);
    }


    private void Update()
    {
        if (!flying)
        {
            return;
        }

        if (transform.position.y < -1)
        {
            flying = false;
            return;
        }

        // keey move
        var detTime = Time.time - shootTime;
        var xOffset = param.startXspeed * detTime;
        var yOffest = param.startYspeed * detTime + (-gravity) * detTime * detTime / 2;
        Vector3 newPosition = new Vector3(startPs.x + xOffset, startPs.y + yOffest, startPs.z);
        transform.position = newPosition;

        var ySpeed = param.startYspeed + detTime * (-gravity);
        var rotateAnlge = Mathf.Atan(ySpeed / param.startXspeed) / Mathf.PI * 180;
        transform.rotation = Quaternion.Euler(0, 0, rotateAnlge);

        // object positon and desV contribute the flying direction
        Vector3 desV = Quaternion.Euler(0, 0, rotateAnlge) * Vector3.right * 1f + transform.position;
        //Debug.DrawLine(transform.position, desV, Color.red);
        CheckCollisions(desV);
    }

    private void CheckCollisions(Vector3 rangeDes)
    {
        var hitDir = (rangeDes - transform.position).normalized;
        Ray ray = new Ray(transform.position, hitDir);
        //Debug.DrawRay(transform.position, hitDir * 2f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.25f, targetMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit");
            flying = false;
            transform.parent = hit.collider.gameObject.transform;
            var hitAngle = Mathf.Atan(hitDir.y / hitDir.x) / Mathf.PI * 180;
            ProjectileData data = new ProjectileData(hitDir, hitAngle, 1);
            hit.collider.gameObject.GetComponent<LivingEntity>().OnHit(data);
            return;
        }
        else if (Physics.Raycast(ray, out hit, 0.3f, groundMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit ground");
            flying = false;
        }

    }

    public class ProjectileData
    {
        public Vector3 hitDir;
        public float hitAngle;
        public int damage;

        public ProjectileData(Vector3 _hitDir, float _hitAngle, int _damage)
        {
            hitDir = _hitDir;
            hitAngle = _hitAngle;
            damage = _damage;
        }
    }
}
