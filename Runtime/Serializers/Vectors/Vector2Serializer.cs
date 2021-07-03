using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.SerializationSaver
{
    public class Vector2Serializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2 target = (Vector2)obj;
            info.AddValue("x", target.x);
            info.AddValue("y", target.y);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 target = (Vector2)obj;
            target.x = (float)info.GetValue("x", typeof(float));
            target.y = (float)info.GetValue("y", typeof(float));
            return target;
        }
    }
}