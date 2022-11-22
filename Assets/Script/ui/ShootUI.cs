using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class ShootUI : MonoBehaviour
{
    [SerializeField] public Text angleText;
    [SerializeField] public Text powerText;

    [SerializeField] public Transform dotAnimStart;
    [SerializeField] public Transform dotHost;
    [SerializeField] public GameObject dot;

    private GameObject[] dots = new GameObject[6];
    bool inAim;
    AimParam flyParam;
    Vector2 touchPs = Vector2.zero;

    private void Start()
    {
        PlayerController.OnStartAnim += OnStartAim;
        PlayerController.OnAim += OnAim;
        PlayerController.OnTouchRealese += OnTouchRealse;

        for (int i = 0; i < dots.Length; i++)
        {
            GameObject _dot = Instantiate(dot, Vector3.zero, transform.rotation, transform);
            _dot.SetActive(false);
            dots[i] = _dot;
        }
    }

    private void OnStartAim()
    {
        inAim = true;
        foreach (GameObject dot in dots)
        {
            dot.SetActive(true);
        }
        StartCoroutine("AimAnimation");
    }

    private void OnTouchRealse(AimParam _flyParam)
    {
        inAim = false;
        foreach (GameObject dot in dots)
        {
            dot.SetActive(false);
        }
        ClearDot();
        StopCoroutine("AimAnimation");
    }

    private void OnAim(AimParam _flyParam)
    {
        flyParam = _flyParam;
        inAim = true;
        angleText.text = flyParam.startAngle.ToString("#0") + "°";
        powerText.text = (flyParam.startPowerPecent * 100).ToString("#0.0") + "%";
    }

    private void ClearDot()
    {
        foreach (GameObject o in dots)
        {
            o.transform.position = Vector3.zero;
        }
    }

    private IEnumerator AimAnimation()
    {
        float gravity = GameManagers.Instance.config.gravity;
        float spanTime = GameManagers.Instance.config.aimDotShowSpanTime;

        while (inAim)
        {
            yield return new WaitForSeconds(0.05f);

            ClearDot();

            float xSpeed = Mathf.Abs(flyParam.startXspeed);
            float yStartSpeed = flyParam.startYspeed;
            //Debug.Log("Angel= " + aimAnagel + " sinSpeed= " + (speed * Mathf.Sin(aimAnagel)) + "  xSpeed=" + xSpeed + "  ySpeed=" + yStartSpeed);

            for (int i = 0; i < dots.Length; i++)
            {
                float time = spanTime * i;
                float x = time * xSpeed + dotAnimStart.position.x;
                float y = yStartSpeed * time + (-gravity) * time * time / 2 + dotAnimStart.position.y;
                GameObject theDot = dots[i];
                theDot.transform.position = new Vector3(x, y);
            }

        }
    }
}
