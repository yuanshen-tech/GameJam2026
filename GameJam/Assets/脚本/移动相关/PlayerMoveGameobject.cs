using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveGameobject : MonoBehaviour
{
    public float movespeed = 10;
    public float rotatespeed = 30;
    //[Tooltip("身体半径，射线检测时往前多探的距离，防止贴墙穿模")]
    public float bodyRadius = 0.5f;
    private float ra=1;
    void Start()
    {
        Cursor.visible = false;
        //Cursor.lockState=CursorLockMode.Locked;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        // 鼠标 X 旋转
        transform.Rotate(Vector3.up, Time.deltaTime * rotatespeed * Input.GetAxis("Mouse X"));
        transform.Translate(Vector3.forward * Time.deltaTime * movespeed * ra * Input.GetAxis("Vertical"));
        if (Physics.Raycast(transform.position, transform.forward, bodyRadius, 1 << 6))
        {
            Debug.Log("前面检测到了");
            if (Input.GetAxis("Vertical") > 0)
            {
                ra = 0;
            }
            else
            {
                ra = 1;
            }
        }
        //if (Physics.Raycast(transform.position, -transform.forward, bodyRadius, 1 << 6))
        //{
        //    Debug.Log("后面检测到了");
        //    if (Input.GetAxis("Vertical") < 0)
        //    {
        //        ra = 0;
        //    }
        //    else
        //    {
        //        ra = 1;
        //    }
        //}

        //// WASD 前后移动 - 保留 transform.Translate 风格，但先用射线探测墙
        //float v = Input.GetAxis("Vertical");
        //Vector3 delta = transform.forward * v * movespeed * Time.deltaTime;

        //if (delta.sqrMagnitude > 0f)
        //{
        //    // 从胸口高度朝移动方向发射线，距离 = 本帧位移 + 身体半径
        //    // 命中则不移动（贴墙就停下，不会穿过去）
        //    Vector3 origin = transform.position + Vector3.up * 1f;
        //    float dist = delta.magnitude + bodyRadius;
        //    if (!Physics.Raycast(origin, delta.normalized, dist))
        //    {
        //        transform.position += delta;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.I))
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}
