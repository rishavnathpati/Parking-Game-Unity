using UnityEngine;

public class Steering : MonoBehaviour
{
    public float rotateSpeed = 10f;

    public GameObject playerCar;

    private bool _rotateBack = true;

    private Camera _camera;

    private float _screenWidth;

    private Vector2 _touchPosStart;

    private Vector2 _initTouchPos;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Welcome!!!!!!!");
        _screenWidth = Screen.width;
    }

    private void Update()
    {
        if (Input.touchCount <= 0)
        {
            _rotateBack = true;
            return;
        }

        var i = 0;

        while (Input.touchCount > i) //loop over every touch found    
        {
            if (Input.GetTouch(i).position.x < _screenWidth / 2) //Checking for touchpoints on the leftside of screen
            {
                // if (Input.GetTouch(i).phase == TouchPhase.Began) // Storing the position when the screen is touched for the first time
                // {
                //     rotateBack = false;
                //     _touchPosStart = Input.GetTouch(i).position;
                // }
                //
                // else
                if (Input.GetTouch(i).phase == TouchPhase.Moved) // Storing continuous finger positions for rotation
                {
                    _rotateBack = false;
                    var currTouchPos = Input.GetTouch(i).position;

                    if (_touchPosStart.x < currTouchPos.x) //Move Right
                    {
                        var touchDiff = _touchPosStart.x - currTouchPos.x;
                        transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
                        playerCar.gameObject.transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
                    }
                    else if (_touchPosStart.x > currTouchPos.x) //Move Left
                    {
                        var touchDiff = _touchPosStart.x - currTouchPos.x;
                        transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
                        playerCar.gameObject.transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
                    }
                }

                else if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                {
                    _touchPosStart = Input.GetTouch(i).position;
                }
                else if (_rotateBack == true)
                {
                    transform.rotation =
                        Quaternion.RotateTowards(
                            transform.rotation,
                            Quaternion.Euler(0f, 0f, 0f),
                            20f * Time.deltaTime);

                    playerCar.gameObject.transform.rotation = Quaternion.RotateTowards(
                        playerCar.gameObject.transform.rotation,
                        Quaternion.Euler(0f, 0f, -90f),
                        20f * Time.deltaTime);
                }
            }

            i++;
        }


        // Vector2 initTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        // if (!(initTouchPos.x < 0)) return;
        // if (Input.GetMouseButtonDown(0)) // Storing the position when the screen is touched for the first time
        // {
        //     rotateBack = false;
        //     _touchPosStart = initTouchPos.x;
        // }
        // else if (Input.GetMouseButton(0)) // Storing continuous finger positions for rotation
        // {
        //     rotateBack = false;
        //     Vector2 currTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        //
        //     if (_touchPosStart < currTouchPos.x) // Move Right
        //     {
        //         Debug.Log("Finger Moved Right");
        //         var touchDiff = _touchPosStart.x - currTouchPos.x;
        //         // if (Mathf.Abs(touchDiff) > 1)
        //         transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
        //         playerCar.gameObject.transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
        //     }
        //     else if (_touchPosStart > currTouchPos.x)
        //     {
        //         Debug.Log("Finger Moved Left"); // Move Left
        //         var touchDiff = _touchPosStart - currTouchPos.x;
        //         // if (Mathf.Abs(touchDiff) > 1)
        //         transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
        //         playerCar.gameObject.transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
        //     }
        // }
        // else if (Input.GetMouseButtonUp(0)) // When finger is lifted
        // {
        //     rotateBack = true;
        //     // RotateBack();
        //     // transform.Rotate(Vector3.forward, -Mathf.Lerp(0f, transform.eulerAngles.z, 5));
        //     // transform.rotation = Quaternion.Lerp(quaternion.Euler(0,0,0), Quaternion.Euler(0, 0, 0), 5 * Time.deltaTime);
        //     // playerCar.gameObject.transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
        // }
        //

        // if (!rotateBack) return;
    }
}