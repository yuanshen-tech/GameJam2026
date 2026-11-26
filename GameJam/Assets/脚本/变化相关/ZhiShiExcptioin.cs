using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhiShiExcptioin : ExcptionMgr<ZhiShiExcptioin>
{
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
        if(index==7)
        {
            base.Exception(index);
        }
    }
}
