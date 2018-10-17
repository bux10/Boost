using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    //[SerializeField] float period = 2f;


    [Range(2,10), SerializeField] float period;

    Vector3 startingPos;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
        period = 5f;
	}
	
	// Update is called once per frame
	void Update () {

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        offset = movementVector * rawSinWave;
        transform.position = startingPos + offset;

    }
}
