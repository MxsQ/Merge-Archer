using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers : MonoBehaviour
{

    private static GameManagers instance;
    public static GameManagers Instance { get { return instance; } }

    [SerializeField] public GlobalConfig config;

    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class GlobalConfig
    {
        [Header("Projectile")]
        public float maxPowerDistance = 100;
        public float maxSpeed = 30;
        public float gravity = 5;
        public float aimDotShowSpanTime = .2f;

        [Header("GameShow")]
        public float forceWhenDeath = 0.5f;
    }
}
