using UnityEngine;

public class LanternMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 rightHandOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float shakeAmplitude = 0.1f; // How far the lantern shakes
    [SerializeField] private float shakeFrequency = 5f;    // How fast the lantern shakes (match with walk animation)
    [SerializeField] private Vector3 rightMoveOffset;      // Additional offset when moving right
    private PlayerMovement playerMovement;
    private Vector3 leftHandOffset;
    private float shakeTimer;

    void Start()
    {
        // Get player movement script
        if (playerTransform == null)
        {
            playerTransform = transform.parent;
        }
        
        if (playerTransform != null)
        {
            playerMovement = playerTransform.GetComponent<PlayerMovement>();
        }

        // Calculate left hand offset (mirror of right hand)
        leftHandOffset = new Vector3(-rightHandOffset.x, rightHandOffset.y, rightHandOffset.z);
    }

    void Update()
    {
        if (playerMovement != null)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            // Determine which hand should hold the lantern based on facing direction
            Vector3 targetOffset;
            Vector3 targetRotation;
            
            // When moving or stationary, position based on facing direction
            if (playerMovement.IsFacingRight())
            {
                targetOffset = rightHandOffset;
                targetRotation = rotationOffset;
            }
            else
            {
                targetOffset = leftHandOffset;
                targetRotation = new Vector3(-rotationOffset.x, -rotationOffset.y, -rotationOffset.z);
            }

            // Apply shake effect only when moving horizontally
            Vector3 shakeOffset = Vector3.zero;
            if (inputX != 0 && inputY == 0) // Only shake when moving horizontally
            {
                shakeTimer += Time.deltaTime * shakeFrequency;
                float shakeX = Mathf.Sin(shakeTimer) * shakeAmplitude;
                shakeOffset = new Vector3(shakeX, 0, 0);
            }
            else
            {
                shakeTimer = 0f;
            }

            // When moving right, use only shake and move offset (no target offset)
            // When moving left or standing, use target offset + shake
            if (inputX > 0 && inputY == 0)
            {
                transform.localPosition = shakeOffset + rightMoveOffset;
            }
            else
            {
                transform.localPosition = targetOffset + shakeOffset;
            }
            
            transform.localRotation = Quaternion.Euler(targetRotation);
        }
    }
}
