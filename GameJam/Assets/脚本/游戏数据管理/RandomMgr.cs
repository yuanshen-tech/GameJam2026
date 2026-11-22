using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMgr : MonoBehaviour
{
    //目前没用，可能之后会用上
    private static RandomMgr instance;
    public static RandomMgr Instance=>instance;
    public LinkedList<int> indexs=new LinkedList<int>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1;i<=8;i++)
        {
            indexs.AddLast(i);
        }
    }
    private void Awake()
    {
        instance=this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //如果不是检测游戏轮数，而是检测链表是否为空会用上
    public void GetIndex(int index,ref int i)
    {
        LinkedListNode<int> node=indexs.First;
        while (node!=null)
        {
            if(node.Value==index)
            {
                i++;
                indexs.Remove(node);
            }
            node = node.Next;
        }
        
    }
}
