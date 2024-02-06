using UnityEngine;
using Cinemachine;

public class RocketController : MonoBehaviour
{
    public SFXType sFXType;
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject rocketParticles; // Objeto de part�culas do foguete

    private bool hasSeparated = false;
    private bool gameStarted = false; // Controla se o jogo come�ou ou n�o
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a acelera��o conforme necess�rio

    void Start()
    {
        // Desativa o foguete no in�cio
        rocketRigidbody.isKinematic = true;
        rocketRigidbody.detectCollisions = false;
        PlaySFX();
    }

    private void PlaySFX()
    {
        if (!hasSeparated) // Verifica se o foguete j� se separou antes de tocar o som
        {
            SFXPool.Instance.Play(sFXType);
        }
    }

    void Update()
    {
        // Verifica se o espa�o foi pressionado para iniciar o jogo
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Executa apenas se o jogo come�ou
        if (gameStarted)
        {
            // Verifica se ultrapassou a altura de separa��o
            if (!hasSeparated && transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro est�gio...");
                SeparateFirstStage();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplica uma for�a de empuxo suave apenas enquanto o objeto ainda n�o se separou
        if (gameStarted && !hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            // Verifica se o foguete est� subindo e ativa as part�culas
            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                rocketParticles.SetActive(true);
            }
        }
    }

    void StartGame()
    {
        // Ativa o foguete para come�ar o jogo
        rocketRigidbody.isKinematic = false;
        rocketRigidbody.detectCollisions = true;
        gameStarted = true;
    }

    public void SeparateFirstStage()
    {
        if (separationPoint != null)
        {
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);
            gameObject.SetActive(false);

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Refer�ncia para a c�mera Cinemachine n�o atribu�da. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro ap�s a separa��o

            // Desliga as part�culas do foguete quando atinge a posi��o de separa��o
            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }

            // Para o som quando o foguete se separa
            SFXPool.Instance.Stop(); // Removido o argumento aqui
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }

}




/*using UnityEngine;
using Cinemachine;

public class RocketController : MonoBehaviour
{
    public SFXType sFXType;
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject rocketParticles; // Objeto de part�culas do foguete

    private bool hasSeparated = false;
    private bool gameStarted = false; // Controla se o jogo come�ou ou n�o
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a acelera��o conforme necess�rio

    void Start()
    {
        // Desativa o foguete no in�cio
        rocketRigidbody.isKinematic = true;
        rocketRigidbody.detectCollisions = false;
        PlaySFX();
    }

    private void PlaySFX()
    {
        SFXPool.Instance.Play(sFXType);
    }

    void Update()
    {
        // Verifica se o espa�o foi pressionado para iniciar o jogo
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Executa apenas se o jogo come�ou
        if (gameStarted)
        {
            // Verifica se ultrapassou a altura de separa��o
            if (!hasSeparated && transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro est�gio...");
                SeparateFirstStage();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplica uma for�a de empuxo suave apenas enquanto o objeto ainda n�o se separou
        if (gameStarted && !hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            // Verifica se o foguete est� subindo e ativa as part�culas
            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                rocketParticles.SetActive(true);
            }
        }
    }

    void StartGame()
    {
        // Ativa o foguete para come�ar o jogo
        rocketRigidbody.isKinematic = false;
        rocketRigidbody.detectCollisions = true;
        gameStarted = true;
    }

    public void SeparateFirstStage()
    {
        if (separationPoint != null)
        {
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);
            gameObject.SetActive(false);

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Refer�ncia para a c�mera Cinemachine n�o atribu�da. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro ap�s a separa��o

            // Desliga as part�culas do foguete quando atinge a posi��o de separa��o
            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }
}*/




