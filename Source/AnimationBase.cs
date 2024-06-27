using UnityEngine;

namespace CoroutineAnimator
{
    public abstract class AnimationBase : MonoBehaviour
    {
        [SerializeField] protected AnimationData data;
        private Coroutine _coroutine;
        private bool _play;

        public bool Play
        {
            get => _play;
            set 
            {
                _play = value;
                if (_play) StartAnimation();
                else StopAnimation();
            }
        }
        private bool PlayDelegate() { return _play; }

        public AnimationData Data
        {
            get => data;
            set
            {
                StopAnimation();
                data = value;
            }
        }

        private void Start()
        {
            if (data.PlayOnStart)
                Play = true;
        }
        private void StartAnimation()
        {
            if (data == null)
                return;
            StopAnimation();
            _coroutine = AnimatorPerformer.StartAnimation(data, Render, PlayDelegate);
        }
        private void StopAnimation()
        {
            if (_coroutine != null)
                AnimatorPerformer.StopAnimation(_coroutine);
        }

        protected abstract void Render(int frame);
    }
}