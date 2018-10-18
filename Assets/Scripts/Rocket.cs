using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;
    AudioSource sound;

    enum State { ALIVE, DYING, TRANSCENDING, REFUELING, RESCUING };

    State state = State.ALIVE;

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
        if(state != State.DYING)
        {
            RespondToThrustInput();
            RespondToRotationInput();
        }
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
            rb.freezeRotation = false;
            SetShipConstraints();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.DYING && state != State.TRANSCENDING)
        {
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
    }

    private void StartSuccessSequence()
    {
        state = State.TRANSCENDING;
        successPS.Play();
        sound.Stop();
        sound.PlayOneShot(successSound);
        Invoke("LoadNextLevel", 1.5f);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //TODO Allow for more than 2 levels
    }

    private void StartDeathSequence()
    {
        state = State.DYING;
        Invoke("RestartFromBegining", 1.5f);
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