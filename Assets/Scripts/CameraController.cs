using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; 
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 2, -10); 

    [Header("Ajuste de Salto")]
    public float jumpOffsetMultiplier = 2f; 
    public float thresholdY = 2f; 

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        float diffY = target.position.y - transform.position.y;

        if (Mathf.Abs(diffY) > thresholdY)
        {
            desiredPosition.y += diffY * jumpOffsetMultiplier;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, offset.z);
    }
}