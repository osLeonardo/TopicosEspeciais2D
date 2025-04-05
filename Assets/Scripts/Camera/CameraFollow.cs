using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public void LateUpdate()
    {
        if (target is not null)
        {
            transform.position = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                transform.position.z);
        }
    }
}
