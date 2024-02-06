using System.Collections;
using System.Collections.Generic;
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
    public GameObject rocketParticles; 

    private bool hasSeparated = false;
    private bool gameStarted = false; 
    private float currentThrust = 0f;
    public float acceleration = 5f; 
    private float elapsedTime = 0f; 

    void Start()
    {
        rocketRigidbody.isKinematic = true;
        rocketRigidbody.detectCollisions = false;
        PlaySFX();
    }

    private void PlaySFX()
    {
        if (!hasSeparated) 
        {
            SFXPool.Instance.Play(sFXType);
        }
    }

    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (gameStarted)
        {
            elapsedTime += Time.deltaTime;

            if (!hasSeparated && transform.position.y >= separationHeight)
            {
                SeparateFirstStage();
            }

            if (elapsedTime >= 5f)
            {
                EndThrust();
            }
        }
    }

    void FixedUpdate()
    {
        if (gameStarted && !hasSeparated)
        {
            currentThrust += Time.fixedDeltaTime * acceleration;
            float clampedThrust = Mathf.Clamp(currentThrust, 0f, thrustForce);
            rocketRigidbody.AddForce(Vector3.up * clampedThrust);

            if (rocketRigidbody.velocity.y > 0 && rocketParticles != null)
            {
                rocketParticles.SetActive(true);
            }
        }
    }

    void StartGame()
    {
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
            SFXPool.Instance.Stop();

            if (virtualCamera != null)
            {
                virtualCamera.Follow = segundoEstagio.transform;
                virtualCamera.LookAt = segundoEstagio.transform;
            }

            hasSeparated = true; 

            if (rocketParticles != null)
            {
                rocketParticles.SetActive(false);
            }
        }
    }

    void EndThrust()
    {
        currentThrust = 0f; 
    }
}