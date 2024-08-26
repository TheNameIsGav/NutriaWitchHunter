using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Public Fields

    [SerializeField]
    public GameObject MeleeEnemy;

    [SerializeField]
    public GameObject RangedEnemy;

    [SerializeField]
    public int Variance = 0;
    #endregion

    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        if(playerObject == null) {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        DontDestroyOnLoad(this);

        Invoke("SetActive", 3);
    }

    private Stack<GameObject> meleeWaitingPool = new Stack<GameObject>();
    private List<GameObject> meleeActivePool = new List<GameObject>();

    private Stack<GameObject> rangedWaitingPool = new Stack<GameObject>();
    private List<GameObject> rangedActivePool = new List<GameObject>();

    private bool _isActive = false;
    private void SetActive() {
        _isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive) {
            if (meleeWaitingPool.Count <= 0) {
                PopulateMeleeEnemies();
            }
            if (rangedWaitingPool.Count <= 0) {
                PopulateRangedEnemies();
            }

            if ((rangedActivePool.Count + meleeActivePool.Count) < 15 || _timeSinceLastSpawn >= _spawnCooldownInSeconds) {
                SpawnNewWave();
            }
        }
    }

    [SerializeField]
    private int _spawnCooldownInSeconds = 20;
    private int _timeSinceLastSpawn = 0;

    void SpawnNewWave() {
        var enemySeed = EnemySpawnEquation();
        var enemiesToSpawn = Random.Range(enemySeed - Variance, enemySeed + Variance);
        _timeSinceLastSpawn = 0;

        for (int i = 0; i < enemiesToSpawn; i++) {
            float r = Random.value;

            if(r < .5) {
                SpawnRangedEnemy();
            }
            if(r >= .5) {
                SpawnMeleeEnemy();
            }
        }
        StartCoroutine(SpawnCooldown());
    }

    IEnumerator SpawnCooldown() {
        
        while (_timeSinceLastSpawn < _spawnCooldownInSeconds) {
            yield return new WaitForSeconds(1);
            //Debug.Log($"{_timeSinceLastSpawn} seconds since last spawn");
            _timeSinceLastSpawn++;
        }
    }



    //Moves Waiting Enemies to the Active Pools
    void SpawnRangedEnemy() {
        if (rangedWaitingPool.Count == 0) { 
            PopulateRangedEnemies();
        }

        if (rangedWaitingPool.Peek() != null) {
            GameObject newEnemy = rangedWaitingPool.Pop();
            newEnemy.SetActive(true);
            rangedActivePool.Add(newEnemy);
        }
    }

    void SpawnMeleeEnemy() {

        if(meleeWaitingPool.Count == 0) {
            PopulateMeleeEnemies();
        }

        if (meleeWaitingPool.Peek() != null) {
            GameObject newEnemy = meleeWaitingPool.Pop();
            newEnemy.SetActive(true);
            meleeActivePool.Add(newEnemy);
        }
    }

    //Populates Waiting Pools of Enemies
    void PopulateMeleeEnemies () {

        var spawnCount = (int)(EnemySpawnEquation() * 1.5);
        for (int i = 0; i < spawnCount; i++){
            var newEnemy = GameObject.Instantiate(MeleeEnemy);
            newEnemy.transform.position = new Vector3(0, 0, -10);
            meleeWaitingPool.Push(newEnemy);
        }
    }

    void PopulateRangedEnemies() {
        var spawnCount = (int)(EnemySpawnEquation() * 1.5);
        for (int i = 0; i < spawnCount; i++) {
            var newEnemy = GameObject.Instantiate(RangedEnemy);
            newEnemy.transform.position = new Vector3(0, 0, -10);
            rangedWaitingPool.Push(newEnemy);
        }
    }

    int EnemySpawnEquation() { return (int)(Mathf.Pow(1000, (GameMode.GlobalDifficulty / 200)) + 10); }
}
