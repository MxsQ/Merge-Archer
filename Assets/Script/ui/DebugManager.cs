using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public void LetEnemyShoot()
    {
        EnemyControl.Instance.LetArcherShoot();
    }
}
