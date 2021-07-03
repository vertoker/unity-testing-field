using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.SerializationSaver
{
    public class ColorSerializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color target = (Color)obj;
            info.AddValue("r", target.r);
            info.AddValue("g", target.g);
            info.AddValue("b", target.b);
            info.AddValue("a", target.a);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color target = (Color)obj;
            target.r = (float)info.GetValue("r", typeof(float));
            target.g = (float)info.GetValue("g", typeof(float));
            target.b = (float)info.GetValue("b", typeof(float));
            target.a = (float)info.GetValue("a", typeof(float));
            return target;
        }
    }
}