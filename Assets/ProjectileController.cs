using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float Damage => GameMode.GM.PlayerScript.Damage;
    public int PierceCount;
    public int Lifetime = 10;

    [SerializeField]
    private int _speed = 20;
    private Vector3 _targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        PierceCount = GameMode.GM.PlayerScript.PierceCount;
        Invoke("Destroy", Lifetime);
    }

    public void AssignTargetPoint(Vector3 targetPosition) {
        _targetPoint = targetPosition;
        fireDirection = (_targetPoint - transform.position).normalized;

        GetComponent<Rigidbody2D>().velocity = fireDirection * _speed;
        //transform.rotation = Quaternion.LookRotation(targetPosition, Vector3.up);
    }

    void Destroy() {
        Destroy(gameObject);
    }

    public void RegisterHit() {
        PierceCount--;
        if (PierceCount <= 0) {
            Destroy(gameObject);
        }
    }

    private Vector3 fireDirection;
    // Update is called once per frame
    void Update()
    {
    }
}
