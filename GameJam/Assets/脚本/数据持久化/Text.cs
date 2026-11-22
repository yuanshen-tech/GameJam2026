using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//用于记录失败玩家的姓名，并显示到公告板上
public class Text
{
    private static Text instance=new Text();
    public static Text Instance=>instance;
    private Text() { }
    public BroadManager broadWrite=new BroadManager();
    //用于清空测试数据
    public void ClearRecord()
    {
        broadWrite.Texts.Clear();
    }
    //添加数据
    public BroadManager DeadRecord(string name)
    {
        broadWrite.Texts.Add(name);
        XMlManager<BroadManager>.Instance.SaveData(broadWrite, "Notice board");
        return XMlManager<BroadManager>.Instance.LoadData(typeof(BroadManager), "Notice board");
    }
}
