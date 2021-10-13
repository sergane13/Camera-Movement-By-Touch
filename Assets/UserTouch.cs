using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTouch: MonoBehaviour
{
    [SerializeField]
    [Header("The touch Area where user touches are taken and used in script aka [Available touch area]")]
    private GameObject _touchArea;

    [SerializeField]
    [Header("The threshold value of deltaPosition so touch phase can be considered [Let some tolerances]")]
    private float _touchSensitivity; //10

    // Panel that represent the touch area
    public static RectTransform touchArea;
    public static float touchSensitivity;

    private void Awake()
    {
        touchArea = _touchArea.GetComponent<RectTransform>();
        touchSensitivity = _touchSensitivity;
    }

    // Position in world coordonates of the touch
    public static Vector3 TouchPosition(int touchNumber)
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(touchNumber);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            return touchPosition;
        }
        else
        {
            return new Vector3(-1000, -1000, -1000);
        }
    }

    public static Vector3 TouchPosition(int touchNumber, RectTransform bounds)
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(touchNumber);

            if (RectTransformUtility.RectangleContainsScreenPoint(bounds, touch.position))
            {
                return Camera.main.ScreenToWorldPoint(touch.position);
            }
        }

        return new Vector3(-1000, -1000, -1000);
    }

    public static Vector3Int TouchPositionInt(int touchNumber, RectTransform bounds)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(touchNumber);

            if (RectTransformUtility.RectangleContainsScreenPoint(bounds, touch.position))
            {
                return new Vector3Int((int)Camera.main.ScreenToWorldPoint(touch.position).x, (int)Camera.main.ScreenToWorldPoint(touch.position).y, 0);
            }
        }

        return new Vector3Int(-99999, -99999, -99999);
    }

    // Detecting colliders touched
    public static Collider2D DetectColliderTouched()
    {
        Vector3 temp = TouchPosition(0, touchArea);
        RaycastHit2D hitInfo = Physics2D.Raycast(temp, Vector2.zero);

        return hitInfo.collider;
    }

    public static Collider2D DetectColliderTouched(LayerMask layer)
    {
        Vector3 temp = TouchPosition(0, touchArea);
        RaycastHit2D hitInfo = Physics2D.Raycast(temp, Vector2.zero, 10000, layer);

        if (hitInfo.collider != null )
        {
            return hitInfo.collider;
        }
        else
        {
            return null;
        }
    }

    // Phases of touch
    public static bool TouchPhaseBegan(int touchNumber)
    {
        if(Input.touchCount > 0)
        {
            return Input.GetTouch(touchNumber).phase == TouchPhase.Began ? true : false;
        }
        return false;
    }

    public static bool TouchPhaseMoved(int touchNumber)
    {
        if (Input.touchCount > 0)
        {
            if(Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > touchSensitivity || Mathf.Abs(Input.GetTouch(0).deltaPosition.y) > touchSensitivity)
            {
                return Input.GetTouch(touchNumber).phase == TouchPhase.Moved ? true : false;
            }
         
        }
        return false;
    }

    public static bool TouchPhaseEnded(int touchNumber)
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(touchNumber).phase == TouchPhase.Ended ? true : false;
        }
        return false;
    }
}