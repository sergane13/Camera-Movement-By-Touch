using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CameraActions
{
    public class PanPinch : MonoBehaviour
    {
        [SerializeField]
        private Text text;
        [SerializeField]
        [Header("Camera that is child of the main camera")]
        private GameObject _camera;
        private Camera _lineCamera;

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

        [SerializeField]
        [Header("Threshold value for the the deltaPosition of the touches for the minimap to be activated")]
        private int _minimapActivationSensibility = 120;
        [SerializeField]
        private float panSpeed = 0.05f;
        private Vector2 zoomTarget;
        private bool _touchingRobot = false;
        private bool _minimapActivated = false;
        private bool _lastFramePinch = false;
        float initDist = 42f;
        float initOrtho = 6;
        private Vector3 initPos;
        private float resolutionRatio;
        //private Vector2 scalingFactor;

        // Raised when the orthographic size of the camera is changed
        // Used by other cameras (Children of main camera) to adjust their orthographic size
        public static event Action OnChangingOrto;

        // If orto is > than maxOrto, activiate the minimap view
        public static event Action<bool> OnMinimapActivation;

        private void Awake()
        {
            resolutionRatio = Screen.width / Screen.height;
            Debug.Log(Screen.height);
            Debug.Log(Screen.width);
            
        }
        private void Update(){
            if (_touchingRobot == false)
            {
                Panning();
                Pinching();
            }
            else
            {
                _touchingRobot = false;
            }
        }

        private void Panning()
        {
            // One finger on the screen [Pan]
            
            {
                if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved && _minimapActivated == false)
                {                   
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    Debug.Log(touchDeltaPosition);
                    
                    PanningFunction(touchDeltaPosition);
                }
            }
        }

        private void Pinching()
        {
           
            
            if (Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                
                if(!_lastFramePinch){
                    zoomTarget = Camera.main.ScreenToWorldPoint((touchZero.position + touchOne.position)/2);
                    initPos = Camera.main.transform.position;
                    initDist = Vector2.Distance(touchZero.position, touchOne.position);
                    initOrtho = Camera.main.orthographicSize;                        
                }

                if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved){

                    Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    
                    float prevDist =  Vector2.Distance ( touchZero.position - touchZero.deltaPosition , touchOne.position - touchOne.deltaPosition);
                    float dist = Vector2.Distance (touchZero.position, touchOne.position);
                    PanningFunction(touchZero.deltaPosition + touchOne.deltaPosition);
                    Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize* (prevDist/dist), orthoMin, orthoMax);
                    float t;
                    float x = Camera.main.orthographicSize;

                    if(initOrtho != orthoMin){
                        float a = -(1/((initOrtho - orthoMin)));
                        float b = 1 + (orthoMin/((initOrtho - orthoMin)));
                        t = Mathf.Clamp( a * x + b, 0f, 1f);
                        
                        Camera.main.transform.position = Vector3.Lerp(initPos, new Vector3(zoomTarget.x, zoomTarget.y, Camera.main.transform.position.z),t);
                    }
                }
                    
                _lastFramePinch = true;
                Vector3 prevTarg = ((touchZero.position - touchZero.deltaPosition) + (touchOne.position - touchOne.deltaPosition))/2;
                Vector3 targ = (touchZero.position + touchOne.position)/2;
                zoomTarget = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(zoomTarget) - (targ - prevTarg));
                initPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(initPos) - (targ - prevTarg));
                Debug.Log(zoomTarget);
                    

            }
            else{
                _lastFramePinch = false;
            }
            
        }

        // Check if user pinch fast when ortho size is at the maxium value
        private void CheckIfMinimapIsActivated(ref Touch touchZero, ref Touch touchOne)
        {
            if (Mathf.Abs((touchZero.deltaPosition - touchOne.deltaPosition).x) > _minimapActivationSensibility && Camera.main.orthographicSize > orthoMax - 1)
            {
                if (UserTouch.TouchPhaseEnded(0))
                {
                    _minimapActivated = !_minimapActivated;
                    OnMinimapActivation?.Invoke(_minimapActivated);
                }
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

        private void SetTouchingRobot()
        {
            _touchingRobot = true;
        }
    }
}