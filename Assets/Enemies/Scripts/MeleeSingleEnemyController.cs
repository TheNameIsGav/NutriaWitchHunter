using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeSingleEnemyController : MonoBehaviour
{

    public GameObject player;

    private Camera mainCamera => GameMode.MainCamera;

    public int MoveSpeed = 5;
    public float Damage => 20 * GameMode.GlobalDifficulty;
    public float Health => 40 * GameMode.GlobalDifficulty;
    public float Value => 20 * GameMode.GlobalDifficulty/2;


    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(player == null) { //Somehow we didn't get passed the player
            player = GameMode.Player;
        }

        FindSpawnPosition();
    }

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
    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer() {
        if (player != null) {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        //We hit the player, so do stuff with that. 
        if (collision.gameObject.name == player.name) {
            GameMode.GM.DealPlayerDamage(Damage);
            Destroy(gameObject);
        }
        
        //We were hit by a player's projectile so should die or lose health. 
        if(collision.gameObject.tag == "playerProjectile") {

            //Deal Damage to self based on projectile.
            if(Health <= 0) {
                GameMode.GM.UpdatePlayerScore(Value);
                Destroy(gameObject);
            }
            
        }
    }
}
