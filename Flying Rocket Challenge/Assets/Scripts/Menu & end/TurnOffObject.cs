using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffObject : MonoBehaviour
{
    public GameObject objetoParaDesligar;
    private bool spacePressed = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
            Invoke("Desligar", 5f);
        }
    }

    void Desligar()
    {
        if (spacePressed && objetoParaDesligar != null)
        {
            objetoParaDesligar.SetActive(false);
        }
    }
}