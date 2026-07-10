using UnityEngine;

public class SC_Player : MonoBehaviour
{
    // --- Movement --- //

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CharacterController characterController;

    private float walkSpeed;
    private float sprintSpeed;
    private bool isRunning = false;

    private float gravity;

    private Vector3 moveDirection;

    private float viewSensitivity;
    private float xRotation;

    private bool canMouseRotate = true;
    private bool isCrouched = false;

    private float baseCameraHeight;

    private float originalHeight;
    private float crouchSpeed; // Adjust this value to control the crouch speed
    private bool canStandUp;

    private float rotationSpeed; // Rotation Speed
    private float targetZRotation = 0.0f;
    private float currentZRotation = 0.0f;

    // New variables for jumping
    private float jumpForce;
    private bool isGrounded;

    // Game manager for character data
    private SC_GameManager gameManager;

    // Used for inventory and pausing
    private bool isPaused = false;

    // For stamina fixes
    private Vector3 lastPosition;

    // Basic Character Data
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseSprintSpeedMultiplier;
    [SerializeField] private float baseCrouchSpeedMultiplier;
    [SerializeField] private float baseJumpForce;
    [SerializeField] private float baseGravity;
    [SerializeField] private float baseViewSensitivity;
    [SerializeField] private float baseRotationSpeed;
    [SerializeField] private float baseSprintStaminaDrain;
    [SerializeField] private float baseJumpStaminaDrain;
    [SerializeField] private float baseHungerDrain;

    [SerializeField] private float maxHealth;
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxHunger;
    [SerializeField] private float maxAdrenaline;

    [SerializeField] private float health;
    [SerializeField] private float stamina;
    [SerializeField] private float hunger;
    [SerializeField] private float adrenaline;

    // Picking up items
    [SerializeField] SC_LineOfSight lineOfSight;


    private void Start()
    {
        lastPosition = transform.position; // To track sprinting

        gameManager = FindAnyObjectByType<SC_GameManager>();

        // Find attached camera
        foreach (Transform findObj in this.transform)
        {
            if (findObj.name == "PlayerCamera")
            {
                mainCamera = findObj.GetComponent<Camera>();
            }
        }

        characterController = transform.GetComponent<CharacterController>();

        // Crouching
        originalHeight = characterController.height;
        baseCameraHeight = mainCamera.transform.localPosition.y;

        LockCursor(); // Lock cursor after game start
    }

    private void UpdateCharacterData()
    {
        walkSpeed = (baseSpeed / 2.5f) + 1.0f;

        sprintSpeed = walkSpeed * baseSprintSpeedMultiplier;
        crouchSpeed = walkSpeed * baseCrouchSpeedMultiplier;

        jumpForce = baseJumpForce;
        gravity = baseGravity;

        viewSensitivity = baseViewSensitivity;
        rotationSpeed = baseRotationSpeed;
    }

    private void Update()
    {
        if (!isPaused)
        {
            UpdateCharacterData();

            Move();

            if (canMouseRotate)
            {
                View();
            }

            if (characterController.height == 1.0f)
            {
                CheckObstaclesAbove();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                lineOfSight.UseTargetedItem();
            }
        }

        // Inventory and pause menus
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager.GetGameState() == SC_GameManager.GameState.INVENTORY)
            {
                gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
            }
            else
            {
                if (!isPaused)
                {
                    gameManager.SetGameState(SC_GameManager.GameState.PAUSED);
                }
                else
                {
                    gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (gameManager.GetGameState() == SC_GameManager.GameState.PLAYING)
            {
                gameManager.SetGameState(SC_GameManager.GameState.INVENTORY);
            }
            else if (gameManager.GetGameState() == SC_GameManager.GameState.INVENTORY)
            {
                gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
            }
        }

        switch (gameManager.GetGameState())
        {
            case SC_GameManager.GameState.PAUSED:
            case SC_GameManager.GameState.INVENTORY:
            case SC_GameManager.GameState.LOOTING:
                SetPaused(true);
                break;

            case SC_GameManager.GameState.PLAYING:
                SetPaused(false);
                break;
        }

        if (stamina < maxStamina && !isRunning)
        {
            stamina += baseSprintStaminaDrain / 3.0f * Time.deltaTime;
        }
        hunger -= baseHungerDrain * Time.deltaTime;
    }

    private void Move()
    {
        lastPosition = transform.position;

        if (characterController.isGrounded)
        {
            isGrounded = true; // Player is on the ground
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            if (stamina > 5)
            {
                if (Input.GetKey(KeyCode.LeftShift) && !isCrouched)
                {
                    isRunning = true;
                    moveDirection *= sprintSpeed;
                }
                else
                {
                    isRunning = false;

                    if (isCrouched)
                    {
                        moveDirection *= crouchSpeed;
                    }
                    else
                    {
                        moveDirection *= walkSpeed;
                    }
                }
            }
            else if (stamina < 2)
            {
                isRunning = false;

                if (isCrouched)
                {
                    moveDirection *= crouchSpeed;
                }
                else
                {
                    moveDirection *= walkSpeed;
                }
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                characterController.height = originalHeight - 1;
                mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x,
                                            baseCameraHeight - 1,
                                            mainCamera.transform.localPosition.z);
                characterController.Move(new Vector3(0, -0.5f, 0));

                isCrouched = true;
            }
            else if (canStandUp && isCrouched)
            {
                characterController.Move(new Vector3(0, 0.5f, 0));
                mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x,
                                            baseCameraHeight,
                                            mainCamera.transform.localPosition.z);
                characterController.height = originalHeight; // Set the target standing height

                isCrouched = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                targetZRotation = -0.75f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                targetZRotation = 0.75f;
            }
            else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                targetZRotation = 0.0f;
            }

            currentZRotation = Mathf.Lerp(currentZRotation, targetZRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentZRotation);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (stamina >= baseJumpStaminaDrain)
            {
                moveDirection.y = jumpForce;
                isGrounded = false;
                stamina -= baseJumpStaminaDrain;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        if (isRunning)
        {
            if (Mathf.Abs(lastPosition.x - transform.position.x) < 0.25f && Mathf.Abs(lastPosition.z - transform.position.z) < 0.25f)
            {
                float staminaDrain = baseSprintStaminaDrain * Time.deltaTime;
                stamina -= staminaDrain;
            }
        }
    }

    private void CheckObstaclesAbove()
    {
        float raycastDistance = 1.5f;

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, raycastDistance))
        {
            canStandUp = false;
        }
        else
        {
            canStandUp = true;
        }
    }

    private void View()
    {
        float inputX = Input.GetAxis("Mouse X") * viewSensitivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * viewSensitivity * Time.deltaTime;

        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * inputX);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;

        if (isPaused)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetHunger(float hunger)
    {
        this.hunger = hunger;
    }

    public void SetAdrenaline(float adrenaline)
    {
        this.adrenaline = adrenaline;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetHunger()
    {
        return hunger;
    }

    public float GetAdrenaline()
    {
        return adrenaline;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxHunger()
    {
        return maxHunger;
    }

    public float GetMaxAdrenaline()
    {
        return maxAdrenaline;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }
}
