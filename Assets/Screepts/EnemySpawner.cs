//using UnityEngine;
//using System.Collections.Generic;

//public class EnemySpawner : MonoBehaviour
//{
//    [SerializeField] private EnemyStats enemyStats;
//    [SerializeField] private EnemyCont enemyPref;

//    [SerializeField] private List<Transform> spawnPoints;


//    [SerializeField] private List<GameObject> enemyModels;

//    [SerializeField] private int _maxEnemy = 10;

//    private List<int> slotsUsed = new();

//    private void Start()
//    {
//        SpawnEnemy();
//    }

//    private void SpawnEnemy()
//    {
//        if (enemyPref == null) return;

//        if (spawnPoints.Count == 0) return;

//        if (enemyModels.Count == 0) return;

//        slotsUsed.Clear();

//        for (int i = 0; i < _maxEnemy; i++)
//        {
//            int randomIndex = GetSlotForEnemy();

//            Transform spawnPoint = spawnPoints[randomIndex];

//            EnemyCont enemy = Instantiate(
//                enemyPref,
//                spawnPoint.position,
//                spawnPoint.rotation
//            );


//            int randomModelIndex = Random.Range(0, enemyModels.Count);

//            GameObject model = Instantiate(
//                enemyModels[randomModelIndex],
//                enemy.transform
//            );

//            model.transform.localScale = Vector3.one * 0.1f;
//            model.transform.localPosition = Vector3.zero;
//            model.transform.localRotation = Quaternion.identity;

//            enemy.Initialize(enemyStats);
//        }
//    }

//    private int GetSlotForEnemy()
//    {

//        if (slotsUsed.Count >= spawnPoints.Count)
//        {
//            slotsUsed.Clear();
//        }

//        int randomIndex;

//        do
//        {
//            randomIndex = Random.Range(0, spawnPoints.Count);
//        }
//        while (slotsUsed.Contains(randomIndex));

//        slotsUsed.Add(randomIndex);

//        return randomIndex;
//    }
//}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private List<EnemyCont> enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;

    private List<int> slotsUsed = new();

    public void SpawnWave(int amount)
    {
        if (enemyPrefabs.Count == 0) return;
        if (spawnPoints.Count == 0) return;

        slotsUsed.Clear();

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = GetSlotForEnemy();

            Transform spawnPoint = spawnPoints[randomIndex];

            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);

            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                spawnPoint.position,
                out hit,
                5f,
                NavMesh.AllAreas))
            {
                EnemyCont enemy = Instantiate(
                    enemyPrefabs[randomEnemyIndex],
                    hit.position,
                    spawnPoint.rotation
                );

                enemy.Initialize(enemyStats);
            }
        }
    }

    private int GetSlotForEnemy()
    {
        if (slotsUsed.Count >= spawnPoints.Count)
        {
            slotsUsed.Clear();
        }

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, spawnPoints.Count);
        }
        while (slotsUsed.Contains(randomIndex));

        slotsUsed.Add(randomIndex);

        return randomIndex;
    }
}