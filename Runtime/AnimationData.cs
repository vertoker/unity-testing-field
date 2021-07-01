using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    [CreateAssetMenu(menuName = "Animator/New Anim", fileName = "Animation", order = 0)]
    public class AnimationData : ScriptableObject
    {
        [SerializeField] private Sprite[] _frameArray;
        [SerializeField] private bool _realtime = false;
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _loop = true;
        [SerializeField] private int _initSprite = 0;
        [SerializeField] private int _fps = 30;

        public Sprite this[int index] => _frameArray[index];
        public Sprite[] FrameArray => _frameArray;
        public bool Realtime => _realtime;
        public bool PlayOnStart => _playOnStart;
        public bool Loop => _loop;
        public int FPS => _fps;
        public int InitSprite => _initSprite;
    }
}