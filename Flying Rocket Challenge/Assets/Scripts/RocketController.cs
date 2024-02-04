



using UnityEngine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public Cinemachine.CinemachineVirtualCamera virtualCamera; // Adiciona uma referência à câmera Cinemachine

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition; // Salva a última posição conhecida do foguete

    void Start()
    {
        rocketRigidbody.AddForce(Vector3.up * thrustForce);
        lastRocketPosition = transform.position; // Inicializa a última posição conhecida do foguete
        Invoke("StopThrust", 5f);
    }

    void Update()
    {
        if (rocketRigidbody.velocity.y < 0 && !hasSeparated)
        {
            if (transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro estágio...");
                SeparateFirstStage();
            }
        }
    }

    public void SeparateFirstStage()
    {
        if (separationPoint != null)
        {
            // Instanciar o segundo estágio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);

            // Passar a última posição conhecida do foguete para o segundo estágio
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);

            // Desativar o primeiro estágio
            gameObject.SetActive(false);

            // Trocar a câmera Cinemachine para seguir o segundo estágio
            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Referência para a câmera Cinemachine não atribuída. Verifique se foi configurada corretamente no Inspector.");
            }
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
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
    private Vector3 lastRocketPosition; // Salva a última posição conhecida do foguete

    void Start()
    {
        rocketRigidbody.AddForce(Vector3.up * thrustForce);
        lastRocketPosition = transform.position; // Inicializa a última posição conhecida do foguete
        Invoke("StopThrust", 5f);
    }

    void Update()
    {
        if (rocketRigidbody.velocity.y < 0 && !hasSeparated)
        {
            if (transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro estágio...");
                SeparateFirstStage();
            }
        }
    }


    public void SeparateFirstStage()
    {
       
        if (separationPoint != null)
        {
            // Instanciar o segundo estágio
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);

            // Passar a última posição conhecida do foguete para o segundo estágio
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);

            // Desativar o primeiro estágio
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
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
                Debug.Log("Separando o primeiro estágio...");
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
                Debug.LogError("MeshRenderer do corpo_nariz não encontrado.");
            }

            // Ativar o MeshRenderer do paraquedas
            MeshRenderer parachutesRenderer = parachutesPrefab.GetComponent<MeshRenderer>();
            if (parachutesRenderer != null)
            {
                parachutesRenderer.enabled = true;
            }
            else
            {
                Debug.LogError("MeshRenderer do paraquedas não encontrado.");
            }

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

