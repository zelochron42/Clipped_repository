using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    [SerializeField] float enemyRespawnTime = 5f;
    void Start()
    {

    }
    void Update()
    {
        
    }

    public void AddToRespawn(EnemyHealth enemy) {
        GameObject newEnemy = Instantiate(enemy.gameObject);
        newEnemy.SetActive(false);
        enemy.Death.AddListener(() => {
            Respawn(newEnemy, enemyRespawnTime);
        });
    }

    public void Respawn(GameObject enemy, float waitTime) {
        StartCoroutine(SpawnEnemy(enemy, waitTime));
    }

    IEnumerator SpawnEnemy(GameObject enemy, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.SetActive(true);
        EnemyHealth neh = newEnemy.GetComponent<EnemyHealth>();
        neh.Death.AddListener(() => {
            Respawn(enemy, enemyRespawnTime);
        });
        yield break;
    }
}
