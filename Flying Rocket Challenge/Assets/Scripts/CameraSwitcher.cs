using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject rocket; // Prefab do foguete
    public GameObject secondStage; // Prefab do segundo estágio
    public CameraFollow cameraFollow; // Referência ao script de câmera

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            // Desativar o foguete
            rocket.SetActive(false);

            // Ativar o segundo estágio
            secondStage.SetActive(true);

            // Definir o segundo estágio como alvo da câmera
            cameraFollow.target = secondStage.transform;
        }
    }
}

