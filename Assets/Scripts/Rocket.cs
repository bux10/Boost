using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rb;
    AudioSource sound;

    [SerializeField, Range(100,300)] float rcsThrust = 100f;
    [SerializeField, Range(500,1000)] float mainThrust = 100f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
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
            //rb.constraints = RigidbodyConstraints.FreezeRotationX;
            //rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        //rb.freezeRotation = false; //return control to phsics engine
    }

}
