using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSpin : MonoBehaviour
{

    private float rotationSpeed = 250;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}
