using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject rocket; // Prefab do foguete
    public GameObject secondStage; // Prefab do segundo est�gio
    public CameraFollow cameraFollow; // Refer�ncia ao script de c�mera

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            // Desativar o foguete
            rocket.SetActive(false);

            // Ativar o segundo est�gio
            secondStage.SetActive(true);

            // Definir o segundo est�gio como alvo da c�mera
            cameraFollow.target = secondStage.transform;
        }
    }
}

