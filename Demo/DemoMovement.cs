using System.Collections;
using System.Collections.Generic;
using Game.Map2D;
using UnityEngine;

public class DemoMovement : MonoBehaviour
{
    [SerializeField] private Map map;
    private Transform tr;
    private Camera cam;

    public void Awake()
    {
        tr = transform;
        cam = GetComponent<Camera>();
        map.ViewUpdate(cam);
    }

    public void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        tr.Translate(new Vector3(horizontal, vertical, 0));
        if (Input.GetKeyUp(KeyCode.Space))
            map.PlayPause();
    }
}