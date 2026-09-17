using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyCont : MonoBehaviour
{
    private int _health;
    private float _speed;
    private float _searchRange = 10f;
    private float searchTimer = 0f;
    private float searchCD = 2f;
    private float attackTimer = 0f;
    private float attackCD = 2f;
    [SerializeField] private LayerMask searchLayer;
    private WaveManager waveManager;
    private PlayerCont target;
    private bool isDead = false;
    private Transform randomPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator _anim;
    private float attackRange = 2f;
    private int damage = 1;
    public EnemyState enemy_state {  get; private set; }=EnemyState.Idle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
    }
    private void SearchForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            _searchRange,
            searchLayer
        );

        foreach (Collider collider in colliders)
        {
            PlayerCont player = collider.GetComponentInParent<PlayerCont>();

            if (player != null && player.isAlive)
            {
                target = player;
                enemy_state = EnemyState.Chase;
                return;
            }
        }

        // Якщо гравця тимчасово не знайшли —
        // НЕ втрачаємо стару ціль
        if (target != null && target.isAlive)
        {
            enemy_state = EnemyState.Chase;
            return;
        }

        target = null;
        enemy_state = EnemyState.Idle;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead)
            return;

        //Debug.Log(gameObject.name + " STATE: " + enemy_state);

        searchTimer += Time.deltaTime;

        _anim.SetFloat("Speed", agent.velocity.magnitude);

        if (searchTimer >= searchCD)
        {
            searchTimer = 0;
            SearchForPlayer();
        }

        switch (enemy_state)
        {
            case EnemyState.Idle:
                Move();
                break;

            case EnemyState.Chase:
                GoTo();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }
    public void Initialize(EnemyStats stats)
    {
        _health = stats.Health;
        _speed = stats.Speed;
    }
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        _health -= damage;

        //Debug.Log("Enemy отримав шкоду: " + damage);
        //Debug.Log("HP Enemy: " + _health);
        if (_health <= 0)
        {
            isDead = true;

            agent.isStopped = true;
            agent.ResetPath();

            GetComponent<Collider>().enabled = false;

            _anim.SetTrigger("Death");

            waveManager.EnemyDied();

            StartCoroutine(DestroyAfterDeath());
        }
    }

    private void Die()
    {
        enemy_state = EnemyState.None;

        agent.isStopped = true;

        _anim.SetTrigger("Death");
        waveManager.EnemyDied();
    }
    private void Move()
    {

    }
    private void GoTo()
    {
        if (target == null) return;
        agent.SetDestination(target.transform.position);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < attackRange)
        {
            enemy_state = EnemyState.Attack;
        }

    }
    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer <= attackCD) return;

        attackTimer = 0;

        if (target == null)
        {
            Debug.Log("Target is NULL");
            return;
        }

        _anim.SetTrigger("AttackEnemy");


        Debug.Log("Enemy attacks player!");

        target.TakeDamage(damage);
    }

    private IEnumerator DestroyAfterDeath()
    {
        // Чекаємо, поки закінчиться Death-анімація
        yield return new WaitForSeconds(4.6f);

        // Ще 2 секунди тіло лежить
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
public enum EnemyState
{
    None=0,
    Idle=1,
    Chase=2,
    Attack=3
}
