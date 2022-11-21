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
        public float maxPowerDistance = 100;
        public float maxSpeed = 30;
        public float gravity = 5;
        public float aimDotShowSpanTime = .2f;
    }
}
