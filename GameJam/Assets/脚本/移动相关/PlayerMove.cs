using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float rotatespeed=20;
    private bool isRotate;
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localRotation.x < 45 && transform.localRotation.x > -45)
        {
             transform.Rotate(Vector3.right, Time.deltaTime * rotatespeed * -Input.GetAxis("Mouse Y"));
        }
       
    }
}
