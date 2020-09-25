using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelComplete;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem levelCompleteParticles;
    enum States { Alive, Dying, Trancending};
    States state = States.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (state == States.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (state != States.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":                
                break;
            case "Finish":
                LevelSuccess();
                break;
            default:               
                LevelDeath();
                break;
        }
    }
    private void LevelSuccess()
    {
        state = States.Trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelComplete);
        Invoke("LoadNextLevel", 1f); //TODO Paramaterize the time 
        levelCompleteParticles.Play();
    }

    private void LevelDeath()
    {
        state = States.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", 1.5f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        
        
        SceneManager.LoadScene(1);
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
                   
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; //resume rotation.
    }
    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust - Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            ApplyThrust(thrustThisFrame);

        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {      
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
    }
}
