using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera yourCamera;
    public float parallaxValue;

    private Vector2 length;
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
        length = GetComponentInChildren<SpriteRenderer>().bounds.size;
    }

    void Update()
    {
        Vector3 relativePos = yourCamera.transform.position * parallaxValue;
        Vector3 dist = yourCamera.transform.position - relativePos;
    
        {
            startPosition.x += length.x;
        }
        if (dist.x < startPosition.x - length.x)
        {
            startPosition.x -= length.x;
        }
    
        relativePos.z = startPosition.z;
        transform.position = startPosition + relativePos;
    }
}
