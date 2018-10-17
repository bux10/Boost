using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    //TODO: Fix lighting bug when switching levels


    Rigidbody rb;
    AudioSource sound;

    enum State { ALIVE, DYING, TRANSCENDING, REFUELING, RESCUING };

    [SerializeField] State state = State.ALIVE;


    [SerializeField, Range(100,300)] float rcsThrust = 100f;
    [SerializeField, Range(1000,5000)] float mainThrust = 1000f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(state != State.DYING)
        {
            Thrust();
            Rotate();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if(state != State.DYING && state != State.TRANSCENDING)
        {
            switch (collision.gameObject.tag)
            {
                case "Finish":
                    state = State.TRANSCENDING;
                    Invoke("LoadNextLevel", 1.5f);
                    break;
                case "Friendly":
                    print("Safe");
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
                    Invoke("RestartFromBegining", 1.5f);
                    state = State.DYING;
                    if (sound.isPlaying)
                    {
                        sound.Stop();
                    }
                    break;
            }
        }
    }

    private void RestartFromBegining()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //TODO Allow for more than 2 levels
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!sound.isPlaying)
            {
                sound.Play();
            }
        }
        else
        {
            sound.Stop();
        }
    }

    private void Rotate()
    {
        //rb.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            rb.freezeRotation = true; //take manual control of rotation
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else
        if (Input.GetKey(KeyCode.D))
        {
            rb.freezeRotation = true; //take manual control of rotation
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){
            rb.freezeRotation = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

            //rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        //rb.freezeRotation = false; //return control to phsics engine
    }

}
