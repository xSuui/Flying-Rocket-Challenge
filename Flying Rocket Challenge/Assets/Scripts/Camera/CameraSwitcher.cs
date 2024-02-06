using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject rocket;
    public GameObject secondStage; 
    public CameraFollow cameraFollow; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            rocket.SetActive(false);

            secondStage.SetActive(true);

            cameraFollow.target = secondStage.transform;
        }
    }
}

