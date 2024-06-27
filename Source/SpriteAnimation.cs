using UnityEngine;

namespace CoroutineAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : AnimationBase
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        protected override void Render(int frame)
        {
            _spriteRenderer.sprite = data.FrameArray[frame];
        }
    }
}