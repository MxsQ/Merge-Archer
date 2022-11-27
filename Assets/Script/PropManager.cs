using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [SerializeField] public Projectile Bomb;

    public static Action<Projectile> OnBombSelect;


    private static PropManager instance;
    public static PropManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
    }

    public void OnBombClick()
    {
        OnBombSelect?.Invoke(Bomb);
        StartCoroutine(DelayWork(() =>
        {
            PlayerController.Instance.playerTurn = true;
        }, 0.5f));
    }

    public void OnArrowClick()
    {
        StartCoroutine(DelayWork(() =>
        {
            PlayerController.Instance.playerTurn = true;
        }, 0.5f));
    }

    IEnumerator DelayWork(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
