using UnityEngine;

public class TurnOffObject : MonoBehaviour
{
    public GameObject objetoParaDesligar;
    private bool spacePressed = false;

    void Update()
    {
        // Verifica se a tecla de espaço foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
            // Chama o método Desligar após 5 segundos
            Invoke("Desligar", 5f);
        }
    }

    void Desligar()
    {
        // Verifica se a tecla de espaço foi pressionada antes de desligar o objeto
        if (spacePressed && objetoParaDesligar != null)
        {
            objetoParaDesligar.SetActive(false);
        }
    }
}




