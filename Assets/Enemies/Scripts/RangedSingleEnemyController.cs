using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangedSingleEnemyController : MonoBehaviour
{
    public GameObject player;

    private Camera mainCamera => GameMode.MainCamera;

    public int MoveSpeed = 5;
    public int RotateSpeed = 100;
    public int Range = 10;
    public int ProjectileSpeed = 1;
    public float Damage;
    public float Health;
    public int ShotTimer = 2;
    public float Value;

    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { //Somehow we didn't get passed the player
            player = GameMode.GM.Player;
        }

        FindSpawnPosition();

        _rotationDirection = Random.value > .5f ? 1 : -1;

        Damage = 10 * GameMode.GlobalDifficulty;
        Health = 10 * GameMode.GlobalDifficulty;
        Value = 10 * GameMode.GlobalDifficulty/2;

        InvokeRepeating("Shoot", 0, ShotTimer);
    }

    private int _rotationDirection;

    public int SpawnDistance;
    void FindSpawnPosition() {
        // Get the screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Choose a random edge (top, bottom, left, right)
        int edge = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (edge) {
            case 0: // Top
                spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(Random.Range(0, screenWidth), screenHeight + 20, mainCamera.nearClipPlane));
                break;
            case 1: // Bottom
                spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(Random.Range(0, screenWidth), -20, mainCamera.nearClipPlane));
                break;
            case 2: // Left
                spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(-20, Random.Range(0, screenHeight), mainCamera.nearClipPlane));
                break;
            case 3: // Right
                spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth + 20, Random.Range(0, screenHeight), mainCamera.nearClipPlane));
                break;
        }

        // Return the spawn position in world space
        transform.position = new Vector2(spawnPosition.x, spawnPosition.y);
    }

    // Update is called once per frame
    void Update() {
        if(Vector2.Distance(transform.position, player.transform.position) > Range) {
            MoveTowardsPlayer();
        } else {
            CirclePlayer();
            //MaintainDistance();
        }

    }

    public GameObject Projectile;

    void Shoot() {
        //Fire Projectile at the player
        if (Projectile != null) {
            var newProj = GameObject.Instantiate(Projectile);
            newProj.transform.position = transform.position;
        }
    }

    void CirclePlayer() {
        if (player != null) {
            // Calculate the current angle based on the time and orbit speed
            float angle = RotateSpeed * Time.deltaTime * _rotationDirection;
            transform.RotateAround(player.transform.position, player.transform.forward, angle);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void MaintainDistance() {
        if (player != null) {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Vector2 newPosition = (Vector2)transform.position + direction * Range;
            transform.position = newPosition;
        }
    }

    void MoveTowardsPlayer() {
        if (player != null) {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "PlayerProjectile") {
            collision.gameObject.GetComponent<ProjectileController>().RegisterHit();
            Health -= collision.gameObject.GetComponent<ProjectileController>().Damage;

            if (Health <= 0) {
                GameMode.GM.UpdatePlayerScore(Value);
                Destroy(gameObject);
            }
        }
    }
}
