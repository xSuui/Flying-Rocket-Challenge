using UnityEngine;

public class SeparationPoint : MonoBehaviour
{
    public GameObject explosionParticles; // Refer�ncia para o sistema de part�culas de explos�o
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
                // Desativar os colliders do SeparationPoint
                Collider[] colliders = GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }

                // Ativa as part�culas de explos�o imediatamente
                if (explosionParticles != null)
                {
                    explosionParticles.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning("Objeto colidido n�o possui o script RocketController.");
            }
        }
    }
}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationPoint : MonoBehaviour
{
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
                // Desativar os colliders do SeparationPoint
                Collider[] colliders = GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }
            }
            else
            {
                Debug.LogWarning("Objeto colidido n�o possui o script RocketController.");
            }
        }
    }
}*/






/*using UnityEngine;

public class SeparationPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            RocketController rocketController = other.GetComponent<RocketController>();
            if (rocketController != null)
            {
                rocketController.SeparateFirstStage();
            }
            else
            {
                Debug.LogWarning("Objeto colidido n�o possui o script RocketController.");
            }
        }
    }
}*/

