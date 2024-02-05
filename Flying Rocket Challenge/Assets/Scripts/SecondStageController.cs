using UnityEngine;
using Cinemachine;

public class SecondStageController : MonoBehaviour
{
    public CinemachineVirtualCamera rocketCamera; // Referência para a câmera que segue o foguete
    public CinemachineVirtualCamera secondStageCamera; // Referência para a câmera que seguirá o segundo estágio

    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 2f; // Taxa de aumento da velocidade (mais suave)
    public float maxVelocityIncreaseRate = 5f; // Taxa máxima de aumento da velocidade
    public float dragIncreaseRate = 0.1f; // Taxa de aumento do drag
    public float maxDrag = 1f; // Valor máximo de drag
    public float rotationAngle = 5f; // Ângulo de rotação
    public float rotationSpeed = 10f; // Velocidade de rotação
    public float rotationStopDelay = 3f; // Atraso para parar a rotação
    public float freezeRotationDelay = 5f; // Atraso para congelar as rotações

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
        // Iniciar temporizador para parar a rotação após o atraso
        Invoke("StopRotation", rotationStopDelay);
        // Iniciar temporizador para congelar as rotações após o atraso
        Invoke("FreezeRotations", freezeRotationDelay);
        // Definir a taxa de aumento de velocidade inicial como a taxa máxima
        currentVelocityIncreaseRate = maxVelocityIncreaseRate;
    }

    void Update()
    {
        // Verificar se o segundo estágio está descendendo e a rotação não foi interrompida
        if (isDescending && !rotationStopped)
        {
            // Aumentar a velocidade até atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento atual (suavizando)
                secondStageRigidbody.velocity += new Vector3(0f, currentVelocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                // Alternar a rotação do eixo Z entre -rotationAngle e rotationAngle
                float rotationZ = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                // Reduzir gradualmente a taxa de aumento de velocidade conforme o segundo estágio desce
                currentVelocityIncreaseRate = Mathf.Lerp(maxVelocityIncreaseRate, velocityIncreaseRate, Mathf.InverseLerp(initialVelocity, finalVelocity, secondStageRigidbody.velocity.y));
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }

        // Verificar se o segundo estágio está no chão
        if (!grounded && secondStageRigidbody.velocity.y <= 0)
        {
            grounded = true;
        }
    }

    // Método chamado quando o segundo estágio é ativado
    public void ActivateSecondStage()
    {
        // Desativa a câmera que segue o foguete
        rocketCamera.gameObject.SetActive(false);
        // Ativa a câmera que seguirá o segundo estágio
        secondStageCamera.gameObject.SetActive(true);
    }

    // Método para definir a posição inicial do segundo estágio com base na posição do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // Método para parar a descida do segundo estágio
    public void StopDescending()
    {
        isDescending = false;
    }

    // Método para parar a rotação do segundo estágio
    public void StopRotation()
    {
        rotationStopped = true;
    }

    // Método para congelar todas as rotações do segundo estágio
    private void FreezeRotations()
    {
        secondStageRigidbody.freezeRotation = true;
    }
}




