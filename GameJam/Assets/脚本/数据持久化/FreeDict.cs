using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class FreeDict<TKey,Tvalue> : Dictionary<TKey,Tvalue>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.Read();
        XmlSerializer tkey=new XmlSerializer(typeof(TKey));
        XmlSerializer tvalue=new XmlSerializer(typeof(Tvalue));
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            TKey tk=(TKey)tkey.Deserialize(reader);
            Tvalue tv=(Tvalue)tkey.Deserialize(reader);
            this.Add(tk, tv);
        }
        reader.Read();
    }

    public void WriteXml(XmlWriter writer)
    {
        XmlSerializer tkey=new XmlSerializer(typeof(TKey));
        XmlSerializer tvalue=new XmlSerializer(typeof(Tvalue));
        foreach(KeyValuePair<TKey,Tvalue> i in this)
        {
            tkey.Serialize(writer,i.Key);
            tvalue.Serialize(writer,i.Value);
        }
    }
}
