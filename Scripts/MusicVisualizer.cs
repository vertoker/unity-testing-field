using System.Collections.Generic;
using UnityEngine;

public class MusicVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public float height = 10; 
    public int segments = 64;
    private List<Vector3> positions = new List<Vector3>();
    public InstanceOutput instanceOutput;
    private static float[] samples;
    private Transform tr;

    void Start()
    {
        tr = transform;
        samples = new float[segments];
        instanceOutput.StartRender(segments);
    }

    void Update()
    {
        var pos = tr.localPosition;
        positions.Clear();
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        for (var x = 0; x < segments; x++)
        {
            positions.Add(pos + new Vector3(x, samples[x] * height, 0));
        }

        instanceOutput.SetPositions(positions);
    }
}
