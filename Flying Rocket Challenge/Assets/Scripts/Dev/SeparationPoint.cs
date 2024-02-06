using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationPoint : MonoBehaviour
{
    public GameObject explosionParticles; 
    private bool separationOccurred = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket") && !separationOccurred)
        {
            RocketController rocketController = other.GetComponent<RocketController>();
            if (rocketController != null)
            {
                rocketController.SeparateFirstStage();
                separationOccurred = true;
                Collider[] colliders = GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }

                if (explosionParticles != null)
                {
                    explosionParticles.SetActive(true);
                }
            }
        }
    }
}