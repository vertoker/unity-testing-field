using System.Collections;
using System.Collections.Generic;
using Game.Animation;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.25f;
    [SerializeField] private AnimationData[] dataIdle;
    [SerializeField] private AnimationData[] dataRun;
    private int direction = 0;
    private bool move = false;
    private Transform parent;
    private SpriteAnimation animator;

    private void Awake()
    {
        parent = transform;
        animator = GetComponent<SpriteAnimation>();
        AnimUpdate();
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 translation = new Vector3(horizontal, vertical, 0) * speed;
        parent.Translate(translation);

        bool nextMove = translation.sqrMagnitude != 0f;
        int nextDirection = direction;
        if (nextMove)
            nextDirection = DirectionToIndex(translation, 8);
        if (direction != nextDirection || move != nextMove)
        {
            direction = nextDirection;
            move = nextMove;
            AnimUpdate();
        }
    }
    private void AnimUpdate()
    {
        if (move)
            animator.SetAnimation(dataRun[direction]);
        else
            animator.SetAnimation(dataIdle[direction]);
        animator.PlayPause = true;
    }
    private int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized; float step = 360f / sliceCount;
        float angle = Vector2.SignedAngle(Vector2.up, normDir) + step / 2;
        if (angle < 0) { angle += 360; }
        return Mathf.FloorToInt(angle / step);
    }
}
