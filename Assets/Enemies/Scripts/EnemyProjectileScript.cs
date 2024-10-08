using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    public float Damage => 10 * GameMode.GlobalDifficulty;
    public int Speed = 10;
    public int Lifetime = 5;
    public float Value => 5 * GameMode.GlobalDifficulty;

    private Vector3 targetPoint;

    void Start() {
        Invoke("Destroy", Lifetime);
        targetPoint = GameMode.GM.Player.transform.position;
        transform.rotation = Quaternion.LookRotation(targetPoint, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.name == GameMode.GM.Player.name) {
            GameMode.GM.DealPlayerDamage(Damage);
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "PlayerProjectile") {
            collision.gameObject.GetComponent<ProjectileController>().RegisterHit();
            GameMode.GM.UpdatePlayerScore(Value);
            Destroy(gameObject);
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
