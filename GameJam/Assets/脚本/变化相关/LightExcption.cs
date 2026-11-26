using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExcption : ExcptionMgr<LightExcption>
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
        if(index==6)
        {
            base.Exception(index);
        }
    }
}
