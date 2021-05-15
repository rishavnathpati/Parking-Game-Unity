using System;
using UnityEngine;
using UnityEngine.UI;

public class Steering : MonoBehaviour
{
    public float rotateSpeed = 10f;

    public GameObject playerCar;

    private Camera _camera;

    private Rigidbody2D _playerCarRb;

    private bool _rotateBack = true;

    private float _screenWidth;

    private bool _moveCar;

    private float _remTurnAngle;

    private float _touchPosDiff;

    [SerializeField] private float velocityMag;

    private Vector2 _touchPosStart;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // _playerCarrigidbody2D = playerCar.GetComponent<Rigidbody2D>();
        Debug.Log("Welcome!!!!!!!");
        _playerCarRb = playerCar.GetComponent<Rigidbody2D>();
        _screenWidth = Screen.width;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private bool CarMoving()
    {
        velocityMag = playerCar.GetComponent<Rigidbody2D>().velocity.magnitude;

        return velocityMag > .5;
    }

    private void Update()
    {
        _moveCar = CarMoving();

        if (_rotateBack) // To rotate back car and steering when finger is lifted/ not on steering
        {
            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.Euler(0f, 0f, 0f),
                    20f * Time.deltaTime);

            if (_moveCar && transform.rotation.z < playerCar.gameObject.transform.rotation.z)
            {
                playerCar.gameObject.transform.rotation = Quaternion.RotateTowards(
                    playerCar.gameObject.transform.rotation,
                    Quaternion.Euler(0f, 0f, -90f),
                    20f * Time.deltaTime);
            }
        }

        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld when Input.touchCount <= 0:
                _rotateBack = true;
                return;
            case DeviceType.Handheld:
            {
                var i = 0;

                while (Input.touchCount > i) //loop over every touch found    
                {
                    if (Input.GetTouch(i).position.x < _screenWidth / 2) //Checking for touchpoints on the leftside of screen
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
                            _touchPosDiff = Mathf.Sign(_touchPosStart.x - currTouchPos.x);

                            transform.Rotate(Vector3.forward, _touchPosDiff * rotateSpeed * Time.deltaTime);

                            switch (_moveCar)
                            {
                                case true:
                                    playerCar.gameObject.transform.Rotate(Vector3.forward, _touchPosDiff * rotateSpeed * Time.deltaTime); //Rotate car if its moving front/back
                                    break;
                                case false:
                                    _remTurnAngle = _touchPosDiff * rotateSpeed * Time.deltaTime;
                                    break;
                            }
                        }

                        else if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                        {
                            _touchPosStart = Input.GetTouch(i).position;

                            while (Mathf.Abs(playerCar.gameObject.transform.rotation.z - _remTurnAngle) > 0 && CarMoving() == true)
                            {
                                playerCar.gameObject.transform.rotation = Quaternion.RotateTowards(
                                    playerCar.gameObject.transform.rotation,
                                    Quaternion.Euler(0f, 0f, _remTurnAngle),
                                    20f * Time.deltaTime);
                            }
                        }
                        else
                        {
                            _rotateBack = true;
                        }
                    }

                    i++;
                }

                break;
            }

            #region Desktop Controls

            case DeviceType.Desktop:
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
                        _touchPosDiff = Mathf.Sign(_touchPosStart.x - currTouchPos.x);

                        transform.Rotate(Vector3.forward, _touchPosDiff * rotateSpeed * Time.deltaTime);
                        playerCar.gameObject.transform.Rotate(Vector3.forward, _touchPosDiff * rotateSpeed * Time.deltaTime);
                        // _playerCarRb.MoveRotation(_touchPosDiff * rotateSpeed * Time.deltaTime);

                        // _direction = Mathf.Sign(Vector2.Dot(_playerCarRb.velocity, _playerCarRb.GetRelativeVector(Vector2.up)));
                        // _playerCarRb.rotation += .5f * _playerCarRb.velocity.magnitude * _direction;
                    }
                    else if (Input.GetMouseButtonUp(0)) // When finger is lifted
                    {
                        _rotateBack = true;
                    }
                }

                break;
            }

            #endregion
        }
    }
}