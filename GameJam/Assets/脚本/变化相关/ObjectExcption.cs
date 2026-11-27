using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExcption : ExcptionMgr<ObjectExcption>
{
    public GameObject zhishi_excption;
    public GameObject zhishi_normal;
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
        if(index==8)
        {
            base.Exception(index);
            zhishi_excption.SetActive(false);
            zhishi_normal.SetActive(true);
        }
    }
}
