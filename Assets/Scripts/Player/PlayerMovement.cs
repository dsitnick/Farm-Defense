using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraRoot;
    public float sensitivity = 2;
    
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 10f;
    public float gravity = -9.81f;
    
    [Header("Ground Check")]
    public float groundCheckRadius = 0.5f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayerMask = 1;
    
    private Rigidbody rb;
    private bool isGrounded;
    private bool mouseLookEnabled = true;
    private Vector3 velocity;
    Vector2 mouseInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        SetMouseLook(true);
    }

    void Update()
    {
        CursorToggling();
        MouseLook();
        JumpInput();
    }

    void FixedUpdate()
    {
        Movement();
        GroundCheck();
        
        // Set rigidbody velocity at the very end after all calculations
        rb.linearVelocity = velocity;
    }

    private void CursorToggling()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetMouseLook(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMouseLook(false);
        }
    }

    private void MouseLook()
    {
        if (!mouseLookEnabled) return;

        mouseInput.x += Input.GetAxis("Mouse X") * sensitivity;
        mouseInput.y -= Input.GetAxis("Mouse Y") * sensitivity;

        mouseInput.y = Mathf.Clamp(mouseInput.y, -90, 90);

        transform.eulerAngles = Vector3.up * mouseInput.x;
        cameraRoot.localEulerAngles = Vector3.right * mouseInput.y;
    }

    private void SetMouseLook(bool enabled)
    {
        mouseLookEnabled = enabled;
        
        if (enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            velocity.y = jumpForce;
        }
    }

    private void GroundCheck()
    {
        if (velocity.y > 0) return;

        Vector3 origin = transform.position;
        isGrounded = false;
        float highestGroundY = float.MinValue;
        
        // Cast multiple rays in a circle around the player
        int rayCount = 8;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = (360f / rayCount) * i;
            Vector3 rayOrigin = origin + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), groundCheckDistance, Mathf.Sin(angle * Mathf.Deg2Rad));
            
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance * 2, groundLayerMask, QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                if (hit.point.y > highestGroundY)
                {
                    highestGroundY = hit.point.y;
                }
            }
        }
        
        // Additional center ray for more precise ground detection
        Vector3 centerRayOrigin = origin + Vector3.up * groundCheckDistance;
        RaycastHit centerHit;
        if (Physics.Raycast(centerRayOrigin, Vector3.down, out centerHit, groundCheckDistance * 2, groundLayerMask, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            if (centerHit.point.y > highestGroundY)
            {
                highestGroundY = centerHit.point.y;
            }
        }
        
        // Set player height to the highest grounded point found
        if (isGrounded && highestGroundY > float.MinValue)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = highestGroundY;
            transform.position = newPosition;
        }
    }

    private void Movement()
    {
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // Calculate movement direction
        Vector3 moveDirection = (transform.forward * keyboardInput.y + transform.right * keyboardInput.x).normalized;
        
        // Check for sprint input
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : movementSpeed;
        
        // Apply horizontal movement
        Vector3 horizontalVelocity = moveDirection * currentSpeed;
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;
        
        // Apply gravity
        if (isGrounded)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }
    }
}
