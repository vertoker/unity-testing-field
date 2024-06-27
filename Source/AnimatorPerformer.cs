using System.Collections;
using UnityEngine;

namespace CoroutineAnimator
{
    public delegate void Render(int frame);
    public delegate float DeltaTime();
    public delegate bool Play();
    
    public class AnimatorPerformer : MonoBehaviour
    {
        public static AnimatorPerformer Instance;
        private static MonoBehaviour _behaviour;

        private void Awake()
        {
            Instance = this;
            _behaviour = this;
        }

        public static Coroutine StartAnimation(AnimationData data, Render render, Play play)
        {
            var routine = Animation(data, render, play, GetDeltaTime(data.Realtime));
            return _behaviour.StartCoroutine(routine);
        }
        public static void StopAnimation(Coroutine coroutine)
        {
            _behaviour.StopCoroutine(coroutine);
        }

        private static DeltaTime GetDeltaTime(bool realtime)
        {
            if (realtime)
                return UnscaledDeltaTime;
            return DeltaTime;
        }
        private static float DeltaTime() => Time.deltaTime;
        private static float UnscaledDeltaTime() => Time.unscaledDeltaTime;
        
        private static IEnumerator Animation(AnimationData data, Render render, Play play, DeltaTime deltaTime)
        {
            int length = data.FrameArray.Length - 1;
            float timer = 0; int frame = 0;
            render.Invoke(frame);

            while (frame < length || data.Loop)
            {
                yield return null;
                if (play.Invoke())
                {
                    timer += deltaTime();
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