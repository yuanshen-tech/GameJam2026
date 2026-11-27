using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowExcption : ExcptionMgr<WindowExcption>
{
    public GameObject xiaofang;
    public GameObject window_excption;
    public GameObject window_normal;
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
       if(index==4)
       {
           base.Exception(index);
           xiaofang.SetActive(true);
           window_excption.SetActive(true);
           window_normal.SetActive(false);
       }
    }
}
