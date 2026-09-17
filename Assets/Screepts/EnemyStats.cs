using UnityEngine;
[CreateAssetMenu(fileName ="New Enemy Stats", menuName ="Enemy Stats")]

public class EnemyStats : ScriptableObject
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;

    public int Health => _health;
    public float Speed => _speed;
}
