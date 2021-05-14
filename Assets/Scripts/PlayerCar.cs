using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCar : MonoBehaviour
{
    [SerializeField] private float force;

    private bool _accelerate = false;

    public bool moveCar = false;

    private Rigidbody2D _rb;


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
}