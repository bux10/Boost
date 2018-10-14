using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rb;
    AudioSource sound;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up);
        }



        if (Input.GetKeyDown(KeyCode.Space))
            sound.Play();
        if (Input.GetKeyUp(KeyCode.Space))
            sound.Stop();

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }else 
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }

    void FixedUpdate()
    {
        
    }
}