/*using UnityEngine;
using Cinemachine;


public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject rocketParticles; // Objeto de part�culas do foguete

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a acelera��o conforme necess�rio

    void Start()
    {
        lastRocketPosition = transform.position;
        Invoke("StopThrust", 5f);
    }

    void FixedUpdate()
    {
        // Aplica uma for�a de empuxo suave apenas enquanto o objeto ainda n�o se separou
        if (!hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            // Verifica se o foguete est� subindo e ativa as part�culas
            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                //Debug.Log("Ativando part�culas do foguete.");
                rocketParticles.SetActive(true);
            }
        }
    }



    void Update()
    {
        // Verifica se ultrapassou a altura de separa��o
        if (!hasSeparated && transform.position.y >= separationHeight)
        {
            Debug.Log("Separando o primeiro est�gio...");
            SeparateFirstStage();
        }
    }

    public void SeparateFirstStage()
    {
        if (separationPoint != null)
        {
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);
            gameObject.SetActive(false);

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Refer�ncia para a c�mera Cinemachine n�o atribu�da. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro ap�s a separa��o

            // Desliga as part�culas do foguete quando atinge a posi��o de separa��o
            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }


    public void StopThrust()
    {
        // Desliga a for�a de empuxo ap�s um tempo
        hasSeparated = true;
        rocketRigidbody.velocity = Vector3.zero;
        rocketRigidbody.angularVelocity = Vector3.zero;
    }
}*/




/*using UnityEngine;
using Cinemachine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public CinemachineVirtualCamera virtualCamera;

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a acelera��o conforme necess�rio

    void Start()
    {
        lastRocketPosition = transform.position;
        Invoke("StopThrust", 5f);
    }

    void FixedUpdate()
    {
        // Aplica uma for�a de empuxo suave apenas enquanto o objeto ainda n�o se separou
        if (!hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);
        }
    }

    void Update()
    {
        // Verifica se ultrapassou a altura de separa��o
        if (!hasSeparated && transform.position.y >= separationHeight)
        {
            Debug.Log("Separando o primeiro est�gio...");
            SeparateFirstStage();
        }
    }

    public void SeparateFirstStage()
    {
        if (separationPoint != null)
        {
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);
            gameObject.SetActive(false);

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Refer�ncia para a c�mera Cinemachine n�o atribu�da. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro ap�s a separa��o
        }
        else
        {
            Debug.LogError("separationPoint n�o encontrado. Verifique se foi atribu�do corretamente no Inspector.");
        }
    }

    public void StopThrust()
    {
        // Desliga a for�a de empuxo ap�s um tempo
        hasSeparated = true;
        rocketRigidbody.velocity = Vector3.zero;
        rocketRigidbody.angularVelocity = Vector3.zero;
    }
}*/


/*using UnityEngine;
using Cinemachine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public CinemachineVirtualCamera virtualCamera;

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a acelera��o conforme necess�rio

    void Start()
    {
        lastRocketPosition = transform.position;
        Invoke("StopThrust", 5f);
    }

    void FixedUpdate()
    {
        // Aplica uma for�a de empuxo suave
        if (!hasSeparated && rocketRigidbody.velocity.y >= 0)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);
        }
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
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);
            gameObject.SetActive(false);

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }
            else
            {
                Debug.LogError("Refer�ncia para a c�mera Cinemachine n�o atribu�da. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true;
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




/*using UnityEngine;
using Cinemachine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public GameObject segundoEstagioPrefab;
    public Transform separationPoint;
    public float thrustForce = 1000f;
    public float separationHeight = 50f;
    public CinemachineVirtualCamera virtualCamera;

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition;

    void Start()
    {
        rocketRigidbody.AddForce(Vector3.up * thrustForce);
        lastRocketPosition = transform.position;
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
            GameObject segundoEstagio = Instantiate(segundoEstagioPrefab, separationPoint.position, separationPoint.rotation);
            segundoEstagio.GetComponent<SecondStageController>().SetLastRocketPosition(transform.position);
            gameObject.SetActive(false);

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;

                // Congela a rota��o da c�mera
                virtualCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
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
}*/


/*using UnityEngine;

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
}*/


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

