using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos; //must be stored for absolute movement

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //We use Epsilon instead of zero because we're comparing it against a floating point variable
        //A floating point comparison can lead to odd behavior because of the large number of decimals stored
        if (period <= Mathf.Epsilon) { return; } 

        float cycles = Time.time / period; //grows continually from 0

        const float tau = Mathf.PI * 2; //Just a constant value of 6.28 (2Pi)
        float rawSinWave = Mathf.Sin(cycles * tau); 

        movementFactor = (rawSinWave / 2f) + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
