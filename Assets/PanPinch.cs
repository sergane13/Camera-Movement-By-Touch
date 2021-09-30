using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CameraActions
{
    public class PanPinch : MonoBehaviour
    {
        #region "Input data"
        [SerializeField]
        [Header("Camera that is child of the main camera")]
        private GameObject _camera;

        [SerializeField]
        [Header("X Inferior limit")]
        private float _limitXMin;

        [SerializeField]
        [Header("X Superior limit")]
        private float _limitXMax;

        [SerializeField]
        [Header("Y Inferior limit")]
        private float limitYMin;

        [SerializeField]
        [Header("Y Superior limit")]
        private float limitYMax;

        [SerializeField]
        [Header("Minimum orthographic size")]
        private float orthoMin;

        [SerializeField]
        [Header("Maximum orthographic size")]
        private float orthoMax;
        #endregion

        private float _panSpeed = 0.005f;
        private float _resolutionRatio;

        private Vector2 _zoomTarget;

        private bool _lastFramePinch = false;


        private void Awake()
        {
            _resolutionRatio = Screen.width / Screen.height;
        }

        private void Update()
        {
            Panning();
            Pinching();
        }


        /// <summary>
        /// 
        /// Panning method that requires one finger to move the camera
        /// 
        /// </summary>
        private void Panning()
        {            
            {
                if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;                  
                    PanningFunction(touchDeltaPosition);
                }
            }
        }


        /// <summary>
        /// 
        ///  ...
        /// 
        /// </summary>
        private void Pinching()
        {
            if (Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                
                if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
                {

                    Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    
                    float prevDist =  Vector2.Distance ( touchZero.position - touchZero.deltaPosition , touchOne.position - touchOne.deltaPosition);

                    float dist = Vector2.Distance (touchZero.position, touchOne.position);

                    PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 2);

                    Camera.main.orthographicSize *= prevDist/dist;

                    if(!_lastFramePinch)
                    {
                        _zoomTarget = Camera.main.ScreenToWorldPoint((touchZero.position + touchOne.position)/2);
                    }

                    if(dist / prevDist > 1f)
                    {

                        PanningFunction(-(((Vector2)Camera.main.WorldToScreenPoint(_zoomTarget))-screenCenter) * Time.deltaTime);
                    }

                    _lastFramePinch = true;
                }                
            }
            else
            {
                _lastFramePinch = false;
            }
        }


        private void PanningFunction(Vector2 touchDeltaPosition)
        {
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 1f);
            Vector3 screenTouch = screenCenter + new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f);

            Vector3 worldCenterPosition = Camera.main.ScreenToWorldPoint(screenCenter);
            Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouch);

            Vector3 worldDeltaPosition = worldTouchPosition - worldCenterPosition;

                       
            transform.Translate(-worldDeltaPosition);

            // float x = Mathf.Clamp(transform.position.x, _limitXMin + Camera.main.orthographicSize * resolutionRatio, _limitXMax - Camera.main.orthographicSize * resolutionRatio);
            // float y = Mathf.Clamp(transform.position.y, limitYMin + Camera.main.orthographicSize, limitYMax - Camera.main.orthographicSize);

            // transform.position = new Vector3(x, y, -10f);
        }
    }
}