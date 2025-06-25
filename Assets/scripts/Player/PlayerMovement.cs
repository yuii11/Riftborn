using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller; // ������ �� CharacterController
    [SerializeField] private Camera playerCamera;            // ������ �� ������
    [SerializeField] private float walkSpeed = 12f;          // �������� ������
    [SerializeField] private float runSpeed = 18f;           // �������� ����
    [SerializeField] private float gravity = -9.81f;         // ���� ����������
    [SerializeField] private float jumpHeight = 3f;          // ������ ������
    [SerializeField] private float normalFOV = 60f;          // ���������� ���� ������
    [SerializeField] private float runFOV = 70f;             // ���� ������ ��� ����
    [SerializeField] private float fovChangeSpeed = 5f;      // �������� ��������� FOV

    private Vector3 velocity; // ������ �������� ��� ���������� � ������
    private bool isGrounded;  // ����, ��������� �� �������� �� �����

    void Update()
    {
        // ���������, �������� �� �������� �����
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ������������� ��������� ������������� ��������, ����� �������� ��������� �� �����
        }

        // �������� ���� �� ������ (WASD ��� �������)
        float x = Input.GetAxis("Horizontal"); // �����/������
        float z = Input.GetAxis("Vertical");   // ������/�����

        // ��������� ����������� �������� ������������ ���������� ���������
        Vector3 move = transform.right * x + transform.forward * z;

        // ���������� ������� �������� (������ ��� ���)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // ���������� ���������
        controller.Move(move * currentSpeed * Time.deltaTime);

        // ������������ ������
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // ������� ��� ������ ������
        }

        // ��������� ����������
        velocity.y += gravity * Time.deltaTime;

        // ���������� ��������� � ������ ������������ �������� (���������� + ������)
        controller.Move(velocity * Time.deltaTime);

        // �������� FOV ������ � ����������� �� ��������� ����
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? runFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
    }
}
