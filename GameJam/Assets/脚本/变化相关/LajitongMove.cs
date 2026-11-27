using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LajitongMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(WinorFail.Instance.index==9)
        {
            if(Vector3.Distance(transform.position,player.transform.position)<5)
            {
                transform.position=player.transform.position+player.transform.forward*1.5f;
            }
        }
    }
}
