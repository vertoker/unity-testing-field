using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.SerializationSaver
{
    public class Texture2DSerializer : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Texture2D target = (Texture2D)obj;
            int width = target.width;
            int height = target.height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = target.GetPixel(i, j);
                    info.AddValue(string.Join(":", i, j, "r"), color.r);
                    info.AddValue(string.Join(":", i, j, "g"), color.g);
                    info.AddValue(string.Join(":", i, j, "b"), color.b);
                    info.AddValue(string.Join(":", i, j, "a"), color.a);
                }
            }
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Texture2D target = (Texture2D)obj;
            int width = target.width;
            int height = target.height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = new Color
                    {
                        r = (float)info.GetValue(string.Join(":", i, j, "r"), typeof(float)),
                        g = (float)info.GetValue(string.Join(":", i, j, "g"), typeof(float)),
                        b = (float)info.GetValue(string.Join(":", i, j, "b"), typeof(float)),
                        a = (float)info.GetValue(string.Join(":", i, j, "a"), typeof(float))
                    };
                    target.SetPixel(i, j, color);
                }
            }
            return target;
        }
    }
}