using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // ��� ������
    [SerializeField] private float attackRange = 2f;     // ��������� �����
    [SerializeField] private float attackCooldown = 1f;  // �������� ����� �������
    [SerializeField] private int attackDamage = 10;      // ���� �� �����
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip PounchSound;
    //[SerializeField] private float rotationSpeed = 5f;   // �������� �������� � ������

    [SerializeField] private NavMeshAgent agent;                          // ��������� ��� ���������
    private Transform player;                            // ������ �� ������
    [SerializeField] private Animator animator;                           // ��������� ��������
    private float lastAttackTime;                        // ����� ��������� �����

    // ����� ���������� � Animator
    private static readonly int WalkParameter = Animator.StringToHash("Walk");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();            // �������������� NavMeshAgent
        animator = GetComponent<Animator>();             // �������������� Animator
        lastAttackTime = -attackCooldown;                // ����� ����� ����� �������� �����

        // ������� ������ �� ����
        FindPlayer();
    }

    void Update()
    {
        // ���� ����� �� ������, �������� ����� �����
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // ������� ����� � ������
        agent.SetDestination(player.position);

        // ��������� �������� ������
        UpdateWalkAnimation();

        // ��������� ���������� �� ������
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // ������������ ����� � ������
            //FacePlayer();

            // ���� ������ ���������� ������� � ��������� �����
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();                                // ������� ������
                lastAttackTime = Time.time;              // ��������� ����� �����
            }
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("����� ������: " + playerObj.name);
        }
        else
        {
            Debug.LogWarning("����� � ����� '" + playerTag + "' �� ������!");
        }
    }

    private void UpdateWalkAnimation()
    {
        if (animator != null)
        {
            // ������������� �������� Walk � ����������� �� �������� ��������
            bool isWalking = agent.velocity.magnitude > 0.1f;
            animator.SetBool(WalkParameter, isWalking);
        }
    }

    /*private void FacePlayer()
    {
        if (player == null) return;

        // ��������� ����������� � ������
        Vector3 direction = (player.position - transform.position).normalized;
        // ���������� ��� Y, ����� ���� �� ���������� �����/����
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // ������ ������������ �����
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }*/

    private void Attack()
    {
        if (audioSource != null && PounchSound != null)
        {
            audioSource.PlayOneShot(PounchSound);
        }
        // ��������� �������� �����
        if (animator != null)
        {
            animator.SetTrigger(AttackTrigger);
        }
        // ������� ���� ������
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("���� �������� ������!");
        }
    }
}