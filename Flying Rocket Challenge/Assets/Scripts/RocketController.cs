



using UnityEngine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public Cinemachine.CinemachineVirtualCamera virtualCamera; // Adiciona uma refer�ncia � c�mera Cinemachine

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition; // Salva a �ltima posi��o conhecida do foguete

    void Start()
    {
        rocketRigidbody.AddForce(Vector3.up * thrustForce);
        lastRocketPosition = transform.position; // Inicializa a �ltima posi��o conhecida do foguete
        Invoke("StopThrust", 5f);
    }

    void Update()
    {
        if (rocketRigidbody.velocity.y < 0 && !hasSeparated)
        {
            if (transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro est�gio...");
                SeparateFirstStage();
            }
        }
    }

    public void SeparateFirstStage()
    {
        if (separationPoint != null)
        {
            // Instanciar o segundo est�gio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);

            // Passar a �ltima posi��o conhecida do foguete para o segundo est�gio
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);

            // Desativar o primeiro est�gio
            gameObject.SetActive(false);

            // Trocar a c�mera Cinemachine para seguir o segundo est�gio
            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Refer�ncia para a c�mera Cinemachine n�o atribu�da. Verifique se foi configurada corretamente no Inspector.");
            }
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }

    public void StopThrust()
    {
        rocketRigidbody.velocity = Vector3.zero;
        rocketRigidbody.angularVelocity = Vector3.zero;
    }
}


/*using UnityEngine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition; // Salva a �ltima posi��o conhecida do foguete

    void Start()
    {
        rocketRigidbody.AddForce(Vector3.up * thrustForce);
        lastRocketPosition = transform.position; // Inicializa a �ltima posi��o conhecida do foguete
        Invoke("StopThrust", 5f);
    }

    void Update()
    {
        if (rocketRigidbody.velocity.y < 0 && !hasSeparated)
        {
            if (transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro est�gio...");
                SeparateFirstStage();
            }
        }
    }


    public void SeparateFirstStage()
    {
       
        if (separationPoint != null)
        {
            // Instanciar o segundo est�gio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);

            // Passar a �ltima posi��o conhecida do foguete para o segundo est�gio
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);

            // Desativar o primeiro est�gio
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }

    public void StopThrust()
    {
        rocketRigidbody.velocity = Vector3.zero;
        rocketRigidbody.angularVelocity = Vector3.zero;
    }
}*/




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public GameObject corpoNarizPrefab;
    public GameObject parachutesPrefab;
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
        if (rocketRigidbody.velocity.y < 0 && !hasSeparated)
        {
            if (transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro est�gio...");
                SeparateFirstStage();
            }
        }

        rocketRigidbody.drag = hasDeployedParachute ? 2f : 0.1f;

        if (transform.position.y >= parachuteHeight && !hasDeployedParachute)
        {
            DeployParachute();
        }
    }

    public void SeparateFirstStage()
    {
        hasSeparated = true;

        if (separationPoint != null)
        {
            // Desativar o MeshRenderer do corpo_nariz
            MeshRenderer corpoNarizRenderer = corpoNarizPrefab.GetComponent<MeshRenderer>();
            if (corpoNarizRenderer != null)
            {
                corpoNarizRenderer.enabled = false;
            }
            else
            {
                Debug.LogError("MeshRenderer do corpo_nariz n�o encontrado.");
            }

            // Ativar o MeshRenderer do paraquedas
            MeshRenderer parachutesRenderer = parachutesPrefab.GetComponent<MeshRenderer>();
            if (parachutesRenderer != null)
            {
                parachutesRenderer.enabled = true;
            }
            else
            {
                Debug.LogError("MeshRenderer do paraquedas n�o encontrado.");
            }

            // Criar e ativar o segundo est�gio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.SetActive(true);

            // Desativar o primeiro est�gio
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }

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
}*/




/*
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
                Debug.Log("Separando o primeiro est�gio...");
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

        // Verificar se o m�todo SeparateFirstStage() est� sendo chamado
        Debug.Log("SeparateFirstStage() chamado.");

        // Verificar se o separationPoint est� atribu�do corretamente
        if (separationPoint != null)
        {
            Debug.Log("separationPoint encontrado.");
            // Desativar o primeiro est�gio (corpo_nariz)
            corpoNarizPrefab.SetActive(false);

            // Criar e ativar o segundo est�gio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.SetActive(true);

            // Desativar o primeiro est�gio
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }



    /*void SeparateFirstStage()
    {
        hasSeparated = true;

        // Desativar o primeiro est�gio (corpo_nariz)
        corpoNarizPrefab.SetActive(false);

        // Criar e ativar o segundo est�gio
        GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
        segundoEstagio.SetActive(true);

        // Desativar o prefab do foguete
        gameObject.SetActive(false);
    }

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
}*/

