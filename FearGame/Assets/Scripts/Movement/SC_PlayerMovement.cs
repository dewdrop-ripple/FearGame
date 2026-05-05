using System.Collections;
using UnityEngine;

public class SC_PlayerMovement : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private Camera mMainCamera;
    [SerializeField] private CharacterController mCharacterController;

    private float mWalkSpeed;
    private float mSprintSpeed;
    private bool mIsRunning = false;
    private float mGravity;
    private Vector3 mMoveDirection;
    private float mActualHorizontalSpeed;
    private float mActualSpeed;
    private Vector3 mPreviousPosition;
    private float mViewSensitivity;
    private float mXRotation;
    private bool mCanMouseRotate = true;
    private bool mCanMove = true;
    private bool mCanSprint = true;

    private float mOriginalHeight;
    private float mTargetHeight;
    private float mCrouchSpeed; // Adjust this value to control the crouch speed
    private bool mCanStandUp;

    private float mRotationSpeed; // Rotation Speed
    private float mTargetZRotation = 0.0f;
    private float mCurrentZRotation = 0.0f;

    // New variables for jumping
    private float mJumpForce;
    private bool mIsGrounded;

    private float mTimer = 0.0f;
    [SerializeField] private float mBobbingSpeed = 0.24f;
    [SerializeField] private float mBobbingAmount = 0.06f;
    private float mBobbingAmountMultiplier = 0.5f;
    private float mMidpoint = 0.7f;

    // Game manager for character data
    private SC_GameManager mGameManager;
    private SC_CharacterDataManager mCharacterManager;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
        mCharacterManager = FindAnyObjectByType<SC_CharacterDataManager>();
        UpdateCharacterData();

        // Find attached camera
        foreach (Transform findObj in this.transform)
        {
            if (findObj.name == "PlayerCamera")
            {
                mMainCamera = findObj.GetComponent<Camera>();
            }
        }

        mCharacterController = transform.GetComponent<CharacterController>();
        Cursor.visible = false;
        LockCursor(); // Lock cursor after game start
    }

    private void UpdateCharacterData()
    {
        SC_CharacterData characterData = mCharacterManager.GetCharacterData(mGameManager.GetCurrentCharacter());

        mWalkSpeed = characterData.GetSpeed();
        mSprintSpeed = mWalkSpeed * mCharacterManager.GetSprintSpeedMultiplier();
        mCrouchSpeed = mWalkSpeed * mCharacterManager.GetCrouchSpeedMultiplier();

        mJumpForce = mCharacterManager.GetBaseJumpForce();
        mGravity = mCharacterManager.GetBaseGravity();

        mViewSensitivity = mCharacterManager.GetBaseViewSensitivity();
        mRotationSpeed = mCharacterManager.GetBaseRotationSpeed();
    }

    private void Update()
    {
        if (mCanMove)
        {
            Move();
        }

        if (mCanMouseRotate && !Input.GetKey(KeyCode.R))
        {
            View();
        }

        if (mCharacterController.height == 1.0f)
        {
            CheckObstaclesAbove();
        }
    }

    private void Move()
    {
        RaycastHit hit;
        mActualHorizontalSpeed = ((new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(mPreviousPosition.x, 0f, mPreviousPosition.z)).magnitude) / Time.deltaTime;
        mActualSpeed = ((transform.position - mPreviousPosition).magnitude) / Time.deltaTime;
        mPreviousPosition = transform.position;

        if (mCharacterController.isGrounded)
        {
            mIsGrounded = true; // Player is on the ground
            mMoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            mMoveDirection = transform.TransformDirection(mMoveDirection);

            if (mActualHorizontalSpeed > 0.5f)
            {
                ViewHeadBobbing();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (mCharacterController.height == mOriginalHeight)
                {
                    mTargetHeight = mOriginalHeight - 1;
                }
                else
                {
                    if (mCanStandUp)
                    {
                        mTargetHeight = mOriginalHeight; // Set the target standing height
                    }
                }

                StartCoroutine(ChangeHeightSmoothly());
            }

            if (Input.GetKey(KeyCode.LeftShift) && mCanSprint)
            {
                mIsRunning = true;
                mMoveDirection *= mSprintSpeed;
            }
            else
            {
                mIsRunning = false;
                mMoveDirection *= mWalkSpeed;
            }

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && mIsGrounded)
            {
                mMoveDirection.y = mJumpForce;
                mIsGrounded = false; // Prevent multiple jumps
            }

            if (Input.GetKey(KeyCode.D))
            {
                mTargetZRotation = -1.5f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                mTargetZRotation = 1.5f;
            }
            else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                mTargetZRotation = 0.0f;
            }

            mCurrentZRotation = Mathf.Lerp(mCurrentZRotation, mTargetZRotation, Time.deltaTime * mRotationSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, mCurrentZRotation);
        }

        // Apply gravity
        mMoveDirection.y -= mGravity * Time.deltaTime;
        mCharacterController.Move(mMoveDirection * Time.deltaTime);
    }

    private void CheckObstaclesAbove()
    {
        float raycastDistance = 1.5f;

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, raycastDistance))
        {
            mCanStandUp = false;
        }
        else
        {
            mCanStandUp = true;
        }
    }

    private IEnumerator ChangeHeightSmoothly()
    {
        float elapsedTime = 0f;
        float startHeight = mCharacterController.height;

        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * mCrouchSpeed;
            mCharacterController.height = Mathf.Lerp(startHeight, mTargetHeight, elapsedTime);
            yield return null;
        }

        mCharacterController.height = mTargetHeight;
    }

    private void View()
    {
        float inputX = Input.GetAxis("Mouse X") * mViewSensitivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * mViewSensitivity * Time.deltaTime;

        mXRotation -= inputY;
        mXRotation = Mathf.Clamp(mXRotation, -90f, 90f);

        mMainCamera.transform.localRotation = Quaternion.Euler(mXRotation, 0f, 0f);
        transform.Rotate(Vector3.up * inputX);
    }

    private void ViewHeadBobbing()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            mTimer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(mTimer);
            mTimer += mBobbingSpeed * (Time.deltaTime * 60f);

            if (mTimer > Mathf.PI * 2)
            {
                mTimer -= Mathf.PI * 2;
            }
        }

        Vector3 v3T = mMainCamera.transform.localPosition;

        if (waveslice != 0)
        {
            float translateChange = waveslice * (mBobbingAmount * (mBobbingAmountMultiplier * 0.1f));
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            v3T.y = mMidpoint + translateChange;
        }
        else
        {
            v3T.y = mMidpoint;
        }

        mMainCamera.transform.localPosition = v3T;
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
}
