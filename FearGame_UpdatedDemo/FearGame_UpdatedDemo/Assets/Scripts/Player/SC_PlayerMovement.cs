using Unity.Android.Gradle.Manifest;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class SC_PlayerMovement : MonoBehaviour
{
    /* 
     * Goal: Create a first person movement system that can handle complex parkour
     *      * Walk, Run, Crouch
     *      * Jump
     *      * Double Jump
     *      * Wall Jump
     *      * Glide
     *      * Rope Swing
     *      * Edge Grab
     *      * Slide Down Slope
     *      
     * Part 1: Basic walk/run/crouch, jump, and slide down basic slopes
     * 
     *      1. Clear old force data
     *      2. Handle look rotation
     *      3. Check ground beneath player: 
     *              Are they on the ground?
     *              Is the ground sloped? What angle?
     *      4. Update variables accordingly
     *      5. Apply sliding forces
     *      6. Handle player input
    */

    public enum PlayerMovementSetting
    {
        WALK,
        RUN,
        CROUCH,
        IN_MENU
    }

    [SerializeField] private PlayerMovementSetting movementSetting = PlayerMovementSetting.WALK;

    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float crouchSpeed = 1.0f;

    [SerializeField] private float baseHeight = 1.0f;
    [SerializeField] private float crouchHieght = 0.5f;

    [SerializeField] private float baseJumpForce = 5.0f;
    [Tooltip("How much the player's ability to jump should be limited when standing on an 89 degree slope.\nWill range from 1.0-value based on exact angle.")]
    [Range(0.0f, 1.0f)][SerializeField] private float slopeJumpDampening = 1.0f;

    private float groundDistance = 0.0f;
    private Vector3 groundNormal = Vector3.zero;
    private float groundAngle = 0.0f;

    private float roofDistance = 0.0f;
    private Vector3 roofNormal = Vector3.zero;
    private float roofAngle = 0.0f;

    private const float MIN_SLOPE_ANGLE = 0.0f;
    private const float MAX_SLOPE_ANGLE = 90.0f;

    [Range(0.0f, 1.0f)][SerializeField] private float slideForceStrength = .05f;

    [SerializeField] private float standardRaycastDistance = 3.0f;

    private Vector3 forceAccum = Vector3.zero;

    private Vector3 velocity = Vector3.zero;
    [Range(0.0f, 1.0f)][SerializeField] private float slopeDampening = 0.95f;
    [Range(0.0f, 1.0f)][SerializeField] private float flatGroundDampening = 0.25f;

    private Vector3 kinematicMotion = Vector3.zero;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerBody;
    private CharacterController characterController;
    private Rigidbody rigidbody;

    // --------------------
    // Credit: Karl Ramstedt on Github - https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e
    // --------------------
    [Range(0.1f, 9f)][SerializeField] private float lookSensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] private float lookYRotationLimit = 88f;

    private Vector2 lookRotation = Vector2.zero;
    // --------------------

    [SerializeField] private Vector3 gravity = new Vector3(0, -9.8f, 0);

    [SerializeField] private float terminalVelocity = 30;
    [SerializeField] private float acceptableGroundDistance = 0.1f;

    // Game manager for character data
    private SC_GameManager gameManager;

    [SerializeField] private float baseSprintStaminaDrain;
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

    bool isJumping = false;
    float jumpingTimer = 0.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();

        gameManager = FindAnyObjectByType<SC_GameManager>();
    }

    // --- Movement and Data --- //

    private void UpdateCharacterData()
    {
        if (movementSetting == PlayerMovementSetting.RUN)
        {
            stamina -= baseSprintStaminaDrain * Time.deltaTime;
        }
        else if (stamina < maxStamina)
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

        if(Vector3.Dot(velocity, gravity) < 0)
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
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

        if (movementSetting == PlayerMovementSetting.IN_MENU)
        {
            SetCursorLocked(false);
            HUD.enabled = false;
        }
        else
        {
            SetCursorLocked(true);
            HUD.enabled = true;

            UpdateCharacterData();

            if (gameObject.transform.position.y <= killY)
            {
                Die(false);
            }

            // Reset forces to not mess up acceleration
            ClearForces();

            // Get data about the world to handle sliding/falling/etc.
            GetGroundData(standardRaycastDistance);
            GetRoofData(standardRaycastDistance);

            // Look
            SetLookRotation();

            // Forces
            if (groundAngle > characterController.slopeLimit) { AddSlideForce(); }
            else { AddGravityForce(); } // Doing my own gravity so I have more control over direction and sliding
            ApplyPlayerInput();

            // Apply forces to velocity
            UpdateVelocity();

            // Move
            characterController.Move((velocity + kinematicMotion) * Time.deltaTime); // Velocity is in m/s, so account for framerate
        }

        // Inventory and pause menus
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC");

            if (gameManager.GetGameState() == SC_GameManager.GameState.INVENTORY)
            {
                gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
            }
            else
            {
                if (movementSetting != PlayerMovementSetting.IN_MENU)
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

        if (gameManager.GetGameState() != SC_GameManager.GameState.PLAYING)
        { 
            movementSetting = PlayerMovementSetting.IN_MENU;
        }
    }

    // --------------------
    // Credit: Karl Ramstedt on Github - https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e
    // --------------------
    private void SetLookRotation()
    {
        lookRotation.x += Input.GetAxis("Mouse X") * lookSensitivity;
        lookRotation.y += Input.GetAxis("Mouse Y") * lookSensitivity;
        lookRotation.y = Mathf.Clamp(lookRotation.y, -lookYRotationLimit, lookYRotationLimit);

        var xQuat = Quaternion.AngleAxis(lookRotation.x, gravity.normalized * -1);
        var yQuat = Quaternion.AngleAxis(lookRotation.y, Vector3.left);

        transform.localRotation = xQuat;
        playerCamera.transform.localRotation = yQuat;
    }
    // --------------------

    // Update contained variables about distance to and angle of ground
    private void GetGroundData(float raycastDistance)
    {
        if (Physics.Raycast(transform.position, gravity.normalized, out RaycastHit hitInfo, raycastDistance))
        {
            groundDistance = hitInfo.distance;
            groundNormal = hitInfo.normal;
            groundAngle = Vector3.Angle(hitInfo.normal, gravity.normalized * -1);
            return;
        }

        groundDistance = raycastDistance + 1.0f;
        groundNormal = gravity.normalized * -1;
        groundAngle = 0;
    }

    // Update contained variables about distance to and angle of roof
    private void GetRoofData(float raycastDistance)
    {
        Vector3 upDirection = gravity.normalized * -1;
        if (Physics.Raycast(transform.position + upDirection, upDirection, out RaycastHit hitInfo, raycastDistance))
        {
            roofDistance = hitInfo.distance;
            roofNormal = hitInfo.normal;
            roofAngle = Vector3.Angle(hitInfo.normal, gravity.normalized);
            return;
        }

        roofDistance = raycastDistance + 1.0f;
        roofNormal = gravity.normalized;
        roofAngle = 0;
    }

    // Calculate the distance to the ground based on angle
    private float GetMaxFallDistance()
    {
        Vector3 groundVector = groundNormal;

        groundVector = Vector3.Normalize(groundVector) * (transform.lossyScale.x / 2);

        return groundDistance - Mathf.Abs(groundVector.y);
    }

    // Lock/Unlock cursor for UI vs game
    private void SetCursorLocked(bool isLocked)
    {
        if (isLocked)
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

    // Remove all forces acting on the object
    private void ClearForces()
    {
        forceAccum = Vector3.zero;
    }

    // Add a force to be acting on the object
    public void AddForce(Vector3 force)
    {
        forceAccum += force;
    }

    // If on a sloped surface, slide down in
    private void AddSlideForce()
    {
        Vector3 groundVector = Vector3.Cross(groundNormal, Vector3.Cross(groundNormal, gravity * -1));

        Vector3 slideForce = Vector3.Normalize(groundVector) * slideForceStrength * groundAngle;

        if (Physics.Raycast(transform.position + slideForce.normalized, slideForce.normalized, out RaycastHit hitInfo, slideForce.magnitude))
        {
            slideForce = slideForce.normalized * hitInfo.distance;
        }

        if (slideForce.magnitude > gravity.magnitude) { slideForce = slideForce.normalized * gravity.magnitude; }

        AddForce(slideForce);
    }

    // Apply gravity based on direction and mass
    private void AddGravityForce()
    {
        AddForce(gravity * rigidbody.mass);
    }

    // Update velocity based on forces
    private void UpdateVelocity()
    {
        Vector3 acceleration = forceAccum / rigidbody.mass;

        Debug.Log(forceAccum);
        Debug.Log(acceleration);

        velocity += acceleration * Time.deltaTime;

        if (groundAngle > characterController.slopeLimit)
        {
            velocity *= Mathf.Pow(slopeDampening, Time.deltaTime);
        }
        else
        {
            velocity *= Mathf.Pow(flatGroundDampening, Time.deltaTime);
        }

        // Don't fall through the ground
        float maxFall = GetMaxFallDistance();
        if (velocity.y * Time.deltaTime > maxFall)
        {
            velocity.y = maxFall;
        }

        if (velocity.magnitude > Mathf.Abs(terminalVelocity))
        {
            velocity = velocity.normalized * terminalVelocity;
        }
    }

    // Is there enough space above the player's head to stand up?
    private bool CanStandUp()
    {
        Debug.Log(roofDistance + "> (" + baseHeight + "-" + crouchHieght + ")");
        return roofDistance > (baseHeight - crouchHieght);
    }

    private bool CanJump()
    {
        return (groundDistance <= (acceptableGroundDistance + transform.lossyScale.y)) && !isJumping && Vector3.Dot(velocity, gravity) > 0.5;
    }

    private void ApplyPlayerInput()
    {
        kinematicMotion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (!(movementSetting == PlayerMovementSetting.CROUCH) || CanStandUp())
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && stamina > 5 && kinematicMotion != Vector3.zero)
            {
                movementSetting = PlayerMovementSetting.RUN;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                movementSetting = PlayerMovementSetting.CROUCH;
            }
            else
            {
                movementSetting = PlayerMovementSetting.WALK;
            }
        }

        if (movementSetting == PlayerMovementSetting.CROUCH)
        {
            playerCamera.transform.localPosition = new Vector3(0, crouchHieght, 0);
            playerBody.transform.localScale = new Vector3(1, crouchHieght, 1);
            playerBody.transform.localPosition = new Vector3(0, 0 - (baseHeight - crouchHieght), 0);
        }
        else
        {
            playerCamera.transform.localPosition = new Vector3(0, baseHeight, 0);
            playerBody.transform.localScale = new Vector3(1, baseHeight, 1);
            playerBody.transform.localPosition = Vector3.zero;
        }

        switch (movementSetting)
        {
            case PlayerMovementSetting.WALK:
                kinematicMotion *= walkSpeed;
                break;

            case PlayerMovementSetting.RUN:
                kinematicMotion *= runSpeed;
                break;

            case PlayerMovementSetting.CROUCH:
                kinematicMotion *= crouchSpeed;
                break;
        }

        kinematicMotion = transform.TransformDirection(kinematicMotion);

        if (Input.GetKey(KeyCode.Space) && CanJump())
        {
            Debug.Log("Jump");
            jumpingTimer = 0.35f;
            isJumping = true;
        }

        if (isJumping && jumpingTimer > 0.0f)
        {
            jumpingTimer -= Time.deltaTime;
            AddForce(gravity.normalized * -1 * baseJumpForce);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            lineOfSight.UseTargetedItem();
        }
    }


    // --- Damage --- //

    public void TakeDamage(float damage)
    {
        if (time >= immunityTime)
        {
            time = 0.0f;
            health -= damage;
        }

        if (health < 0.0f)
        {
            Die(true);
        }
    }

    public void Die(bool makeCorpse)
    {
        if (gameManager.GetGameState() != SC_GameManager.GameState.DEAD)
        {
            if (makeCorpse)
            {
                GameObject deadBody = Instantiate(corpse);
                deadBody.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
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
}
