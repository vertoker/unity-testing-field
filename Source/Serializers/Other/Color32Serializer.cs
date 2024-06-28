using System.Runtime.Serialization;
using UnityEngine;

namespace SerializationSaver.Serializers.Other
{
    public class Color32Serializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color32 target = (Color32)obj;
            info.AddValue("r", target.r);
            info.AddValue("g", target.g);
            info.AddValue("b", target.b);
            info.AddValue("a", target.a);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color32 target = (Color32)obj;
            target.r = (byte)info.GetValue("r", typeof(byte));
            target.g = (byte)info.GetValue("g", typeof(byte));
            target.b = (byte)info.GetValue("b", typeof(byte));
            target.a = (byte)info.GetValue("a", typeof(byte));
            return target;
        }
    }
}