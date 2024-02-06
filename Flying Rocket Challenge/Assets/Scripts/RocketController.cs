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
    public GameObject rocketParticles; // Objeto de partículas do foguete

    private bool hasSeparated = false;
    private bool gameStarted = false; // Controla se o jogo começou ou não
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a aceleração conforme necessário

    void Start()
    {
        // Desativa o foguete no início
        rocketRigidbody.isKinematic = true;
        rocketRigidbody.detectCollisions = false;
        PlaySFX();
    }

    private void PlaySFX()
    {
        if (!hasSeparated) // Verifica se o foguete já se separou antes de tocar o som
        {
            SFXPool.Instance.Play(sFXType);
        }
    }

    void Update()
    {
        // Verifica se o espaço foi pressionado para iniciar o jogo
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Executa apenas se o jogo começou
        if (gameStarted)
        {
            // Verifica se ultrapassou a altura de separação
            if (!hasSeparated && transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro estágio...");
                SeparateFirstStage();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplica uma força de empuxo suave apenas enquanto o objeto ainda não se separou
        if (gameStarted && !hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            // Verifica se o foguete está subindo e ativa as partículas
            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                rocketParticles.SetActive(true);
            }
        }
    }

    void StartGame()
    {
        // Ativa o foguete para começar o jogo
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
                Debug.LogError("Referência para a câmera Cinemachine não atribuída. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro após a separação

            // Desliga as partículas do foguete quando atinge a posição de separação
            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }

            // Para o som quando o foguete se separa
            SFXPool.Instance.Stop(); // Removido o argumento aqui
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
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
    public GameObject rocketParticles; // Objeto de partículas do foguete

    private bool hasSeparated = false;
    private bool gameStarted = false; // Controla se o jogo começou ou não
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a aceleração conforme necessário

    void Start()
    {
        // Desativa o foguete no início
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
        // Verifica se o espaço foi pressionado para iniciar o jogo
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Executa apenas se o jogo começou
        if (gameStarted)
        {
            // Verifica se ultrapassou a altura de separação
            if (!hasSeparated && transform.position.y >= separationHeight)
            {
                Debug.Log("Separando o primeiro estágio...");
                SeparateFirstStage();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplica uma força de empuxo suave apenas enquanto o objeto ainda não se separou
        if (gameStarted && !hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            // Verifica se o foguete está subindo e ativa as partículas
            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                rocketParticles.SetActive(true);
            }
        }
    }

    void StartGame()
    {
        // Ativa o foguete para começar o jogo
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
                Debug.LogError("Referência para a câmera Cinemachine não atribuída. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro após a separação

            // Desliga as partículas do foguete quando atinge a posição de separação
            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
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
    public GameObject rocketParticles; // Objeto de partículas do foguete

    private bool hasSeparated = false;
    private Vector3 lastRocketPosition;
    private float currentThrust = 0f;
    public float acceleration = 5f; // Ajuste a aceleração conforme necessário

    void Start()
    {
        lastRocketPosition = transform.position;
        Invoke("StopThrust", 5f);
    }

    void FixedUpdate()
    {
        // Aplica uma força de empuxo suave apenas enquanto o objeto ainda não se separou
        if (!hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            // Verifica se o foguete está subindo e ativa as partículas
            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                //Debug.Log("Ativando partículas do foguete.");
                rocketParticles.SetActive(true);
            }
        }
    }



    void Update()
    {
        // Verifica se ultrapassou a altura de separação
        if (!hasSeparated && transform.position.y >= separationHeight)
        {
            Debug.Log("Separando o primeiro estágio...");
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
                Debug.LogError("Referência para a câmera Cinemachine não atribuída. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro após a separação

            // Desliga as partículas do foguete quando atinge a posição de separação
            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
        }
    }


    public void StopThrust()
    {
        // Desliga a força de empuxo após um tempo
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
    public float acceleration = 5f; // Ajuste a aceleração conforme necessário

    void Start()
    {
        lastRocketPosition = transform.position;
        Invoke("StopThrust", 5f);
    }

    void FixedUpdate()
    {
        // Aplica uma força de empuxo suave apenas enquanto o objeto ainda não se separou
        if (!hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);
        }
    }

    void Update()
    {
        // Verifica se ultrapassou a altura de separação
        if (!hasSeparated && transform.position.y >= separationHeight)
        {
            Debug.Log("Separando o primeiro estágio...");
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
                Debug.LogError("Referência para a câmera Cinemachine não atribuída. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true; // Define como verdadeiro após a separação
        }
        else
        {
            Debug.LogError("separationPoint não encontrado. Verifique se foi atribuído corretamente no Inspector.");
        }
    }

    public void StopThrust()
    {
        // Desliga a força de empuxo após um tempo
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
    public float acceleration = 5f; // Ajuste a aceleração conforme necessário

    void Start()
    {
        lastRocketPosition = transform.position;
        Invoke("StopThrust", 5f);
    }

    void FixedUpdate()
    {
        // Aplica uma força de empuxo suave
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
                Debug.Log("Separando o primeiro estágio...");
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
                Debug.LogError("Referência para a câmera Cinemachine não atribuída. Verifique se foi configurada corretamente no Inspector.");
            }

            hasSeparated = true;
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
                Debug.Log("Separando o primeiro estágio...");
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

                // Congela a rotação da câmera
                virtualCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
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
}*/


/*using UnityEngine;

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

