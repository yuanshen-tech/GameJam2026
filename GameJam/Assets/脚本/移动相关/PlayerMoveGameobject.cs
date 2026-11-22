using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveGameobject : MonoBehaviour
{
    public float movespeed = 10;
    public float rotatespeed = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * movespeed * Input.GetAxis("Vertical"));
        transform.Rotate(Vector3.up, Time.deltaTime * rotatespeed * Input.GetAxis("Mouse X"));
    }
}
