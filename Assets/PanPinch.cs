using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CameraActions
{
    public class PanPinch : MonoBehaviour
    {
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
        private float orthoMin;

        [SerializeField]
        [Header("Maximum orthographic size")]
        private float orthoMax;

        [SerializeField]
        [Header("Threshold value for the the deltaPosition of the touches for the minimap to be activated")]
        private int _minimapActivationSensibility = 120;
        [SerializeField]
        private float panSpeed = 0.005f;
        private Vector2 zoomTarget;
        private bool _touchingRobot = false;
        private bool _minimapActivated = false;
        private bool _lastFramePinch = false;
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
            //scalingFactor = new Vector2(Screen.width,Screen.height);
            Debug.Log(Screen.height);
            Debug.Log(Screen.width);
             //_lineCamera = _camera.GetComponent<Camera>();
        }
        private void FixedUpdate(){
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
        // private void Update()
        // {
        //     if (_touchingRobot == false)
        //     {
        //         Panning();
        //        // Pinching();
        //     }
        //     else
        //     {
        //         _touchingRobot = false;
        //     }
        // }

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
                
                if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved){

                    Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    

                    float prevDist =  Vector2.Distance ( touchZero.position - touchZero.deltaPosition , touchOne.position - touchOne.deltaPosition);
                    float dist = Vector2.Distance (touchZero.position, touchOne.position);
                    PanningFunction(touchZero.deltaPosition + touchOne.deltaPosition);
                    Camera.main.orthographicSize *= prevDist/dist;
                    if(!_lastFramePinch){
                        zoomTarget = Camera.main.ScreenToWorldPoint((touchZero.position + touchOne.position)/2);
                    }
                    if(dist/prevDist > 1f){

                        PanningFunction(-(((Vector2)Camera.main.WorldToScreenPoint(zoomTarget))-screenCenter)*panSpeed);
                    }
                    _lastFramePinch = true;
                }
                    ///PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 2);

                    // Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    // Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    // float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    // Camera.main.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
                    // _lineCamera.orthographicSize += deltaMagnitudeDiff * zoomSpeed;

                   // OnChangingOrto?.Invoke();

                    //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthoMin, orthoMax);
                   // _lineCamera.orthographicSize = Mathf.Clamp(_lineCamera.orthographicSize, orthoMin, orthoMax);
                
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
            //if (RectTransformUtility.RectangleContainsScreenPoint(UserTouch.touchArea, Input.GetTouch(0).position))
            //{
                
                Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 1f);
                Vector3 screenTouch = screenCenter + new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f);

                Vector3 worldCenterPosition = Camera.main.ScreenToWorldPoint(screenCenter);
                Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouch);

                Vector3 worldDeltaPosition = worldTouchPosition - worldCenterPosition;

                

          
                transform.Translate(-worldDeltaPosition);

                // Debug.Log(Screen.height);
                // Debug.Log(Screen.width);

                // float x = Mathf.Clamp(transform.position.x, _limitXMin + Camera.main.orthographicSize * resolutionRatio, _limitXMax - Camera.main.orthographicSize * resolutionRatio);
                // float y = Mathf.Clamp(transform.position.y, limitYMin + Camera.main.orthographicSize, limitYMax - Camera.main.orthographicSize);

                // transform.position = new Vector3(x, y, -10f);

           // }
        }

        private void SetTouchingRobot()
        {
            _touchingRobot = true;
        }
    }
}