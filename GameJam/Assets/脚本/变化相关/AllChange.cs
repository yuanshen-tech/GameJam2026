using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChange : MonoBehaviour
{
    public Queue<string> paths_player = new Queue<string>();
    public GameObject player;
    public float born_x=80;
    public float born_z=58;
    public float born_y=58;
    //可用同步加载
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
    public void ChangeManager(int index,string player_)
    {
        Debug.Log(player_);
        Debug.Log(index);
        if(index==9)
        {
            if(WinorFail.Instance.winnumber>WinorFail.Instance.failnumber)
            {
                //胜利界面
                Success.Instance.ShowEnding();
            }
            else
            {
                //失败界面
                Fail.Instance.ShowEnding();
            }
        }
        if(index!=9)
        {
            born_y-=0.3f;
            AudioClip playerClip = Resources.Load<AudioClip>(player_);
            if (playerClip != null)
            {
                audioSource_player.clip = playerClip;
                audioSource_player.Play();
            }
        }
        if(index==3)
        {
            WinorFail.Instance.isException = false;
        }
        //调用异常物体的方法
        switch (index)
        {
            case 1:
                TimeException.Instance.Exception(index);
                break;
            case 2:
                DoorException.Instance.Exception(index);
                break;
            case 3:
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
                LightExcption.Instance.Exception(index);
                break;
            case 8:
                ObjectExcption.Instance.Exception(index);
                break;
        }
        player.transform.position=new Vector3(born_x,born_y,born_z);
        player.transform.rotation=Quaternion.identity;
        player.transform.GetChild(0).gameObject.transform.rotation=Quaternion.identity;
        //立即同步物理世界，避免本帧/下一帧的 OverlapBox 还认为玩家在判定区里
        Physics.SyncTransforms();
    }
}
