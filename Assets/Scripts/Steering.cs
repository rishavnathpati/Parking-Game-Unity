using System;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public float rotateSpeed = 10f; //Used only with desktop controls

    public GameObject playerCar;

    [SerializeField] private float velocityMag;

    private Camera _camera;

    private Vector2 _deltaPos;

    private bool _moveCar;

    private Rigidbody2D _playerCarRb;

    private bool _rotateBack = true;

    private float _screenWidth;

    private Vector2 _touchPosStart;

    private float _turnDirection;

    private void Awake()
    {
        _playerCarRb = playerCar.GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _screenWidth = Screen.width;
    }


    private void Update()
    {
        _moveCar = CarMoving();
        RotateBack(); //To rotate back car and steering when finger lifted from screen, ie not holding the steering
        UpdateCarRotation();

        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld when Input.touchCount <= 0:
                _rotateBack = true;
                return;
            case DeviceType.Handheld:
            {
                MobileInput();
                break;
            }

            #region DontCareCases

            case DeviceType.Desktop:
            {
                DesktopControls();

                break;
            }
            case DeviceType.Unknown:
                break;
            case DeviceType.Console:
                break;
            default:
                throw new ArgumentOutOfRangeException();

            #endregion
        }
    }

    private bool CarMoving() // To check if car is moving or not
    {
        velocityMag = _playerCarRb.velocity.magnitude;

        return velocityMag > .8f;
    }

    private void UpdateCarRotation() // Updating car rotation based on steering rotation, only if car moves
    {
        if (!_moveCar || transform.rotation.z == 0) return;

        if (_turnDirection < 0)
        {
            //to be implemented for accessing complete 360 to -360 degree rotation of steering, current state allows a range of 180 to -180
        }

        switch (PlayerCar.Reverse)
        {
            case false:
                // _transform.Rotate(Vector3.forward, transform.rotation.z);
                _playerCarRb.AddTorque(transform.rotation.z * Mathf.Deg2Rad * _playerCarRb.inertia * 150f * velocityMag, ForceMode2D.Force);
                break;

            case true:
                // _transform.Rotate(Vector3.forward, -transform.rotation.z);
                _playerCarRb.AddTorque(-transform.rotation.z * Mathf.Deg2Rad * _playerCarRb.inertia * 150f * velocityMag, ForceMode2D.Force);
                break;
        }
    }


    private void MobileInput()
    {
        var i = 0;

        while (Input.touchCount > i) //loop over every touch found    
        {
            if (Input.GetTouch(i).position.x < _screenWidth / 2) //Checking for touchdowns on the left-side of screen
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began) // Storing the position when the screen is touched for the first time
                {
                    _rotateBack = false;
                    _touchPosStart = Input.GetTouch(i).position;
                }

                else if (Input.GetTouch(i).phase == TouchPhase.Moved) // Storing continuous finger positions for rotation
                {
                    _rotateBack = false;
                    var currTouchPos = Input.GetTouch(i).position;
                    _turnDirection = Mathf.Sign(_touchPosStart.x - currTouchPos.x);
                    _deltaPos = Input.GetTouch(i).deltaPosition;
                    transform.Rotate(Vector3.forward, -_deltaPos.x * 10f * Time.deltaTime); // Rotating steering left/right depending on turn direction

                    // if (_moveCar) //Rotate car if its moving front/back
                    //     playerCar.gameObject.transform.Rotate(Vector3.forward, _turnDirection * rotateSpeed * Time.deltaTime);
                }

                else if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                {
                    _touchPosStart = Input.GetTouch(i).position;
                }
                else
                {
                    _rotateBack = true;
                }
            }

            i++;
        }
    }

    private void RotateBack()
    {
        if (!_rotateBack) return;
        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0f, 0f, 0f),
                30f * Time.deltaTime);
    }


    private void DesktopControls()
    {
        Vector2 initTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (!(initTouchPos.x < 0)) return;

        if (Input.GetMouseButtonDown(0)) // Storing the position when the screen is touched for the first time
        {
            _rotateBack = false;
            _touchPosStart.x = initTouchPos.x;
        }
        else if (Input.GetMouseButton(0)) // Storing continuous finger positions for rotation
        {
            _rotateBack = false;
            Vector2 currTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (Math.Abs(_touchPosStart.x - currTouchPos.x) > .1)
            {
                _rotateBack = false;
                _turnDirection = Mathf.Sign(_touchPosStart.x - currTouchPos.x);

                transform.Rotate(Vector3.forward, _turnDirection * rotateSpeed * Time.deltaTime);

                // playerCar.gameObject.transform.Rotate(Vector3.forward, _turnDirection * rotateSpeed * Time.deltaTime);
                // _playerCarRb.MoveRotation(_touchPosDiff * rotateSpeed * Time.deltaTime);

                // _direction = Mathf.Sign(Vector2.Dot(_playerCarRb.velocity, _playerCarRb.GetRelativeVector(Vector2.up)));
                // _playerCarRb.rotation += .5f * _playerCarRb.velocity.magnitude * _direction;
            }
            else if (Input.GetMouseButtonUp(0)) // When finger is lifted
            {
                _rotateBack = true;
            }
        }
    }
}