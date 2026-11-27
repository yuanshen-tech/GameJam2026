using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeException : ExcptionMgr<TimeException>
{
    public GameObject shizhong;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Exception(int index)
    {
        if(index==1)
        {
            base.Exception(index);
            shizhong.transform.rotation=Quaternion.AngleAxis(180,Vector3.right)*shizhong.transform.rotation;
        }
    }
}