/*using UnityEngine;
using Cinemachine;

public class SecondStageController : MonoBehaviour
{
    public CinemachineVirtualCamera rocketCamera; // Referência para a câmera que segue o foguete
    public CinemachineVirtualCamera secondStageCamera; // Referência para a câmera que seguirá o segundo estágio

    public Rigidbody secondStageRigidbody;
    public float initialVelocity = -10f; // Velocidade inicial para baixo
    public float finalVelocity = -30f; // Velocidade final para baixo
    public float velocityIncreaseRate = 5f; // Taxa de aumento da velocidade
    public float dragIncreaseRate = 0.1f; // Taxa de aumento do drag
    public float maxDrag = 1f; // Valor máximo de drag
    public float rotationAngle = 5f; // Ângulo de rotação
    public float rotationSpeed = 10f; // Velocidade de rotação
    public float rotationStopDelay = 3f; // Atraso para parar a rotação
    public float freezeRotationDelay = 5f; // Atraso para congelar as rotações
 

   

    private bool isDescending = true;
    private bool rotationStopped = false;
    private bool grounded = false;

    void Start()
    {
        // Configurar a velocidade inicial
        secondStageRigidbody.velocity = new Vector3(0f, initialVelocity, 0f);
        // Configurar o drag inicial
        secondStageRigidbody.drag = 0f;
        // Iniciar temporizador para parar a rotação após o atraso
        Invoke("StopRotation", rotationStopDelay);
        // Iniciar temporizador para congelar as rotações após o atraso
        Invoke("FreezeRotations", freezeRotationDelay);
    }

    void Update()
    {
        // Verificar se o segundo estágio está descendendo e a rotação não foi interrompida
        if (isDescending && !rotationStopped)
        {
            // Aumentar a velocidade até atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento
                secondStageRigidbody.velocity += new Vector3(0f, velocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                // Alternar a rotação do eixo Z entre -rotationAngle e rotationAngle
                float rotationZ = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            else
            {
                // Parar de aumentar a velocidade quando atingir a velocidade final
                isDescending = false;
            }
        }

        // Verificar se o segundo estágio está no chão
        if (!grounded && secondStageRigidbody.velocity.y <= 0)
        {
            grounded = true;
        }
    }

    // Método chamado quando o segundo estágio é ativado
    public void ActivateSecondStage()
    {
        // Desativa a câmera que segue o foguete
        rocketCamera.gameObject.SetActive(false);
        // Ativa a câmera que seguirá o segundo estágio
        secondStageCamera.gameObject.SetActive(true);
    }

    // Método para definir a posição inicial do segundo estágio com base na posição do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // Método para parar a descida do segundo estágio
    public void StopDescending()
    {
        isDescending = false;
    }

    // Método para parar a rotação do segundo estágio
    public void StopRotation()
    {
        rotationStopped = true;
    }

    // Método para congelar todas as rotações do segundo estágio
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
    public float maxDrag = 1f; // Valor máximo de drag
    public float rotationAngle = 5f; // Ângulo de rotação
    public float rotationSpeed = 10f; // Velocidade de rotação

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
        // Verificar se o segundo estágio está descendendo
        if (isDescending)
        {
            // Aumentar a velocidade até atingir a velocidade final
            if (secondStageRigidbody.velocity.y > finalVelocity)
            {
                // Aumentar a velocidade de acordo com a taxa de aumento
                secondStageRigidbody.velocity += new Vector3(0f, velocityIncreaseRate * Time.deltaTime, 0f);
                // Aumentar o drag para suavizar a descida
                if (secondStageRigidbody.drag < maxDrag)
                {
                    secondStageRigidbody.drag += dragIncreaseRate * Time.deltaTime;
                }

                // Alternar a rotação do eixo Z entre -rotationAngle e rotationAngle
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

    // Método para definir a posição inicial do segundo estágio com base na posição do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // Método para parar a descida do segundo estágio
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
    public float maxDrag = 1f; // Valor máximo de drag

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
        // Verificar se o segundo estágio está descendendo
        if (isDescending)
        {
            // Aumentar a velocidade até atingir a velocidade final
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

    // Método para definir a posição inicial do segundo estágio com base na posição do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // Método para parar a descida do segundo estágio
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
        // Verificar se o segundo estágio está descendendo
        if (isDescending)
        {
            // Aumentar a velocidade até atingir a velocidade final
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

    // Método para definir a posição inicial do segundo estágio com base na posição do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // Método para parar a descida do segundo estágio
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
    public float gravityDecreaseRate = 0.1f; // Taxa de diminuição da gravidade

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
        // Verificar se o segundo estágio está descendendo
        if (isDescending)
        {
            // Diminuir gradualmente a gravidade até atingir a gravidade final
            if (secondStageRigidbody.useGravity && secondStageRigidbody.velocity.y < 0)
            {
                secondStageRigidbody.useGravity = false;
            }
        }
    }

    // Método para definir a posição inicial do segundo estágio com base na posição do foguete
    public void SetLastRocketPosition(Vector3 position)
    {
        transform.position = position;
    }

    // Método para parar a descida do segundo estágio
    public void StopDescending()
    {
        isDescending = false;
    }
}*/


