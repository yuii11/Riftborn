using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 1f; // ���������������� ����
    [SerializeField] private Transform playerBody;        // ���� ������
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // ��������� ���������� ����������������
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 100f;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 100f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}