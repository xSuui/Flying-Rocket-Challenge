using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Transform do foguete ou do segundo estágio
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Offset da posição da câmera em relação ao alvo
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera

    void LateUpdate()
    {
        // Se o alvo estiver definido
        if (target != null)
        {
            // Calcular a posição desejada da câmera com base no alvo e no offset
            Vector3 desiredPosition = target.position + offset;

            // Suavizar o movimento da câmera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Definir a nova posição da câmera
            transform.position = smoothedPosition;

            // Fazer a câmera olhar para o alvo
            transform.LookAt(target);
        }
    }
}




