using UnityEngine;
using Cinemachine;

public class SecondStageController : MonoBehaviour
{
    public CinemachineVirtualCamera rocketCamera; // Refer�ncia para a c�mera que segue o foguete
    public CinemachineVirtualCamera secondStageCamera; // Refer�ncia para a c�mera que seguir� o segundo est�gio

    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 2f; // Taxa de aumento da velocidade (mais suave)
    public float maxVelocityIncreaseRate = 5f; // Taxa m�xima de aumento da velocidade
    public float dragIncreaseRate = 0.1f; // Taxa de aumento do drag
    public float maxDrag = 1f; // Valor m�ximo de drag
    public float rotationAngle = 5f; // �ngulo de rota��o
    public float rotationSpeed = 10f; // Velocidade de rota��o
    public float rotationStopDelay = 3f; // Atraso para parar a rota��o
    public float freezeRotationDelay = 5f; // Atraso para congelar as rota��es

    private bool isDescending = true;
    private bool rotationStopped = false;
    private bool grounded = false;
    private float currentVelocityIncreaseRate; // Taxa de aumento de velocidade atual

    void Start()
    {
        // Configurar a velocidade inicial
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);
        // Configurar o drag inicial
        secondStageRigidbody.drag = 0f;
        // Iniciar temporizador para parar a rota��o ap�s o atraso
        Invoke("StopRotation", rotationStopDelay);
        // Iniciar temporizador para congelar as rota��es ap�s o atraso
        Invoke("FreezeRotations", freezeRotationDelay);
        // Definir a taxa de aumento de velocidade inicial como a taxa m�xima
        currentVelocityIncreaseRate = maxVelocityIncreaseRate;
    }

    void Update()
    {
        // Verificar se o segundo est�gio est� descendendo e a rota��o n�o foi interrompida
        if (isDescending && !rotationStopped)
        {
            // Aumentar a velocidade at� atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento atual (suavizando)
                secondStageRigidbody.velocity += new Vector3(0f, currentVelocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                // Alternar a rota��o do eixo Z entre -rotationAngle e rotationAngle
                float rotationZ = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                // Reduzir gradualmente a taxa de aumento de velocidade conforme o segundo est�gio desce
                currentVelocityIncreaseRate = Mathf.Lerp(maxVelocityIncreaseRate, velocityIncreaseRate, Mathf.InverseLerp(initialVelocity, finalVelocity, secondStageRigidbody.velocity.y));
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }

        // Verificar se o segundo est�gio est� no ch�o
        if (!grounded && secondStageRigidbody.velocity.y <= 0)
        {
            grounded = true;
        }
    }

    // M�todo chamado quando o segundo est�gio � ativado
    public void ActivateSecondStage()
    {
        // Desativa a c�mera que segue o foguete
        rocketCamera.gameObject.SetActive(false);
        // Ativa a c�mera que seguir� o segundo est�gio
        secondStageCamera.gameObject.SetActive(true);
    }

    // M�todo para definir a posi��o inicial do segundo est�gio com base na posi��o do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // M�todo para parar a descida do segundo est�gio
    public void StopDescending()
    {
        isDescending = false;
    }

    // M�todo para parar a rota��o do segundo est�gio
    public void StopRotation()
    {
        rotationStopped = true;
    }

    // M�todo para congelar todas as rota��es do segundo est�gio
    private void FreezeRotations()
    {
        secondStageRigidbody.freezeRotation = true;
    }
}




/*using UnityEngine;
using Cinemachine;

public class SecondStageController : MonoBehaviour
{
    public CinemachineVirtualCamera rocketCamera; // Refer�ncia para a c�mera que segue o foguete
    public CinemachineVirtualCamera secondStageCamera; // Refer�ncia para a c�mera que seguir� o segundo est�gio

    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 5f; // Taxa de aumento da velocidade
    public float dragIncreaseRate = 0.1f; // Taxa de aumento do drag
    public float maxDrag = 1f; // Valor m�ximo de drag
    public float rotationAngle = 5f; // �ngulo de rota��o
    public float rotationSpeed = 10f; // Velocidade de rota��o
    public float rotationStopDelay = 3f; // Atraso para parar a rota��o
    public float freezeRotationDelay = 5f; // Atraso para congelar as rota��es
 

   

    private bool isDescending = true;
    private bool rotationStopped = false;
    private bool grounded = false;

    void Start()
    {
        // Configurar a velocidade inicial
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);
        // Configurar o drag inicial
        secondStageRigidbody.drag = 0f;
        // Iniciar temporizador para parar a rota��o ap�s o atraso
        Invoke("StopRotation", rotationStopDelay);
        // Iniciar temporizador para congelar as rota��es ap�s o atraso
        Invoke("FreezeRotations", freezeRotationDelay);
    }

    void Update()
    {
        // Verificar se o segundo est�gio est� descendendo e a rota��o n�o foi interrompida
        if (isDescending && !rotationStopped)
        {
            // Aumentar a velocidade at� atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento
                secondStageRigidbody.velocity += new Vector3(0f, velocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                // Alternar a rota��o do eixo Z entre -rotationAngle e rotationAngle
                float rotationZ = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }

        // Verificar se o segundo est�gio est� no ch�o
        if (!grounded && secondStageRigidbody.velocity.y <= 0)
        {
            grounded = true;
        }
    }

    // M�todo chamado quando o segundo est�gio � ativado
    public void ActivateSecondStage()
    {
        // Desativa a c�mera que segue o foguete
        rocketCamera.gameObject.SetActive(false);
        // Ativa a c�mera que seguir� o segundo est�gio
        secondStageCamera.gameObject.SetActive(true);
    }

    // M�todo para definir a posi��o inicial do segundo est�gio com base na posi��o do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // M�todo para parar a descida do segundo est�gio
    public void StopDescending()
    {
        isDescending = false;
    }

    // M�todo para parar a rota��o do segundo est�gio
    public void StopRotation()
    {
        rotationStopped = true;
    }

    // M�todo para congelar todas as rota��es do segundo est�gio
    private void FreezeRotations()
    {
        secondStageRigidbody.freezeRotation = true;
    }
}*/




