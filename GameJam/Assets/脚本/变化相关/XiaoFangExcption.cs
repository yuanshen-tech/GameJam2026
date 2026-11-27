using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaoFangExcption : ExcptionMgr<XiaoFangExcption>
{
    // Start is called before the first frame update
    public AudioSource audio_door;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Exception(int index)
    {
        if(index==2)
        {
            base.Exception(index);
            audio_door.Pause();
           
        }
    }
}
