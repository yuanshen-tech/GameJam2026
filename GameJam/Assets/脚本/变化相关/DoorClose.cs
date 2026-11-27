using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClose : MonoBehaviour
{
    public GameObject player;
    private int index;
    private bool isStart;
    private bool isEnd;
    public AudioSource audio_door;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(WinorFail.Instance.index == 2)
        {
            if (isStart&&Vector3.Distance(player.transform.position, transform.position) < 30&&!isEnd)
            {
                coroutine=StartCoroutine(DouDong());
            }
            if (!isStart&&Vector3.Distance(player.transform.position, transform.position) >100)
            {
                audio_door.Play();
                isStart = true;
            }
        }
        if(WinorFail.Instance.index==3)
        {
            StopCoroutine(coroutine);
        }
    }
    IEnumerator DouDong()
    {
        isEnd = true;
        while(true)
        {
            transform.position = new Vector3(Random.Range(83.5f, 84.5f), transform.position.y, transform.position.z);
            yield return null;
        }
    }
}
