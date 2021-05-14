using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    private const string Vertical = "Vertical";

    private float _horizontalInput;

    private float _verticalInput;

    private float _currentSteerAngle;

    private float _currentBreakForce;

    private bool _isBreaking;

    [SerializeField] private float motorForce;

    [SerializeField] private float breakForce;

    [SerializeField] private float maxSteerAngle;
    

    [SerializeField] private WheelCollider frontLeftWheelCollider;

    [SerializeField] private WheelCollider frontRightWheelCollider;

    [SerializeField] private WheelCollider rearLeftWheelCollider;

    [SerializeField] private WheelCollider rearRightWheelCollider;
    

    [SerializeField] private Transform frontLeftWheelTransform;

    [SerializeField] private Transform frontRightWheeTransform;

    [SerializeField] private Transform rearLeftWheelTransform;

    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        _horizontalInput = Input.GetAxis(Horizontal);
        _verticalInput = Input.GetAxis(Vertical);
        _isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = _verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = _verticalInput * motorForce;
        _currentBreakForce = _isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = _currentBreakForce;
        frontLeftWheelCollider.brakeTorque = _currentBreakForce;
        rearLeftWheelCollider.brakeTorque = _currentBreakForce;
        rearRightWheelCollider.brakeTorque = _currentBreakForce;
    }

    private void HandleSteering()
    {
        _currentSteerAngle = maxSteerAngle * _horizontalInput;
        frontLeftWheelCollider.steerAngle = _currentSteerAngle;
        frontRightWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out var pos, out var rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}