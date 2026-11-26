using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinorFail : MonoBehaviour
{
    private static WinorFail instance;
    public static WinorFail Instance => instance;
    private Collider[] players;
    private Vector3 center;
    private Vector3 scae;
    private int index;
    public bool isException;
    public int winnumber;
    public int failnumber;
    //防止因为帧检测太快而出现问题
    public bool controlspeed;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        center=new Vector3(448,42,98);
        scae = new Vector3(70, 45, 50);
        AllChange.Instance.ChangeManager(index++, AllChange.Instance.paths_bk.Dequeue(), AllChange.Instance.paths_player.Dequeue());
    }

    // Update is called once per frame
    void Update()
    {
        win_fail();
    }
    public void win_fail()
    {
        if(!controlspeed)
        {
            players = Physics.OverlapBox(center, scae, Quaternion.identity, 1 << 3);
            if (players.Length > 0)
            {
                if (isException)
                {
                    failnumber++;
                }
                else
                {
                    winnumber++;
                }
                AllChange.Instance.ChangeManager(index++, AllChange.Instance.paths_bk.Dequeue(), AllChange.Instance.paths_player.Dequeue());
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isException)
                {
                    winnumber++;
                }
                else
                {
                    failnumber++;
                }
                AllChange.Instance.ChangeManager(index++, AllChange.Instance.paths_bk.Dequeue(), AllChange.Instance.paths_player.Dequeue());
            }
        }
    }
}
