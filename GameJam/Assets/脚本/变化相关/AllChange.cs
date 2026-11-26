using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChange : MonoBehaviour
{
    public Queue<string> paths_player = new Queue<string>();
    public Queue<string> paths_bk = new Queue<string>();
    public GameObject player;
    public float born_x=80;
    public float born_z=58;
    public float born_y=60;
    //可用同步加载
    public AudioSource audioSource_bk;
    public AudioSource audioSource_player;
    private static AllChange instance;
    public static AllChange Instance=>instance;
    private void Awake()
    {
        instance = this;
        paths_player.Enqueue("PlayerClip");
        paths_player.Enqueue("PlayerClip1");
        paths_player.Enqueue("PlayerClip2");
        paths_player.Enqueue("PlayerClip");
        paths_player.Enqueue("PlayerClip1");
        paths_player.Enqueue("PlayerClip2");
        paths_player.Enqueue("PlayerClip");
        paths_player.Enqueue("PlayerClip1");
        paths_player.Enqueue("PlayerClip2");
        paths_player.Enqueue("PlayerClip");

        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip"); 
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip"); 
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip");
        paths_bk.Enqueue("BkClip");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //每次序号改变时调用,不同的音乐只需传入两个字符串，改变同步加载内容即可，可在选择往前走或回到原点时调用
    public void ChangeManager(int index,string bk,string player_)
    {
        Debug.Log(bk);
        Debug.Log(player_);
        Debug.Log(index);
        WinorFail.Instance.controlspeed=true;
        if(index==9)
        {
            if(WinorFail.Instance.winnumber>WinorFail.Instance.failnumber)
            {
                //胜利界面
                Debug.Log(WinorFail.Instance.winnumber);
                Debug.Log(WinorFail.Instance.failnumber);
            }
            else
            {
                //失败界面
            }
        }
        if(index!=9)
        {
            born_y -= 2;
            audioSource_bk.clip = Resources.Load<AudioClip>(bk);
            audioSource_bk.Play();
            audioSource_player.clip = Resources.Load<AudioClip>(player_);
            audioSource_player.Play();
        }
        if(index==3)
        {
            WinorFail.Instance.isException = false;
        }
        //调用异常物体的方法
        switch (index)
        {
            case 0:
                TimeException.Instance.Exception(index);
                break;
            case 1:
                DoorException.Instance.Exception(index);
                break;
            case 2:
                XiaoFangExcption.Instance.Exception(index);
                break;
            case 4:
                WindowExcption.Instance.Exception(index);
                break;
            case 5:
                BoardExcption.Instance.Exception(index);
                break;
            case 6:
                LightExcption.Instance.Exception(index);
                break;
            case 7:
                ZhiShiExcptioin.Instance.Exception(index);
                break;
            case 8:
                ObjectExcption.Instance.Exception(index);
                break;
        }
        player.transform.position=new Vector3(born_x,born_y,born_z);
        player.transform.rotation=Quaternion.identity;
        WinorFail.Instance.controlspeed = false;
    }
}
