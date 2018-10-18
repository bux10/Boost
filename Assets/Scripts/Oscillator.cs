using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(3f,3f,3f);
    [Range(2,10), SerializeField] float period = 2;
    [Range(0, 1), SerializeField] float movementFactor;

    Vector3 startingPos;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        offset = movementVector * movementFactor;
        transform.position = startingPos + offset;

    }
}
