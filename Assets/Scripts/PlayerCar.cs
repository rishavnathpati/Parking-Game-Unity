using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerCar : MonoBehaviour
{
    [SerializeField] private float force;

    private bool _accelerate = false;

    public bool moveCar = false;

    private Rigidbody2D _rb;

    [FormerlySerializedAs("GameOver")] [SerializeField]
    private GameObject gameOver;

    [FormerlySerializedAs("GameWin")] [SerializeField]
    private GameObject gameWin;

    public static PlayerCar Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
                _rb.AddRelativeForce(Vector2.up * force, ForceMode2D.Force);
                break;
            case false: // Brake
                _rb.AddRelativeForce(Vector2.down * force, ForceMode2D.Force);
                break;
        }
    }

    public void Decelerate()
    {
        moveCar = false;
    }

    public void Accelerate()
    {
        moveCar = true;
        _accelerate = true;
    }

    public void Brake()
    {
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