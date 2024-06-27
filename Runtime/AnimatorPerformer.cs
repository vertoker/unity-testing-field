using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

namespace Game.Animation
{
    public delegate void Render(int frame);
    public delegate bool Play();
    public class AnimatorPerformer : MonoBehaviour
    {
        public static AnimatorPerformer _instance;
        private static MonoBehaviour _behaviour;

        private void Awake()
        {
            _instance = this;
            _behaviour = this;
        }

        public static Coroutine StartAnimation(AnimationData data, Render render, Play play)
        {
            if (data.Realtime)
                return _behaviour.StartCoroutine(AnimationUnscaled(data, render, play));
            else
                return _behaviour.StartCoroutine(Animation(data, render, play));
        }

        public static void StopAnimation(Coroutine coroutine)
        {
            _behaviour.StopCoroutine(coroutine);
        }

        private static IEnumerator Animation(AnimationData data, Render render, Play play)
        {
            int length = data.FrameArray.Length - 1;
            float timer = 0; int frame = 0;
            render.Invoke(frame);

            while (frame < length || data.Loop)
            {
                yield return null;
                if (play.Invoke())
                {
                    timer += Time.deltaTime;
                    int last = frame;
                    frame = (int)(timer * data.FPS);
                    if (last != frame)
                    {
                        //Debug.Log(frame.ToString() + " " + (frame % data.FrameArray.Length).ToString());
                        if (frame > length)
                            frame %= data.FrameArray.Length;

                        render.Invoke(frame);
                    }
                }
            }
        }
        private static IEnumerator AnimationUnscaled(AnimationData data, Render render, Play play)
        {
            int length = data.FrameArray.Length - 1;
            float timer = 0; int frame = 0;
            render.Invoke(frame);

            while (frame < length || data.Loop)
            {
                yield return null;
                if (play.Invoke())
                {
                    timer += Time.unscaledDeltaTime;
                    int last = frame;
                    frame = (int)(timer * data.FPS);
                    if (last != frame)
                    {
                        if (frame > length)
                            frame %= data.FrameArray.Length;
                        render.Invoke(frame);
                    }
                }
            }
        }
    }
}