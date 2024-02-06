using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionDetector : MonoBehaviour
{
    private SecondStageController secondStageController;

    private void Start()
    {
        secondStageController = GetComponentInParent<SecondStageController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            //secondStageController.StopRotation();
        }
    }
}

