using UnityEngine;

public class ProjectTileCont : MonoBehaviour
{
    [SerializeField] private float _pushForce = 10f;
    [SerializeField] private Rigidbody _rb;

    private int _damage = 10;

    [SerializeField] private float _lifeTime = 5f;
    private float _timer = 0f;

    public void Initialized(int damage, Vector3 pushDirection)
    {
        _damage = damage;
        _timer = 0f;

        // Скидаємо стару швидкість кулі
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Запускаємо кулю в потрібному напрямку
        _rb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _lifeTime)
        {
            OnExplosion();
        }
    }

    private void OnExplosion()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Куля зіткнулась з: " + collision.gameObject.name);

        if (collision.gameObject.TryGetComponent<EnemyCont>(out EnemyCont enemy))
        {
            enemy.TakeDamage(_damage);
        }

        OnExplosion();
    }
}
