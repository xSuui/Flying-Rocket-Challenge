using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Transform do foguete ou do segundo est�gio
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Offset da posi��o da c�mera em rela��o ao alvo
    public float smoothSpeed = 0.125f; // Velocidade de suaviza��o do movimento da c�mera

    void LateUpdate()
    {
        // Se o alvo estiver definido
        if (target != null)
        {
            // Calcular a posi��o desejada da c�mera com base no alvo e no offset
            Vector3 desiredPosition = target.position + offset;

            // Suavizar o movimento da c�mera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Definir a nova posi��o da c�mera
            transform.position = smoothedPosition;

            // Fazer a c�mera olhar para o alvo
            transform.LookAt(target);
        }
    }
}




