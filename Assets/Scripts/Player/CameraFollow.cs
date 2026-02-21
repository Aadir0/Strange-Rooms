using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    void Update()
    {
        Vector3 desiredPosition = transform.position;
        desiredPosition.x = Mathf.Clamp(playerTransform.position.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(playerTransform.position.y, minY, maxY);
        transform.position = desiredPosition;
    }
}
