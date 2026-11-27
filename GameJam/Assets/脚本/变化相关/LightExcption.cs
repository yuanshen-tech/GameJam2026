using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExcption : ExcptionMgr<LightExcption>
{
    public GameObject normal;
    public GameObject except;
    public GameObject[] lights;
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
            normal.SetActive(true);
            except.SetActive(false);
            oroutine = StartCoroutine(TheLight(lights));
        }
        if(index==7)
        {
            StopCoroutine(oroutine);
            for(int i=0; i<lights.Length; i++)
            {
                lights[i].SetActive(true);
            }
        }
    }
    IEnumerator TheLight(GameObject[] lights)
    {
        while(true)
        {
            for(int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(false);
                yield return new WaitForSeconds(2);
            }
            for (int i = lights.Length-1; i>=0; i--)
            {
                lights[i].SetActive(true);
                yield return new WaitForSeconds(2);
            }
        }
    }
}
