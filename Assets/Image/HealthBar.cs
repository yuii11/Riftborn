using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    public Transform target; // ����, �� ������� ����� ���������� �������
    public Image healthFill; // ����������� ����� �������
    public EnemyHealth health;

    private void Start()
    {
        // ������� ������ �� ����
        FindPlayer();
    }
    private void LateUpdate()
    {
        // ������������ ������� � ������
        if (target != null)
        {
            transform.LookAt(Camera.main.transform);
        }
        else
        {

        }
    }

    public void Update()
    {
        // ��������� ���������� �������
        if (healthFill != null)
        {
            healthFill.fillAmount = health.currentHealth / health.maxHealth;
        }
        else
        {

        }
    }
    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
            Debug.Log("����� ������ ��� HealthBar: " + playerObj.name);
        }
        else
        {
            Debug.LogWarning("����� � ����� '" + playerTag + "' �� ������ ��� HealthBar!");
        }
    }
}
