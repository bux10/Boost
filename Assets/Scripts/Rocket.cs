using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource sound;

    enum State { ALIVE, DYING, TRANSCENDING, REFUELING, RESCUING };

    State state = State.ALIVE;

    bool collisionToggle = true;

    [SerializeField] float levelLoadDelay = 2f;

    //Thrust
    [SerializeField, Range(100,300)] float rcsThrust = 100f;
    [SerializeField, Range(1000,5000)] float mainThrust = 1000f;

    //Sounds
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip deathExplosion;

    //Particles
    [SerializeField] ParticleSystem thrustPS;
    [SerializeField] ParticleSystem explosionPS;
    [SerializeField] ParticleSystem successPS;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Debug.isDebugBuild) RespondToDebugKeys(); //Only when debug is on

        if (state != State.DYING)
        {
            RespondToThrustInput();
            RespondToRotationInput();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) LoadNextLevel();
        if (Input.GetKeyDown(KeyCode.C)) ToggleCollision();
    }

    private void ToggleCollision()
    {
        collisionToggle = !collisionToggle;
    }

    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            thrustPS.Stop();
            sound.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rb.AddRelativeForce(Vector3.up * thrustThisFrame);
        //if (!thrustPS.isPlaying)
        //{
            thrustPS.Play();
        //}
        if (!sound.isPlaying)
        {
            sound.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotationInput()
    {

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThisFrame);
        }
        else
        if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThisFrame);
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            SetShipConstraints();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.angularVelocity = Vector3.zero; //remove any residual rotation from physics engine
        transform.Rotate(Vector3.forward * rotationThisFrame);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state == State.DYING || collisionToggle != true) return;

            switch (collision.gameObject.tag)
            {
                case "Finish":
                    StartSuccessSequence();
                    break;
                case "Friendly":
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    break;
                case "Fuel":
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    state = State.REFUELING;
                    break;
                case "Rescue":
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    state = State.RESCUING;
                    break;
                default:
                    StartDeathSequence();
                    break;
            }
    }

    private void StartSuccessSequence()
    {
        state = State.TRANSCENDING;
        successPS.Play();
        sound.Stop();
        sound.PlayOneShot(successSound);
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings) nextScene = 0;  //if on last level go to first level buildIndex 0
        SceneManager.LoadScene(nextScene);
    }

    private void StartDeathSequence()
    {
        state = State.DYING;
        Invoke("RestartFromBegining", levelLoadDelay);
        explosionPS.Play();
        sound.Stop();
        sound.PlayOneShot(deathExplosion);
    }

    private void RestartFromBegining()
    {
        SceneManager.LoadScene(0);
    }

    private void SetShipConstraints()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }
}