/*using UnityEngine;

public class SecondStageController : MonoBehaviour
{
    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 5f; // Taxa de aumento da velocidade
    public float dragIncreaseRate = 0.1f; // Taxa de aumento do drag
    public float maxDrag = 1f; // Valor m�ximo de drag
    public float rotationAngle = 5f; // �ngulo de rota��o
    public float rotationSpeed = 10f; // Velocidade de rota��o

    private bool isDescending = true;

    void Start()
    {
        // Configurar a velocidade inicial
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);
        // Configurar o drag inicial
        secondStageRigidbody.drag = 0f;
    }

    void Update()
    {
        // Verificar se o segundo est�gio est� descendendo
        if (isDescending)
        {
            // Aumentar a velocidade at� atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento
                secondStageRigidbody.velocity += new Vector3(0f, velocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                // Alternar a rota��o do eixo Z entre -rotationAngle e rotationAngle
                float rotationZ = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }
    }

    // M�todo para definir a posi��o inicial do segundo est�gio com base na posi��o do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // M�todo para parar a descida do segundo est�gio
    public void StopDescending()
    {
        isDescending = false;
    }
}*/




/*using UnityEngine;

public class SecondStageController : MonoBehaviour
{
    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 5f; // Taxa de aumento da velocidade
    public float dragIncreaseRate = 0.1f; // Taxa de aumento do drag
    public float maxDrag = 1f; // Valor m�ximo de drag

    private bool isDescending = true;

    void Start()
    {
        // Configurar a velocidade inicial
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);
        // Configurar o drag inicial
        secondStageRigidbody.drag = 0f;
    }

    void Update()
    {
        // Verificar se o segundo est�gio est� descendendo
        if (isDescending)
        {
            // Aumentar a velocidade at� atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento
                secondStageRigidbody.velocity += new Vector3(0f, velocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }
    }

    // M�todo para definir a posi��o inicial do segundo est�gio com base na posi��o do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // M�todo para parar a descida do segundo est�gio
    public void StopDescending()
    {
        isDescending = false;
    }
}*/





/*using UnityEngine;

public class SecondStageController : MonoBehaviour
{
    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 5f; // Taxa de aumento da velocidade

    private bool isDescending = true;

    void Start()
    {
        // Configurar a velocidade inicial
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);
    }

    void Update()
    {
        // Verificar se o segundo est�gio est� descendendo
        if (isDescending)
        {
            // Aumentar a velocidade at� atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento
                secondStageRigidbody.velocity += new Vector3(0f, velocityIncreaseRate * Time.deltaTime, 0f);
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }
    }

    // M�todo para definir a posi��o inicial do segundo est�gio com base na posi��o do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // M�todo para parar a descida do segundo est�gio
    public void StopDescending()
    {
        isDescending = false;
    }
}*/




/*using UnityEngine;

public class SecondStageController : MonoBehaviour
{
    public Rigidbody secondStageRigidbody;
    public float initialGravity = 9.81f; // Gravidade inicial
    public float finalGravity = 1f; // Gravidade final
    public float gravityDecreaseRate = 0.1f; // Taxa de diminui��o da gravidade

    private bool isDescending = true;

    void Start()
    {
        // Configurar a gravidade inicial
        secondStageRigidbody.useGravity = true;
        secondStageRigidbody.velocity = Vector3.zero;
        secondStageRigidbody.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        // Verificar se o segundo est�gio est� descendendo
        if (isDescending)
        {
            // Diminuir gradualmente a gravidade at� atingir a gravidade final
            if (secondStageRigidbody.useGravity && secondStageRigidbody.velocity.y < 0)
            {
                secondStageRigidbody.useGravity = false;
            }
        }
    }

    // M�todo para definir a posi��o inicial do segundo est�gio com base na posi��o do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // M�todo para parar a descida do segundo est�gio
    public void StopDescending()
    {
        isDescending = false;
    }
}*/


