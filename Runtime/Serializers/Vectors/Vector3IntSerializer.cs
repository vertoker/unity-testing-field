using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.SerializationSaver
{
    public class Vector3IntSerializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3Int target = (Vector3Int)obj;
            info.AddValue("x", target.x);
            info.AddValue("y", target.y);
            info.AddValue("z", target.z);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3Int target = (Vector3Int)obj;
            target.x = (int)info.GetValue("x", typeof(int));
            target.y = (int)info.GetValue("y", typeof(int));
            target.z = (int)info.GetValue("z", typeof(int));
            return target;
        }
    }
}