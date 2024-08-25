using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    public int Damage = 10;
    public int Speed = 10;
    public int Lifetime = 5;

    private Vector3 targetPoint;

    void Start() {
        Invoke("Destroy", Lifetime);
        targetPoint = GameMode.Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
