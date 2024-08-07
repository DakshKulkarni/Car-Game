using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarControl : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }
    [Serializable]
    public struct Wheel
    {
        public GameObject wheel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }
    public float maxAcc=30f;
    public float RevAcc=50f;
    public float TurnSens=1f;
    public float maxAngle=30f;
    public float maxSpeed = 50f;
    public float currentSpeed;
    private Vector3 _centerOfMass;
    public List<Wheel> wheels;
    public float moveInput;
    public float steerInput;
    private Rigidbody car;
    public GameObject barrier;
    public GameObject startSpawnArea;
    public GameObject endSpawnArea;
    public int noOfBarriers=10;

    void Start()
    {
        car = GetComponent<Rigidbody>();
        car.centerOfMass=_centerOfMass;
        SpawnBarriers();
    }
    void Update()
    {
        GetInput();
    }
    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }
    void GetInput()
    {
        moveInput=Input.GetAxis("Vertical");
        steerInput=Input.GetAxis("Horizontal");
    }
    void Move()
    {
        float currentSpeed = car.velocity.magnitude;

        if (currentSpeed < maxSpeed)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * maxAcc * Time.deltaTime * 10000;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0;
            }

            car.velocity = Vector3.ClampMagnitude(car.velocity, maxSpeed);
        }
        Debug.Log(currentSpeed);
    }
    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel==Axel.Front)
            {
                var _steerAngle=steerInput*TurnSens*maxAngle;
                wheel.wheelCollider.steerAngle=Mathf.Lerp(wheel.wheelCollider.steerAngle,_steerAngle,0.6f);
            }
        }
    }
    void Brake()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque=RevAcc*Time.deltaTime*20000;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque=0;
            }
        }
    }
     void SpawnBarriers()
    {
        float xMin = 2397f;
        float xMax = 2450f;
        Vector3 spawnAreaStart = startSpawnArea.transform.position;
        Vector3 spawnAreaEnd = endSpawnArea.transform.position;

        for (int i = 0; i < noOfBarriers; i++)
        {
            if (i % 2 == 0)
            {
                Vector3 spawnPos = new Vector3(
                    UnityEngine.Random.Range(xMin, xMax),
                    UnityEngine.Random.Range(spawnAreaStart.y, spawnAreaEnd.y),
                    UnityEngine.Random.Range(spawnAreaStart.z, spawnAreaEnd.z)
                );

                Quaternion spawnRot = Quaternion.Euler(0, -90, 0);
                Instantiate(barrier, spawnPos, spawnRot);
            }
        }
    }
     void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Barrier"))
        {
            Restart();
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
