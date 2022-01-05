using UnityEngine;
using UnityEngine.EventSystems;


namespace CameraActions
{
    public class PanPinchCameraMovement : MonoBehaviour
    {
        #region "Input data" 
        
        [Header("Camera to move")]
        [SerializeField] private Camera _cameraToMove;

        [Space(40f)]

        [Header("X Inferior limit")]
        [SerializeField] float _limitXMin;
      
        [Header("X Superior limit")]
        [SerializeField] private float _limitXMax;
       
        [Header("Y Inferior limit")]
        [SerializeField] private float _limitYMin;
      
        [Header("Y Superior limit")]
        [SerializeField] private float _limitYMax;

        [Space(40f)]

        [Header("Minimum orthographic size")]
        [SerializeField] private float _orthoMin = 2f;
       
        [Header("Maximum orthographic size")]
        [SerializeField] private float _orthoMax = 12f;


        [Space(40f)]
        [Header("Interpolation step for camera drag")]     
        [SerializeField]  private float _interpolationStep;
        #endregion

        #region "Private members"

        private Vector3 initPos;
        private Vector2 zoomTarget;

        private bool _lastFramePinch = false;

        private float initDist = 42f; // var for calculation [used in Pinching()]
        private float initOrtho = 6;  // var for calculation [used in Pinching()]

        private bool _initTouch = false; // if init touch is on UI element

        private Vector2 _panVelocity;  //delta position of the touch [camera position derivative]
        #endregion


        /// <summary> 
        /// Draw camera boundaries on editor
        /// </summary>
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(_limitXMin, _limitYMin), new Vector3(_limitXMin, _limitYMax));
            Gizmos.DrawLine(new Vector3(_limitXMin, _limitYMax), new Vector3(_limitXMax, _limitYMax));
            Gizmos.DrawLine(new Vector3(_limitXMax, _limitYMax), new Vector3(_limitXMax, _limitYMin));
            Gizmos.DrawLine(new Vector3(_limitXMax, _limitYMin), new Vector3(_limitXMin, _limitYMin));
        }
#endif


        private void Awake()
        {}


        private void Update()
        {
            CheckIfUiHasBeenTouched();

            // If there are no touches 
            if (Input.touchCount < 1)
            {
                _initTouch = true;
            }

            if (_initTouch == false)
            {
                Panning();
                Pinching();
            }
            else
            {
                PanningInertia();
                MinOrthoAchievedAnimation();
            }
        }


        /// <summary>
        /// Checks if one of the touches have started on a UI element 
        /// </summary>
        private void CheckIfUiHasBeenTouched()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                bool check = false;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(i)) // implementation for the old input system!!
                    {
                        check = true;
                        break;
                    }
                }

                if (check == false)
                {
                    _initTouch = false;
                }
            }
        }


        /// <summary>
        /// Panning that is used to move the camera [ignores UI elements]
        /// </summary>
        private void Panning()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                _panVelocity = touchDeltaPosition;
               
                PanningFunction(touchDeltaPosition);
            }
            else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                _panVelocity = Vector2.zero;
            }
        }


        /// <summary>
        /// Pinching that is used for zooming with 2 or more fingers
        /// </summary>
        private void Pinching()
        {
            if (Input.touchCount > 1)
            {
                _panVelocity = Vector2.zero;

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (!_lastFramePinch)
                {
                    zoomTarget = _cameraToMove.ScreenToWorldPoint((touchZero.position + touchOne.position) / 2);
                    initPos = _cameraToMove.transform.position;
                    initDist = Vector2.Distance(touchZero.position, touchOne.position);
                    initOrtho = _cameraToMove.orthographicSize;
                }

                if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
                {
                    float prevDist = Vector2.Distance(touchZero.position - touchZero.deltaPosition, touchOne.position - touchOne.deltaPosition);
                    float dist = Vector2.Distance(touchZero.position, touchOne.position);

                    PanningFunction((touchZero.deltaPosition + touchOne.deltaPosition) / 40);

                    _cameraToMove.orthographicSize = Mathf.Clamp(_cameraToMove.orthographicSize * (prevDist / dist), _orthoMin, _orthoMax);

                    float t;
                    float x = _cameraToMove.orthographicSize;

                    if (initOrtho != _orthoMin)
                    {
                        float a = -(1 / ((initOrtho - _orthoMin)));
                        float b = 1 + (_orthoMin / ((initOrtho - _orthoMin)));
                        t = Mathf.Clamp(a * x + b, 0f, 1f);

                        _cameraToMove.transform.position = Vector3.Lerp(initPos, new Vector3(zoomTarget.x, zoomTarget.y, _cameraToMove.transform.position.z), t);

                        LimitCameraMovement();
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


        /// <summary>
        ///  The method for panning the camera with one input deltaPosition
        ///  Has a little bit of lag from transform.Translate;
        /// </summary>
        /// <param name="touchDeltaPosition"> the delta position for movement </param>
        private void PanningFunction(Vector2 touchDeltaPosition)
        {          
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 1f);
            Vector3 screenTouch = screenCenter + new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f);

            Vector3 worldCenterPosition = _cameraToMove.ScreenToWorldPoint(screenCenter);
            Vector3 worldTouchPosition = _cameraToMove.ScreenToWorldPoint(screenTouch);

            Vector3 worldDeltaPosition = worldTouchPosition - worldCenterPosition;

            transform.Translate(-worldDeltaPosition);

            LimitCameraMovement();
        }


        /// <summary>
        /// Inertia of the camera when panning finishes 
        /// </summary>
        private void PanningInertia()
        {
            if (_panVelocity.magnitude < 0.02f)
            {
                _panVelocity = Vector2.zero;
            }

            if (_panVelocity != Vector2.zero)
            {             
                _panVelocity = Vector2.Lerp(_panVelocity, Vector2.zero, _interpolationStep);
                _cameraToMove.transform.localPosition += new Vector3(-_panVelocity.x / (500 * (1 / _cameraToMove.orthographicSize)), -_panVelocity.y / (500 * (1 / _cameraToMove.orthographicSize)), 0);
                LimitCameraMovement();
            }
        }


        /// <summary>
        /// Camera feedback when achieving minimum ortho
        /// </summary>
        private void MinOrthoAchievedAnimation()
        {           
            if (_cameraToMove.orthographicSize < _orthoMin + 0.6f)
            {
                _cameraToMove.orthographicSize = Mathf.Lerp(_cameraToMove.orthographicSize, _orthoMin + 0.6f, 0.06f);
                _cameraToMove.orthographicSize = Mathf.Round(_cameraToMove.orthographicSize * 1000.0f) * 0.001f;
                LimitCameraMovement();
            }
        }


        /// <summary>
        /// Limits Camera Movement into boundaries
        /// </summary>
        private void LimitCameraMovement()
        {
            float xCord = Mathf.Clamp(_cameraToMove.transform.position.x, _limitXMin + (_cameraToMove.orthographicSize * _cameraToMove.aspect), _limitXMax - (_cameraToMove.orthographicSize * _cameraToMove.aspect));
            float yCord = Mathf.Clamp(_cameraToMove.transform.position.y, _limitYMin + _cameraToMove.orthographicSize, _limitYMax - _cameraToMove.orthographicSize);

            transform.position = new Vector3(xCord, yCord, -10f);
        }
    }
}