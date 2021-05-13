using UnityEngine;
using UnityEngine.Serialization;

public class Steering : MonoBehaviour
{
    public float rotateSpeed = 10f;

    private Camera _camera;

    private float _touchPosStart;

    public GameObject playerCar;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Welcome!!!!!!!");
    }

    private void Update()
    {
        Vector2 initTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (!(initTouchPos.x < 0)) return;

        if (Input.GetMouseButtonDown(0)) // Storing the position when the screen is touched for the first time
        {
            _touchPosStart = initTouchPos.x;
        }
        else if (Input.GetMouseButton(0)) // Storing continuous finger positions for rotation
        {
            Vector2 currTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_touchPosStart < currTouchPos.x) // Move Right
            {
                Debug.Log("Finger Moved Right");
                var touchDiff = _touchPosStart - currTouchPos.x;
                // if (Mathf.Abs(touchDiff) > 1)
                transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
                playerCar.gameObject.transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
            }
            else if (_touchPosStart > currTouchPos.x)
            {
                Debug.Log("Finger Moved Left"); // Move Left
                var touchDiff = _touchPosStart - currTouchPos.x;
                // if (Mathf.Abs(touchDiff) > 1)
                transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
                playerCar.gameObject.transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetMouseButtonUp(0)) // When finger is lifted
        {
            transform.Rotate(Vector3.forward, -Mathf.Lerp(0f, transform.eulerAngles.z, 5));
            playerCar.gameObject.transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
        }
    }
}