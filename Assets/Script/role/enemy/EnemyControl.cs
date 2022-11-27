using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;

    EnemyArcher[] archers;
    EnemyShield[] shileders;

    List<LivingEntity> allEnemy = new List<LivingEntity>();

    private static EnemyControl instance;
    public static EnemyControl Instance { get { return instance; } }

    public List<LivingEntity> GetEnmey()
    {
        return allEnemy;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        archers = FindObjectsOfType<EnemyArcher>();
        shileders = FindObjectsOfType<EnemyShield>();
        allEnemy.AddRange(archers);
        allEnemy.AddRange(shileders);
    }

    public void LetArcherShoot()
    {
        float angle = 40f;
        float radian = Mathf.Deg2Rad * angle;
        float powerPercent = 1f;
        float speed = GameManagers.Instance.config.maxSpeed * powerPercent;
        float xSpeed = angle == 0 ? speed : -Mathf.Abs(speed * Mathf.Cos(radian));
        float ySpeed = angle == 0 ? 0 : speed * Mathf.Sin(radian);

        AimParam aim = new AimParam();
        aim.startAngle = angle;
        aim.startPowerPecent = powerPercent;
        aim.startXspeed = xSpeed;
        aim.startYspeed = ySpeed;

        foreach (EnemyArcher a in archers)
        {
            a.OnShoot(aim, targetMask);
        }
    }
}
