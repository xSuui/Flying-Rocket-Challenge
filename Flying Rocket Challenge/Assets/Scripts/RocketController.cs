using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public GameObject corpoNarizPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float rotationSpeedZ = -50f;
    public float separationHeight = 50f;
    public float parachuteHeight = 100f;

    private bool hasSeparated = false;
    private bool hasDeployedParachute = false;

    void Start()
    {
        rocketRigidbody.AddForce(Vector3.up * thrustForce);
        Invoke("StopThrust", 5f);
    }

    void Update()
    {
        //Debug.Log("Altura atual do foguete: " + transform.position.y);

        if (rocketRigidbody.velocity.y < 0 && !hasSeparated)
        {
            if (transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro estágio...");
                SeparateFirstStage();
            }
        }

        rocketRigidbody.drag = hasDeployedParachute ? 2f : 0.1f;

        if (transform.position.y >= parachuteHeight && !hasDeployedParachute)
        {
            DeployParachute();
            //transform.Rotate(Vector3.forward, 180f);
        }
    }

    void SeparateFirstStage()
    {
        hasSeparated = true;

        // Verificar se o método SeparateFirstStage() está sendo chamado
        Debug.Log("SeparateFirstStage() chamado.");

        // Verificar se o separationPoint está atribuído corretamente
        if (separationPoint != null)
        {
            Debug.Log("separationPoint encontrado.");
            // Desativar o primeiro estágio (corpo_nariz)
            corpoNarizPrefab.SetActive(false);

            // Criar e ativar o segundo estágio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.SetActive(true);

            // Desativar o primeiro estágio
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
        }
    }



    /*void SeparateFirstStage()
    {
        hasSeparated = true;

        // Desativar o primeiro estágio (corpo_nariz)
        corpoNarizPrefab.SetActive(false);

        // Criar e ativar o segundo estágio
        GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
        segundoEstagio.SetActive(true);

        // Desativar o prefab do foguete
        gameObject.SetActive(false);
    }*/

    void DeployParachute()
    {
        hasDeployedParachute = true;
        // Simular abertura do paraquedas
        rocketRigidbody.drag = 2f;
    }

    public void StopThrust()
    {
        rocketRigidbody.velocity = Vector3.zero;
        rocketRigidbody.angularVelocity = Vector3.zero;
    }
}

    