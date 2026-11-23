using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float rotatespeed=20;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.x<45&&transform.rotation.x>-45)
        {
            transform.Rotate(Vector3.right, Time.deltaTime * rotatespeed * Input.GetAxis("Mouse Y"));
        }
    }
}
