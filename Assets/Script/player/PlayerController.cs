using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    TOUCH_STATE curTouchState = TOUCH_STATE.IDLE;

    public static Action<AimParam> OnAim; // angel and power
    public static Action OnStartAnim;
    public static Action<AimParam> OnTouchRealese;

    private Vector2 touchPs = Vector2.zero;
    private Vector2 movePs = Vector2.zero;

    AimParam flyParam = new AimParam();

    void Update()
    {
        var touchPosition = GetTouchScreenPosition();

        if (curTouchState == TOUCH_STATE.START)
        {
            touchPs = touchPosition;
            OnStartAnim.Invoke();
        }
        else if (curTouchState == TOUCH_STATE.MOVE)
        {
            movePs = touchPosition;
            GetAimParam(ref flyParam);
            OnAim?.Invoke(flyParam);
        }
        else if (curTouchState == TOUCH_STATE.RELEASE)
        {
            movePs = touchPosition;
            GetAimParam(ref flyParam);
            OnTouchRealese?.Invoke(flyParam);

            touchPs = Vector2.zero;
            movePs = Vector2.zero;
            curTouchState = TOUCH_STATE.IDLE;
        }
    }

    //private void 

    private void GetAimParam(ref AimParam param)
    {

        // Get aim angel
        float _angle = Vector2.Angle(movePs - touchPs, Vector2.right);
        if (movePs.x > touchPs.x)
        {
            _angle = 0;
        }
        else
        {
            if (movePs.y < touchPs.y)
            {
                // right and down
                _angle = 180 - _angle;
            }
            else
            {
                // right and up
                _angle = -(180 - _angle);

            }
        }
        _angle = _angle > 80 ? 80 : _angle;
        _angle = _angle < -80 ? -80 : _angle;
        //return _angle;
        param.startAngle = _angle;

        // Get aim power percent
        var distance = (touchPs - movePs).magnitude;
        var distancePercent = distance / GameManagers.Instance.config.maxPowerDistance;
        distancePercent = distancePercent > 1 ? 1 : distancePercent;
        param.startPowerPecent = distancePercent;

        // Get X and Y speed
        float speed = distancePercent * GameManagers.Instance.config.maxSpeed;
        float gravity = GameManagers.Instance.config.gravity;
        float radian = Mathf.Deg2Rad * _angle;
        float xSpeed = _angle == 0 ? speed : Mathf.Abs(speed * Mathf.Cos(radian));
        float yStartSpeed = _angle == 0 ? 0 : speed * Mathf.Sin(radian);
        param.startXspeed = xSpeed;
        param.startYspeed = yStartSpeed;
    }


    Vector2 GetTouchScreenPosition()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            curTouchState = TOUCH_STATE.START;
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            curTouchState = TOUCH_STATE.RELEASE;
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButton(0))
        {
            curTouchState = TOUCH_STATE.MOVE;
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
#endif

        if (Input.touches.Length > 0)
        {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                curTouchState = TOUCH_STATE.START;
                return touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                curTouchState = TOUCH_STATE.MOVE;
                return touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                curTouchState = TOUCH_STATE.RELEASE;
                return touch.position;
            }
        }

        return Vector3.zero;
    }


    public enum TOUCH_STATE
    {
        IDLE,
        START,
        MOVE,
        RELEASE
    }

    public class AimParam
    {
        public float startAngle;
        public float startPowerPecent;
        public float startXspeed;
        public float startYspeed;
    }
}
