using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0.1f,20f)] float spawnTimer = 1f;
    [SerializeField] [Range(0f,30f)] int poolSize = 5;

    GameObject[] pool;

    void Awake() {
        populatePool();
    }

    void populatePool(){
        pool = new GameObject[poolSize];

        for (int i=0; i<pool.Length; i++){
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }
    void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    void EnableObjectInPool(){
        for (int i=0; i<pool.Length; i++){
            if (pool[i].activeInHierarchy == false){
                pool[i].SetActive(true);
                return;
            }
        }
    }

    IEnumerator spawnEnemy(){
        while(true){
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
        
    }

}
