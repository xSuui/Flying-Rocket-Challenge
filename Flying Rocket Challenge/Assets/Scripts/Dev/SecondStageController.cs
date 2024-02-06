using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SecondStageController : MonoBehaviour
{
    public CinemachineVirtualCamera rocketCamera;
    public CinemachineVirtualCamera secondStageCamera; 

    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; 
    public float finalVelocity = -30f; 
    public float velocityIncreaseRate = 2f; 
    public float maxVelocityIncreaseRate = 5f; 
    public float dragIncreaseRate = 0.1f; 
    public float maxDrag = 1f; 
    public float rotationAngle = 5f; 
    public float rotationSpeed = 10f; 
    public float rotationStopDelay = 3f; 
    public float freezeRotationDelay = 5f; 

    private bool isDescending = true;
    private bool rotationStopped = false;
    private bool grounded = false;
    private float currentVelocityIncreaseRate;

    void Start()
    {
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);

        secondStageRigidbody.drag = 0f;

        Invoke("StopRotation", rotationStopDelay);

        Invoke("FreezeRotations", freezeRotationDelay);

        currentVelocityIncreaseRate = maxVelocityIncreaseRate;
    }

    void Update()
    {
        if (isDescending && !rotationStopped)
        {
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                secondStageRigidbody.velocity += new Vector3(0f, currentVelocityIncreaseRate * Time.deltaTime, 0f);

                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                float rotationZ = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                currentVelocityIncreaseRate = Mathf.Lerp(maxVelocityIncreaseRate, velocityIncreaseRate, Mathf.InverseLerp(initialVelocity, finalVelocity, secondStageRigidbody.velocity.y));
            }
            else
            {
                isDescending = false;
            }
        }

        if (!grounded && secondStageRigidbody.velocity.y <= 0)
        {
            grounded = true;
        }
    }

    public void ActivateSecondStage()
    {
        rocketCamera.gameObject.SetActive(false);

        secondStageCamera.gameObject.SetActive(true);
    }

    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void StopDescending()
    {
        isDescending = false;
    }

    public void StopRotation()
    {
        rotationStopped = true;
    }

    private void FreezeRotations()
    {
        secondStageRigidbody.freezeRotation = true;
    }
}