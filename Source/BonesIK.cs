using UnityEngine;

namespace InverseKinematic
{
    public class BonesIK : MonoBehaviour
    {
        [SerializeField] protected Transform target;
        [SerializeField] protected Transform originBone;
        [SerializeField] protected bool initializeOnAwake = true;
        [SerializeField] protected bool instantiateBones = true;

        [Header("Animation Parameters")]
        [SerializeField] protected int iterations = 10;
        [SerializeField] protected float delta = 0.001f;

        [SerializeField] protected Transform[] joints;
        [SerializeField] protected Transform[] bones;
        [SerializeField] protected Vector3[] bonesScales;
        [SerializeField] protected float[] bonesLength;
        [SerializeField] protected Vector3[] positions;
        [SerializeField] protected int length;
        
        private float fullLength;
        
        private void Awake()
        {
            if (initializeOnAwake)
                Initialize();
        }
        public virtual void Initialize()
        {
            fullLength = 0;
            length = transform.childCount - 1;
            bonesLength = new float[length];
            bonesScales = new Vector3[length];
            positions = new Vector3[length + 1];
            joints = new Transform[length + 1];//Inverted
            if (instantiateBones)
                bones = new Transform[length];//Inverted

            joints[length] = transform.GetChild(0);
            for (int i = length - 1; i >= 0; i--)
            {
                joints[i] = transform.GetChild(length - i);
                
                if (instantiateBones)
                    bones[i] = Instantiate(originBone, transform);
                
                bonesLength[i] = (joints[i].position - joints[i + 1].position).magnitude;
                bonesScales[i] = new Vector3(bones[i].localScale.x, bones[i].localScale.y, bonesLength[i]);
                fullLength += bonesLength[i];
            }
            
            LateUpdate();

            for (int i = 0; i < length; i++)
                bones[i].gameObject.SetActive(true);
        }

        private void LateUpdate()
        {
            for (int i = 0; i <= length; i++)
                positions[i] = joints[i].position;

            var targetPosition = target.position;
            if ((targetPosition - joints[0].position).sqrMagnitude >= fullLength * fullLength)
            {
                var direction = (targetPosition - positions[0]).normalized;
                
                for (int i = 1; i <= length; i++)
                {
                    positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
                }
            }
            else
            {
                for (int iteration = 0; iteration < iterations; iteration++)
                {
                    positions[length] = targetPosition;
                    
                    for (int i = length - 1; i > 0; i--)
                    {
                        var direction = (positions[i] - positions[i + 1]).normalized;
                        positions[i] = positions[i + 1] + direction * bonesLength[i];
                    }
                    for (int i = 1; i <= length; i++)
                    {
                        var direction = (positions[i] - positions[i - 1]).normalized;
                        positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
                    }
                    
                    if ((targetPosition - positions[length]).sqrMagnitude >= delta * delta)
                        break;
                }
            }

            for (int i = 0; i < length; i++)
            {
                bones[i].position = (positions[i] + positions[i + 1]) / 2;
                bones[i].LookAt(positions[i]);
                bones[i].localScale = bonesScales[i];
            }
            
            for (int i = 0; i <= length; i++)
                joints[i].position = positions[i];
        }
    }
}
