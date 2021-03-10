using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float mainThrust = 25f;
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] AudioClip mainEngineAudio;
    [SerializeField] AudioClip levelSuccessAudio;
    [SerializeField] AudioClip deathAudio;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    //Cached references
    Rigidbody rocketRigidBody;
    AudioSource audioSource;

    enum State {  Alive, Dying, Transcending }
    State state = State.Alive;

    bool collisionsAreEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rocketRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Rotate();
            Thrust();
        }

        //Debug builds are helpful because we can use debug keys
        //Make sure to turn it on or off in the final build menu
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //toggle collision
            collisionsAreEnabled = !collisionsAreEnabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Guard condition to keep anything else from happening (also saves on processor power)
        if (state != State.Alive || !collisionsAreEnabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Nothing needs to happen here
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
        
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelSuccessAudio);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        mainEngineParticles.Stop();
        audioSource.PlayOneShot(deathAudio);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay); 
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == (SceneManager.sceneCountInBuildSettings))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
         
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ManuallyRotate(rcsThrust * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D)) 
        {
            ManuallyRotate(-rcsThrust * Time.deltaTime);
        }
    }

    private void ManuallyRotate(float rotationThisFrame)
    {
        rocketRigidBody.freezeRotation = true; //take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rocketRigidBody.freezeRotation = false; //resume physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rocketRigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); //Could add '* Time.deltaTime' to make it frame-independent, not necessary right now
        //I'm checking to see if the source is already playing so that the audio doesn't layer
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineAudio);
        }

         mainEngineParticles.Play();
    }
}
