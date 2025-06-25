using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller; // Ссылка на CharacterController
    [SerializeField] private Camera playerCamera;            // Ссылка на камеру
    [SerializeField] private float walkSpeed = 12f;          // Скорость ходьбы
    [SerializeField] private float runSpeed = 18f;           // Скорость бега
    [SerializeField] private float gravity = -9.81f;         // Сила гравитации
    [SerializeField] private float jumpHeight = 3f;          // Высота прыжка
    [SerializeField] private float normalFOV = 60f;          // Нормальное поле зрения
    [SerializeField] private float runFOV = 70f;             // Поле зрения при беге
    [SerializeField] private float fovChangeSpeed = 5f;      // Скорость изменения FOV

    private Vector3 velocity; // Вектор скорости для гравитации и прыжка
    private bool isGrounded;  // Флаг, находится ли персонаж на земле

    void Update()
    {
        // Проверяем, касается ли персонаж земли
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Устанавливаем небольшую отрицательную скорость, чтобы персонаж оставался на земле
        }

        // Получаем ввод от игрока (WASD или стрелки)
        float x = Input.GetAxis("Horizontal"); // Влево/вправо
        float z = Input.GetAxis("Vertical");   // Вперед/назад

        // Вычисляем направление движения относительно ориентации персонажа
        Vector3 move = transform.right * x + transform.forward * z;

        // Определяем текущую скорость (ходьба или бег)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Перемещаем персонажа
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Обрабатываем прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Формула для высоты прыжка
        }

        // Применяем гравитацию
        velocity.y += gravity * Time.deltaTime;

        // Перемещаем персонажа с учетом вертикальной скорости (гравитация + прыжок)
        controller.Move(velocity * Time.deltaTime);

        // Изменяем FOV камеры в зависимости от состояния бега
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? runFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
    }
}
