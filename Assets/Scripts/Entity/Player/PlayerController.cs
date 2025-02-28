using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    const float GRAVITY = -9.6f;
    const float MOVE_FRICTION = 9.0f;
    public float moveSpeed = 5.0f;
    public float lookSensitivity = 1.0f;

    public PlayerHUD playerHUD;
    public FixedJoystick moveJoystick;
    public RectTransform lookPanel;

    private CharacterController controller;
    private Camera mainCamera;

    private Vector3 m_velocity;
    private Vector3 m_moveDir;
    private int currentLookTouchID = -1;


    #region INITIALIZATION

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        ProduceAssertions();
    }

    private void ProduceAssertions(){
        Assert.IsNotNull(moveJoystick, "Missing move joystick");
        Assert.IsNotNull(lookPanel, "Missing Look Panel");
        Assert.IsNotNull(mainCamera, "Missing Main Camera");
    }

    #endregion


    #region BEHAVIOR

    private void Update()
    {
        ProduceMoving();
        ProduceRotation();
    }

    private void ProduceMoving()
    {
        Vector2 inputDir = GetMoveInput();
        m_moveDir = transform.right * inputDir.x + transform.forward * inputDir.y;
        Vector3 targetVelocity = m_moveDir * moveSpeed * Time.deltaTime;

        m_velocity = Vector3.Lerp(
            m_velocity,
            new Vector3(targetVelocity.x, m_velocity.y, targetVelocity.z),
            MOVE_FRICTION * Time.deltaTime
        );

        if(!controller.isGrounded) m_velocity.y += GRAVITY * Time.deltaTime;

        controller.Move(m_velocity);
    }

    private void ProduceRotation(){
        if(Input.touchCount == 0) return;

        for(int i = 0; i < Input.touchCount; i++) 
        {
            Touch touch = Input.GetTouch(i);
            
            if(touch.phase == TouchPhase.Began)
            {
                playerHUD.ProduceTouch(touch.position);

                if(currentLookTouchID == -1)
                {
                    if(!IsPosInLookRect(touch.position)) continue;
                    currentLookTouchID = touch.fingerId;
                }
            }
            
            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if(touch.fingerId == currentLookTouchID)
                {
                    currentLookTouchID = -1;
                    continue;
                }
            }

            // PRODUCE ROTATION
            if(touch.phase == TouchPhase.Moved && touch.fingerId == currentLookTouchID)
            {
                playerHUD.AddHudRotationOffset(touch.deltaPosition / 250.0f);

                transform.rotation *= Quaternion.Euler(0.0f, touch.deltaPosition.x * lookSensitivity * Time.deltaTime, 0.0f);

                float cameraXRelative = -touch.deltaPosition.y * lookSensitivity * Time.deltaTime;
                float cameraXAngle = mainCamera.transform.localEulerAngles.x;
                cameraXAngle = (cameraXAngle > 180) ? cameraXAngle - 360 : cameraXAngle;

                mainCamera.transform.localRotation = Quaternion.Euler(
                    Mathf.Clamp(cameraXAngle + cameraXRelative, -90.0f, 90.0f), 
                    0.0f, 
                    0.0f
                );
            }
        }
    }

    #endregion


    #region PRIVATE ACTIONS

    private Vector2 GetMoveInput()
    {
        return new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
    }

    private bool IsPosInLookRect(Vector2 pos){
        return RectTransformUtility.RectangleContainsScreenPoint(lookPanel, pos);
    }

    #endregion
}
