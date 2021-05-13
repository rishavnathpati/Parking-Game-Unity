using UnityEngine;

public class Steering : MonoBehaviour
{
    public float rotateSpeed = 10f;

    private Camera _camera;

    private float _touchPosStart;

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 initTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            _touchPosStart = initTouchPos.x;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currTouchPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_touchPosStart < currTouchPos.x)
            {
                Debug.Log("Finger Moved Right");   
                var touchDiff = _touchPosStart - currTouchPos.x;
                transform.Rotate(Vector3.back,rotateSpeed*Time.deltaTime);
            }
            else if (_touchPosStart > currTouchPos.x)
            {
                Debug.Log("Finger Moved Right");
                var touchDiff= _touchPosStart - currTouchPos.x;
                transform.Rotate(Vector3.back,-rotateSpeed*Time.deltaTime);
            }
        }
        
    }
}