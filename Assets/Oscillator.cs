using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[DisallowMultipleComponent] //A behaviour that modifies the component below it.  eg the class is affected
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(0f, 0f, 0f);
    [SerializeField] float period = 2f;
    
    


    //todo remove from inspector later
    [SerializeField] // modifies the float movementFactor - can be stacked
    [Range(0, 1)]    // Modifies the float movementFactor - can be stacked
    float movementFactor;
    Vector3 startingPos;
    Vector3 offset;
        // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; } //Mathf.Epsilon is the smallest floating point num.  Use vice zero
        float cycles = Time.time / period; //grows continiously from 0

        const float tau = Mathf.PI * 2; // ~2pi
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = (rawSinWave / 2f) + 0.5f;        
        offset = movementFactor * movementVector;        
        transform.position = startingPos + offset;     
    }    

}
