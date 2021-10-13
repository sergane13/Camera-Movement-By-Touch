using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

        #endregion

        private void Awake()
        {}

        private void Update()
        {
            Panning();
            Pinching();
        }

        private void Panning()
        {
            // One finger on the screen [Pan]
                    
            if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {                   
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                Debug.Log(touchDeltaPosition);
                    
                PanningFunction(touchDeltaPosition);
            }          
        }

        private void Pinching()
        {      
            if (Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                
                if(!_lastFramePinch)
                {
                    zoomTarget = Camera.main.ScreenToWorldPoint((touchZero.position + touchOne.position) / 2);
                    initPos = Camera.main.transform.position;
                    initDist = Vector2.Distance(touchZero.position, touchOne.position);
                    initOrtho = Camera.main.orthographicSize;                        
                }

                if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
                {
                    Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    
                    float prevDist =  Vector2.Distance ( touchZero.position - touchZero.deltaPosition , touchOne.position - touchOne.deltaPosition);
                    float dist = Vector2.Distance (touchZero.position, touchOne.position);

                    PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 2);

                    Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize* (prevDist/dist), orthoMin, orthoMax);

                    float t;
                    float x = Camera.main.orthographicSize;

                    if(initOrtho != orthoMin)
                    {
                        float a = -( 1 / ((initOrtho - orthoMin)));
                        float b = 1 + (orthoMin / ((initOrtho - orthoMin)));
                        t = Mathf.Clamp(a * x + b, 0f, 1f);
                        
                        Camera.main.transform.position = Vector3.Lerp(initPos, new Vector3(zoomTarget.x, zoomTarget.y, Camera.main.transform.position.z), t);
                    }
                }
                    
                _lastFramePinch = true;
                Vector3 prevTarg = ((touchZero.position - touchZero.deltaPosition) + (touchOne.position - touchOne.deltaPosition)) / 2;
                Vector3 targ = (touchZero.position + touchOne.position) / 2;

                zoomTarget = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(zoomTarget) - (targ - prevTarg));
                initPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(initPos) - (targ - prevTarg));
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

        }
    }
}