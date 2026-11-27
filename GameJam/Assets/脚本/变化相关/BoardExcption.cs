using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardExcption : ExcptionMgr<BoardExcption>
{
    public GameObject normal;
    public GameObject except;
    public GameObject window_excpion;
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
        if(index==5)
        {
            base.Exception(index);
            normal.SetActive(false);
            except.SetActive(true);
            window_excpion.SetActive(false);
            window_normal.SetActive(true);
        }
    }
}
