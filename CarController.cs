using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    public bool isBreaking;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;
    public Transform rightrayTransform;
    public Transform leftrayTransform;
    public Transform car;

    private bool steeringright;
    private bool steeringleft;


    public float maxSteeringAngle = 30f;
    public float motorForce = 5000f;
    public float brakeForce = 10000f;
    public float ismotor = 0;



    private Vector3 PreviousFramePosition;
    public float Speed;
    public float movementPerFrame;


    private void speed()
    {
        movementPerFrame = Vector3.Distance (PreviousFramePosition, car.position);
        Speed = movementPerFrame;
        PreviousFramePosition = car.position;
        if (Speed < 0.2)
        {
            ismotor = 0.2f;
            isBreaking = false;
        }else
        {
            ismotor = 0f;
            isBreaking = true;
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        Steering();
        speed();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }  

    private void Steering()
    {
        
        
        if (Physics.Raycast(leftrayTransform.position, leftrayTransform.TransformDirection(Vector3.left), out RaycastHit lefthitinfo, 1.5f))
        {
            steerAngle = 5f;
            steeringleft = true;
        }   else
        {
            steeringleft = false;
        }

        if (Physics.Raycast(rightrayTransform.position, rightrayTransform.TransformDirection(Vector3.right), out RaycastHit righthitinfo, 1.5f))
        {
            steerAngle = -5f;
            steeringright = true;
        }   else
        {
            steeringright = false;
        }

        if (steeringright == false & steeringleft == false)
        {
            steerAngle = 0;
        }
        

    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleSteering()
    {
        //steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque =  ismotor * motorForce;
        frontRightWheelCollider.motorTorque =  ismotor * motorForce;
        rearLeftWheelCollider.motorTorque =  ismotor * motorForce;
        rearRightWheelCollider.motorTorque =  ismotor * motorForce;

        brakeForce = isBreaking ? 50000f : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }
}
