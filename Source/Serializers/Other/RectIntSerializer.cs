using System.Runtime.Serialization;
using UnityEngine;

namespace SerializationSaver.Serializers.Other
{
    public class RectIntSerializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            RectInt target = (RectInt)obj;
            info.AddValue("x", target.x);
            info.AddValue("y", target.y);
            info.AddValue("w", target.width);
            info.AddValue("h", target.height);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            RectInt target = (RectInt)obj;
            target.x = (int)info.GetValue("x", typeof(int));
            target.y = (int)info.GetValue("y", typeof(int));
            target.width = (int)info.GetValue("w", typeof(int));
            target.height = (int)info.GetValue("h", typeof(int));
            return target;
        }
    }
}