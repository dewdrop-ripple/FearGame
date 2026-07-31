using UnityEngine;

public class SC_Corpse : MonoBehaviour
{
    private Vector3 oldPos;
    private Quaternion oldRot;

    [SerializeField] float minSpeed;

    private void Start()
    {
        oldPos = transform.position;
        oldRot = transform.rotation;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, oldPos) < minSpeed)
        {
            transform.position = oldPos;
        }

        if (Quaternion.Angle(transform.rotation, oldRot) < minSpeed)
        {
            transform.rotation = oldRot;
        }

        oldPos = transform.position;
        oldRot = transform.rotation;
    }
}
