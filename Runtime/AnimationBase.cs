using UnityEngine;

namespace Game.Animation
{
    public class AnimationBase : MonoBehaviour
    {
        [SerializeField] protected AnimationData _data;
        private Coroutine coroutine;
        private bool _play = false;

        public bool PlayPause
        {
            get { return _play; }
            set 
            {
                _play = value;
                if (_play)
                    StartAnimation();
                else
                    StopAnimation();
            }
        }
        public void SetAnimation(AnimationData data)
        {
            StopAnimation();
            _data = data;
        }

        private void Start()
        {
            PlayPause = _data.PlayOnStart;
        }
        private void StartAnimation()
        {
            if (_data == null)
                return;
            StopAnimation();
            coroutine = AnimatorPerformer.StartAnimation(_data, Render, Play);
        }
        private void StopAnimation()
        {
            if (coroutine != null)
                AnimatorPerformer.StopAnimation(coroutine);
        }
        protected virtual void Render(int frame)
        {

        }
        private bool Play()
        { return _play; }
    }
}