using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMlManager<T> where T : class
{
    private static XMlManager<T> instance=new XMlManager<T>();
    public static XMlManager<T> Instance=>instance;
    private XMlManager() { } 
    public void SaveData(T data,string name)
    {
        string path=Application.persistentDataPath+"/"+name+".xml";
        using(StreamWriter sw = new StreamWriter(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(sw, data);
        }
    }
    public T LoadData(Type type,string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".xml";
        if(!File.Exists(path))
        {
            path=Application.streamingAssetsPath + "/" + name + ".xml";
        }
        using (StreamReader sr = new StreamReader(path))
        {
            XmlSerializer serializer1=new XmlSerializer(typeof(T));
            return serializer1.Deserialize(sr) as T;
        }
    }
}
