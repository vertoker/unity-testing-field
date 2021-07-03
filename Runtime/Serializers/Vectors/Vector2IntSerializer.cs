using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.SerializationSaver
{
    public class Vector2IntSerializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2Int target = (Vector2Int)obj;
            info.AddValue("x", target.x);
            info.AddValue("y", target.y);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2Int target = (Vector2Int)obj;
            target.x = (int)info.GetValue("x", typeof(int));
            target.y = (int)info.GetValue("y", typeof(int));
            return target;
        }
    }
}