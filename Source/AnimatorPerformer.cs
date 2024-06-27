using System.Collections;
using UnityEngine;

namespace CoroutineAnimator
{
    public delegate void Render(int frame);
    public delegate float DeltaTime();
    public delegate bool Play();
    
    public class AnimatorPerformer : MonoBehaviour
    {
        [SerializeField] private bool debug;
        private static AnimatorPerformer _instance;
        
        private void Awake()
        {
            _instance = this;
        }

        public static Coroutine StartAnimation(AnimationData data, Render render, Play play)
        {
            var routine = Animation(data, render, play, GetDeltaTime(data.Realtime));
            return _instance.StartCoroutine(routine);
        }
        public static void StopAnimation(Coroutine coroutine)
        {
            _instance.StopCoroutine(coroutine);
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
            var length = data.FrameArray.Length - 1;
            var timer = 0f;
            var frame = 0;
            
            render.Invoke(frame);

            while (frame < length || data.Loop)
            {
                yield return null;
                if (!play.Invoke()) continue;
                timer += deltaTime();
                
                var last = frame;
                frame = (int)(timer * data.FPS);
                if (last == frame) continue;
                
                if (_instance.debug)
                    Debug.Log(string.Join(" ", data.name, frame, frame % data.FrameArray.Length));
                
                if (frame > length)
                    frame %= data.FrameArray.Length;

                render.Invoke(frame);
            }
        }
    }
}