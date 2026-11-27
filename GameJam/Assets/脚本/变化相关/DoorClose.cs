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
    public GameObject BiaoShi_Door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(WinorFail.Instance.index == 3)
        {
            Debug.Log(Vector3.Distance(player.transform.position, transform.position));
            Debug.Log("进入了");
            if (isStart&&Vector3.Distance(player.transform.position, transform.position) < 30&&!isEnd)
            {
                coroutine=StartCoroutine(DouDong());
            }
            if (!isStart&&Vector3.Distance(player.transform.position, transform.position) >16)
            {
                Debug.Log("有音效播放");
                audio_door.Play();
                isStart = true;
            }
        }
        if(WinorFail.Instance.index==4)
        {
            StopCoroutine(coroutine);
        }
    }
    IEnumerator DouDong()
    {
        isEnd = true;
        while(true)
        {
            transform.position = new Vector3(Random.Range(233.5f, 234.5f), BiaoShi_Door.transform.position.y, BiaoShi_Door.transform.position.z);
            yield return null;
        }
    }
}
