using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace CameraActions
{
    public class PanPinchCameraMovement : MonoBehaviour
    {
        #region "Input data" 
        [SerializeField]
        [Header("Camera to move")]
        private Camera _cameraToMove;

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
        private float orthoMin = 2f;

        [SerializeField]
        [Header("Maximum orthographic size")]
        private float orthoMax = 12f;

        #endregion

        #region "Private members"

        private Vector3 initPos;
        private Vector2 zoomTarget;

        private bool _lastFramePinch = false;

        private float initDist = 42f;
        private float initOrtho = 6;

        private bool _initTouch = false;
        private float resolutionRatio;

        #endregion

        private void Awake()
        {
            resolutionRatio = Screen.width / Screen.height;
        }


        private void Update()
        {
            Panning();
            Pinching();
        }


        /// <summary>
        /// Panning that is used to move the camera [ignores UI elements]
        /// </summary>
        private void Panning()
        {
            // One finger on the screen [Pan]
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                bool check = false;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if(EventSystem.current.IsPointerOverGameObject(i))
                    {
                        check = true;
                        break;
                    }
                }

                if(check == false)
                {
                    _initTouch = false;
                }   
            }


            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _initTouch = true;
            }


            if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved && _initTouch == false)
            {                   
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;                    
                PanningFunction(touchDeltaPosition);
            }          
        }


        /// <summary>
        /// Pinching that is used for zooming with 2 or more fingers
        /// </summary>
        private void Pinching()
        {      
            if (Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                
                if(!_lastFramePinch)
                {
                    zoomTarget = _cameraToMove.ScreenToWorldPoint((touchZero.position + touchOne.position) / 2);
                    initPos = _cameraToMove.transform.position;
                    initDist = Vector2.Distance(touchZero.position, touchOne.position);
                    initOrtho = _cameraToMove.orthographicSize;                        
                }

                if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
                {
                    Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    
                    float prevDist =  Vector2.Distance ( touchZero.position - touchZero.deltaPosition , touchOne.position - touchOne.deltaPosition);
                    float dist = Vector2.Distance (touchZero.position, touchOne.position);

                    PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 2);

                    _cameraToMove.orthographicSize = Mathf.Clamp (_cameraToMove.orthographicSize* (prevDist/dist), orthoMin, orthoMax);

                    float t;
                    float x = _cameraToMove.orthographicSize;

                    if(initOrtho != orthoMin)
                    {
                        float a = -( 1 / ((initOrtho - orthoMin)));
                        float b = 1 + (orthoMin / ((initOrtho - orthoMin)));
                        t = Mathf.Clamp(a * x + b, 0f, 1f);
                        
                        _cameraToMove.transform.position = Vector3.Lerp(initPos, new Vector3(zoomTarget.x, zoomTarget.y, _cameraToMove.transform.position.z), t);
                    }
                }
                    
                _lastFramePinch = true;
                Vector3 prevTarg = ((touchZero.position - touchZero.deltaPosition) + (touchOne.position - touchOne.deltaPosition)) / 2;
                Vector3 targ = (touchZero.position + touchOne.position) / 2;

                zoomTarget = _cameraToMove.ScreenToWorldPoint(_cameraToMove.WorldToScreenPoint(zoomTarget) - (targ - prevTarg));
                initPos = _cameraToMove.ScreenToWorldPoint(_cameraToMove.WorldToScreenPoint(initPos) - (targ - prevTarg));
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

            Vector3 worldCenterPosition = _cameraToMove.ScreenToWorldPoint(screenCenter);
            Vector3 worldTouchPosition = _cameraToMove.ScreenToWorldPoint(screenTouch);

            Vector3 worldDeltaPosition = worldTouchPosition - worldCenterPosition;
          
            transform.Translate(-worldDeltaPosition);

            float x = Mathf.Clamp(transform.position.x, _limitXMin + _cameraToMove.orthographicSize * resolutionRatio, _limitXMax - _cameraToMove.orthographicSize * resolutionRatio);
            float y = Mathf.Clamp(transform.position.y, limitYMin + _cameraToMove.orthographicSize, limitYMax - _cameraToMove.orthographicSize);

            transform.position = new Vector3(x, y, -10f);
        }
    }
}