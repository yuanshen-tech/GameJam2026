using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcptionMgr<T>:MonoBehaviour where T:ExcptionMgr<T>
{
    /// <summary>
    /// 异常的父类
    /// </summary>
    /// <param name="index"></param>
    private static ExcptionMgr<T> instance;
    public static ExcptionMgr<T> Instance=>instance;
    private void Awake()
    {
        instance = this as T;
    }
    //public RaycastHit hit;
    //private static ExcptionMgr instance=new ExcptionMgr();
    //public static ExcptionMgr Instance=>instance;
    //private ExcptionMgr() { }
    //给所有异常物体都挂上,做序号的判断即可
    public virtual void Exception(int index)
    {
        WinorFail.Instance.isException = true;
        //if(index==)
    }
}
