using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // Тег игрока
    [SerializeField] private float attackRange = 2f;     // Дальность атаки
    [SerializeField] private float attackCooldown = 1f;  // Задержка между атаками
    [SerializeField] private int attackDamage = 10;      // Урон от атаки
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip PounchSound;
    //[SerializeField] private float rotationSpeed = 5f;   // Скорость поворота к игроку

    [SerializeField] private NavMeshAgent agent;                          // Компонент для навигации
    private Transform player;                            // Ссылка на игрока
    [SerializeField] private Animator animator;                           // Компонент анимации
    private float lastAttackTime;                        // Время последней атаки

    // Имена параметров в Animator
    private static readonly int WalkParameter = Animator.StringToHash("Walk");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();            // Инициализируем NavMeshAgent
        animator = GetComponent<Animator>();             // Инициализируем Animator
        lastAttackTime = -attackCooldown;                // Чтобы атака могла начаться сразу

        // Находим игрока по тегу
        FindPlayer();
    }

    void Update()
    {
        // Если игрок не найден, пытаемся найти снова
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // Двигаем врага к игроку
        agent.SetDestination(player.position);

        // Обновляем анимацию ходьбы
        UpdateWalkAnimation();

        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Поворачиваем врага к игроку
            //FacePlayer();

            // Если прошло достаточно времени с последней атаки
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();                                // Атакуем игрока
                lastAttackTime = Time.time;              // Обновляем время атаки
            }
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Игрок найден: " + playerObj.name);
        }
        else
        {
            Debug.LogWarning("Игрок с тегом '" + playerTag + "' не найден!");
        }
    }

    private void UpdateWalkAnimation()
    {
        if (animator != null)
        {
            // Устанавливаем параметр Walk в зависимости от скорости движения
            bool isWalking = agent.velocity.magnitude > 0.1f;
            animator.SetBool(WalkParameter, isWalking);
        }
    }

    /*private void FacePlayer()
    {
        if (player == null) return;

        // Вычисляем направление к игроку
        Vector3 direction = (player.position - transform.position).normalized;
        // Игнорируем ось Y, чтобы враг не наклонялся вверх/вниз
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Плавно поворачиваем врага
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }*/

    private void Attack()
    {
        if (audioSource != null && PounchSound != null)
        {
            audioSource.PlayOneShot(PounchSound);
        }
        // Запускаем анимацию атаки
        if (animator != null)
        {
            animator.SetTrigger(AttackTrigger);
        }
        // Наносим урон игроку
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Враг атаковал игрока!");
        }
    }
}