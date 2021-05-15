using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCar : MonoBehaviour
{
    [SerializeField] private float force;

    private bool _accelerate;

    public bool moveCar;

    private Rigidbody2D _rb;

    [FormerlySerializedAs("GameOver")] [SerializeField]
    private GameObject gameOver;

    [FormerlySerializedAs("GameWin")] [SerializeField]
    private GameObject gameWin;

    private static PlayerCar _instance;

    public static bool Reverse;

    private float _startForce;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startForce = force;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        ControlMovement();
    }

    private void ControlMovement()
    {
        if (!moveCar) return;

        switch (_accelerate)
        {
            case true: // Accelerate
                _rb.AddRelativeForce(Vector3.right * force, ForceMode2D.Force);
                force += 1;
                break;
            case false: // Brake
                _rb.AddRelativeForce(Vector3.left * force, ForceMode2D.Force);
                force += .5f;
                break;
        }
    }

    public void Decelerate()
    {
        moveCar = false;
        force = _startForce;
    }

    public void Accelerate()
    {
        moveCar = true;
        _accelerate = true;
        Reverse = false;
    }

    public void Brake()
    {
        Reverse = true;
        moveCar = true;
        _accelerate = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("cars"))
        {
            _rb.velocity = Vector2.zero;
            moveCar = false;
            _accelerate = false;
            gameOver.SetActive(true);
        }
        else if (other.gameObject.CompareTag("boundary"))
        {
            //Show warning, deduct lives
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Parked Properly");
            gameWin.SetActive(true);
            //Next level
        }
    }
}