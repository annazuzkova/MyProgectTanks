using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private TMP_Text waveText;
    private int currentWave = 0;
    private int aliveEnemies = 0;

    private int[] enemiesPerWave = { 3, 5, 6 };

    private void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;
        waveText.text = "Attack " + currentWave;

        int waveIndex = currentWave - 1;

        if (waveIndex >= enemiesPerWave.Length)
        {
            waveIndex = enemiesPerWave.Length - 1;
        }

        int enemyCount = enemiesPerWave[waveIndex];

        aliveEnemies = enemyCount;

        enemySpawner.SpawnWave(enemyCount);
    }

    public void EnemyDied()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            StartCoroutine(NextWave());
        }
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(2f);

        StartNextWave();
    }
}