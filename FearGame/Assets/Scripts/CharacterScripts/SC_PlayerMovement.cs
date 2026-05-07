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
    private bool mIsCrouched = false;
    private float mBaseCameraHeight;
    private float mTargetCameraHeight;

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

    // Game manager for character data
    private SC_CharacterDataManager mCharacterManager;
    private SC_PlayerData mPlayerData;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mCharacterManager = FindAnyObjectByType<SC_CharacterDataManager>();
        mPlayerData = GetComponent<SC_PlayerData>();

        // Find attached camera
        foreach (Transform findObj in this.transform)
        {
            if (findObj.name == "PlayerCamera")
            {
                mMainCamera = findObj.GetComponent<Camera>();
            }
        }

        mCharacterController = transform.GetComponent<CharacterController>();
        mOriginalHeight = mCharacterController.height;
        mTargetHeight = mOriginalHeight;
        mBaseCameraHeight = mMainCamera.transform.position.y;
        mTargetCameraHeight = mBaseCameraHeight;
        Cursor.visible = false;
        LockCursor(); // Lock cursor after game start
    }

    private void UpdateCharacterData()
    {
        mWalkSpeed = mPlayerData.GetSpeed();

        mSprintSpeed = mWalkSpeed * mCharacterManager.GetSprintSpeedMultiplier();
        mCrouchSpeed = mWalkSpeed * mCharacterManager.GetCrouchSpeedMultiplier();

        mJumpForce = mCharacterManager.GetBaseJumpForce();
        mGravity = mCharacterManager.GetBaseGravity();

        mViewSensitivity = mCharacterManager.GetBaseViewSensitivity();
        mRotationSpeed = mCharacterManager.GetBaseRotationSpeed();
    }

    private void Update()
    {
        UpdateCharacterData();

        Move();

        if (mCanMouseRotate)
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
        mActualHorizontalSpeed = ((new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(mPreviousPosition.x, 0f, mPreviousPosition.z)).magnitude) / Time.deltaTime;
        mActualSpeed = ((transform.position - mPreviousPosition).magnitude) / Time.deltaTime;
        mPreviousPosition = transform.position;

        if (mCharacterController.isGrounded)
        {
            mIsGrounded = true; // Player is on the ground
            mMoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            mMoveDirection = transform.TransformDirection(mMoveDirection);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                mTargetHeight = mOriginalHeight - 1;
                mTargetCameraHeight = mBaseCameraHeight - 1;
                mIsCrouched = true;
            }
            else
            {
                mTargetHeight = mOriginalHeight; // Set the target standing height
                mTargetCameraHeight = mBaseCameraHeight;
                mIsCrouched = false;
            }

            if (mCharacterController.height != mTargetHeight)
            {
                mCharacterController.height = mTargetHeight;
                mMainCamera.transform.position = new Vector3(mMainCamera.transform.position.x,
                                            mTargetCameraHeight,
                                            mMainCamera.transform.position.z);
            }

            if (Input.GetKey(KeyCode.LeftShift) && mCanSprint && !mIsCrouched)
            {
                mIsRunning = true;
                mMoveDirection *= mSprintSpeed;
            }
            else
            {
                mIsRunning = false;

                if (mIsCrouched)
                {
                    mMoveDirection *= mCrouchSpeed;
                }
                else
                {
                    mMoveDirection *= mWalkSpeed;
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                mTargetZRotation = -0.75f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                mTargetZRotation = 0.75f;
            }
            else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                mTargetZRotation = 0.0f;
            }

            mCurrentZRotation = Mathf.Lerp(mCurrentZRotation, mTargetZRotation, Time.deltaTime * mRotationSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, mCurrentZRotation);
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && mIsGrounded)
        {
            mMoveDirection.y = mJumpForce;
            mIsGrounded = false;
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

    private void View()
    {
        float inputX = Input.GetAxis("Mouse X") * mViewSensitivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * mViewSensitivity * Time.deltaTime;

        mXRotation -= inputY;
        mXRotation = Mathf.Clamp(mXRotation, -90f, 90f);

        mMainCamera.transform.localRotation = Quaternion.Euler(mXRotation, 0f, 0f);
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
}
