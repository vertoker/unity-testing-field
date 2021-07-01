using UnityEngine;

namespace Game.Animation
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
            _spriteRenderer.sprite = _data.FrameArray[frame];
        }
    }
}