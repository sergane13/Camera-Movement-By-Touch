using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace CameraActions
{
    public class TapOnGameObject : MonoBehaviour
    {
        #region "Input data"
        [SerializeField] private float _sensitivity;
        #endregion

        #region "Private members"
        private static Collider2D s_firstColliderTouched;

        private static bool s_first = false;
        private static bool s_second = false;

        private static bool s_hasMoved = false;
        #endregion

        void Update()
        {
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(0))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        s_firstColliderTouched = hit.collider;
                        s_first = true;
                        s_second = false;
                    }


                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        if (Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > _sensitivity || Mathf.Abs(Input.GetTouch(0).deltaPosition.y) > _sensitivity)
                        {
                            s_hasMoved = true;
                        }
                    }


                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        if (s_firstColliderTouched == hit.collider && s_hasMoved == false)
                        {
                            s_second = true;
                        }
                        else
                        {
                            s_first = false;
                        }

                        s_hasMoved = false;
                    }


                    if (s_first && s_second)
                    {
                        //##################################################################################
                        //if (Your stuff != null)
                        //{
                        //    Execute what you want here. Here is the confirmation for the tap
                        //}
                        //##################################################################################

                        s_first = false;
                        s_second = false;
                    }
                }
            }
        }
    }
}
