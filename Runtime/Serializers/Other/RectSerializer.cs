using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.SerializationSaver
{
    public class RectSerializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Rect target = (Rect)obj;
            info.AddValue("x", target.x);
            info.AddValue("y", target.y);
            info.AddValue("w", target.width);
            info.AddValue("h", target.height);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Rect target = (Rect)obj;
            target.x = (float)info.GetValue("x", typeof(float));
            target.y = (float)info.GetValue("y", typeof(float));
            target.width = (float)info.GetValue("w", typeof(float));
            target.height = (float)info.GetValue("h", typeof(float));
            return target;
        }
    }
}