using UnityEngine;
using UnityEngine.InputSystem; // Библиотека для работы с новой системой ввода

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float walkingSpeed = 15f; // Поставил 5, так как 20 — это слишком быстро
    public float gravity = 20.0f;

    [Header("Настройки камеры")]
    public Camera playerCamera;
    public float lookSpeed = 0.3f; // Понизил для новой системы ввода
    public float lookXLimit = 85f;

    [HideInInspector]
    public bool canMove = true;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. ВЫХОД ИЗ ИГРЫ ПО ESC
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Выход...");
            Application.Quit();
        }

        if (!canMove) return;

        // 2. ПОВОРОТ КАМЕРЫ (МЫШЬ)
        // Считываем движение мыши из новой системы
        Vector2 lookInput = Mouse.current.delta.ReadValue();

        rotationX += -lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, lookInput.x * lookSpeed, 0);

        // 3. ДВИЖЕНИЕ (WASD)
        // Используем старый добрый метод трансформации векторов для CharacterController
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Проверка нажатия клавиш
        float curSpeedX = walkingSpeed * (Keyboard.current.wKey.isPressed ? 1 : Keyboard.current.sKey.isPressed ? -1 : 0);
        float curSpeedY = walkingSpeed * (Keyboard.current.dKey.isPressed ? 1 : Keyboard.current.aKey.isPressed ? -1 : 0);

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // 4. ГРАВИТАЦИЯ
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = -1f; // Небольшое притяжение к полу
        }

        // ПРИМЕНЕНИЕ ДВИЖЕНИЯ
        characterController.Move(moveDirection * Time.deltaTime);
    }
}