using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public abstract class Projectile : MonoBehaviour
{
    //[SerializeField] LayerMask groundMask;
    LayerMask targetMask;
    int groundMask = 1 << 9;
    int shieldMask = 1 << 10;

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
        if (param.startXspeed < 0)
        {
            rotateAnlge += 180;
        }
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
        if (DealwithTarget() && Physics.Raycast(ray, out hit, 0.25f, targetMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit enemy");
            ProjectileData data = PreppareHitData(hitDir);
            LivingEntity entity = hit.collider.gameObject.GetComponent<LivingEntity>();
            if (entity == null)
            {
                entity = hit.collider.gameObject.GetComponentInChildren<LivingEntity>();
            }
            OnCollideTarget(hit, data, entity);
            return;
        }
        else if (DealwithShiled() && Physics.Raycast(ray, out hit, 0.25f, shieldMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit shiled");
            ProjectileData data = PreppareHitData(hitDir);
            OnCollideShiled(hit, data, hit.collider.gameObject.GetComponent<Sheild>().host);
        }
        else if (DealwithGround() && Physics.Raycast(ray, out hit, 0.3f, groundMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit ground");
            OnCollideGround(hit, PreppareHitData(hitDir));
        }
    }

    private ProjectileData PreppareHitData(Vector3 hitDir)
    {
        flying = false;
        var hitAngle = Mathf.Atan(hitDir.y / hitDir.x) / Mathf.PI * 180;
        ProjectileData data = new ProjectileData(hitDir, hitAngle, 1);
        return data;
    }

    protected abstract bool DealwithTarget();
    protected abstract bool DealwithGround();
    protected abstract bool DealwithShiled();

    // only override when want to dealwith sitiation is collide to target;
    protected virtual void OnCollideTarget(RaycastHit hit, ProjectileData projectileData, LivingEntity entity) { }

    // only override when want to dealwith sitiation is collide to ground
    protected virtual void OnCollideGround(RaycastHit hit, ProjectileData projectileData) { }

    // only override when want to dealwith sitiation is collide to shield
    protected virtual void OnCollideShiled(RaycastHit hit, ProjectileData projectileData, LivingEntity entity) { }

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
