using JetBrains.Annotations;
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
    // 0 = No
    // 1 = Inventory/In Game Menu
    // 2 = Pause Menu/Dead
    private int isPaused = 0;

    // For stamina fixes
    private Vector3 lastPosition;

    private bool isFrozen = false;
    private float frozenTime = 0.0f;

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

    // Death and Damage
    [SerializeField] private GameObject corpse;

    [SerializeField] private float killY;

    [SerializeField] private float immunityTime;
    private float time;

    [SerializeField] private float hungerHealthDrainPerSecond;

    [SerializeField] private Canvas deathScreen;

    // Pausing
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private Canvas HUD;

    // Start Delay
    [SerializeField] private bool isEnabled = false;

    // Sliding down slopes
    private bool isSliding;
    private Vector3 slopeSlideVelocity = Vector3.zero;

    [SerializeField] float maxSlopeLaunch;


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


    // --- Movement and Data --- //

    private void UpdateCharacterData()
    {
        walkSpeed = (baseSpeed / 2.5f) + 1.0f;

        sprintSpeed = walkSpeed * baseSprintSpeedMultiplier;
        crouchSpeed = walkSpeed * baseCrouchSpeedMultiplier;

        jumpForce = baseJumpForce;
        gravity = baseGravity;

        viewSensitivity = baseViewSensitivity;
        rotationSpeed = baseRotationSpeed;

        if (stamina < maxStamina && !isRunning)
        {
            stamina += baseSprintStaminaDrain / 3.0f * Time.deltaTime;
        }

        if (hunger <= 0.0f)
        {
            hunger = 0.0f;
            TakeDamage(hungerHealthDrainPerSecond * Time.deltaTime);
        }
        else
        {
            hunger -= baseHungerDrain * Time.deltaTime;
        }
    }

    private void Update()
    {
        deathScreen.enabled = (gameManager.GetGameState() == SC_GameManager.GameState.DEAD);
        pauseMenu.enabled = (gameManager.GetGameState() == SC_GameManager.GameState.PAUSED);

        if (!isEnabled)
        {
            return;
        }

        if (time < immunityTime)
        {
            time += Time.deltaTime;
        }

        if (isPaused == 0 || isPaused == 1)
        {
            if (gameObject.transform.position.y <= killY)
            {
                Die(false);
            }

            UpdateCharacterData();

            if (characterController.height == 1.0f)
            {
                CheckObstaclesAbove();
            }

            if (isPaused == 0)
            {
                HUD.enabled = true;

                Move();

                if (canMouseRotate)
                {
                    View();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    lineOfSight.UseTargetedItem();
                }
            }
            else
            {
                HUD.enabled = false;
            }
        }
        else
        {
            HUD.enabled = false;
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
                if (isPaused == 0)
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
            case SC_GameManager.GameState.DEAD:
                SetPaused(2);
                break;

            case SC_GameManager.GameState.INVENTORY:
            case SC_GameManager.GameState.LOOTING:
            case SC_GameManager.GameState.TALKING:
                SetPaused(1);
                break;

            case SC_GameManager.GameState.PLAYING:
                SetPaused(0);
                break;
        }

        if (health <= 0)
        {
            Die(true);
        }
    }

    private void Move()
    {
        isFrozen = (lastPosition == transform.position);

        if (isFrozen)
        {
            frozenTime += Time.deltaTime;
        }
        else
        {
            frozenTime = 0;
        }

        bool isFrozenPractical = (frozenTime >= 0.4f);

        Debug.Log("Character Sliding = " + isSliding + ", Character Frozen = " + isFrozenPractical);

        lastPosition = transform.position;

        if (characterController.isGrounded || isFrozenPractical)
        {
            if (slopeSlideVelocity != Vector3.zero)
            {
                isSliding = true;
            }

            isGrounded = true; // Player is on the ground

            if (!isSliding || isFrozenPractical)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            }
            else
            {
                moveDirection = Vector3.zero;
            }

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

        if (Input.GetKeyDown(KeyCode.Space) && ((isGrounded && !isSliding) || isFrozenPractical))
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

        SetSlopeSlideVelocity();

        if (slopeSlideVelocity == Vector3.zero)
        {
            isSliding = false;
        }

        if (isSliding && moveDirection.magnitude < maxSlopeLaunch)
        {
            moveDirection.x += slopeSlideVelocity.x;
            moveDirection.z += slopeSlideVelocity.z;
        }

        //Debug.Log(moveDirection);

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


    // --- Damage --- //

    public void TakeDamage(float damage)
    {
        if (time >= immunityTime)
        {
            time = 0.0f;
            health -= damage;
        }
    }

    public void Die(bool makeCorpse)
    {
        if (gameManager.GetGameState() != SC_GameManager.GameState.DEAD)
        {
            if (makeCorpse)
            {
                GameObject deadBody = Instantiate(corpse);
                deadBody.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                deadBody.transform.rotation = transform.rotation;

                SC_StorageUnit deadBodyStorage = deadBody.GetComponent<SC_StorageObject>().GetStorageUnit();
                gameManager.GetInventory().TransferAllItemsTo(deadBodyStorage);
            }

            gameManager.SetGameState(SC_GameManager.GameState.DEAD);
        }
    }

    public void FinalDie()
    {
        gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
        Destroy(gameObject);
    }


    // --- Utility --- //

    public void SetPaused(int paused)
    {
        isPaused = paused;

        if (isPaused < 0)
        {
            isPaused = 0;
        }
        else if (isPaused > 2)
        {
            isPaused = 2;
        }

        if (isPaused == 0)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
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


    // --- Enabling --- //

    public void Enable()
    {
        isEnabled = true;
    }


    // --- Sliding --- //

    private void SetSlopeSlideVelocity()
    {
        /*
        for (int x = -1; x <= 1;  x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                float checkScale = 0.5f;
                Vector3 testPos = new Vector3(transform.position.x + (transform.lossyScale.x * checkScale * x), transform.position.y, transform.position.z + (transform.lossyScale.z * checkScale * z));

                if (Physics.Raycast(testPos, Vector3.down, out RaycastHit hitInfo, 3))
                {
                    float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

                    if (angle > characterController.slopeLimit)
                    {
                        slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, moveDirection.y, 0), hitInfo.normal);
                        return;
                    }
                }
            }
        }
        */

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 3))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle > characterController.slopeLimit)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, moveDirection.y, 0), hitInfo.normal);
                return;
            }
        }

        slopeSlideVelocity = Vector3.zero;
    }
}
