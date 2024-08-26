using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerScript : MonoBehaviour
{

	[SerializeField]
	private GameObject wand;

	public GameObject Projectile;
	public GameObject deathScreen;

	[SerializeField]
	private float _speed = 1f;
	private Vector2 moveDirection;
	private Vector2 lookDirection;

	public int PierceCount = 1;
	public int Damage = 10;
	public int Health = 100;
	public int AttackSpeed = 10;
	public bool Dead = false;

	// Start is called before the first frame update
	void Start()
	{
      
	}
  
  // Update is called once per frame
	void Update()
	{
		if(Dead == false)
		{
			Move();
			Look();
		}
	}

    private void FixedUpdate() {
		if (_attackQueued) {
				Fire();
			}
    }

    //Moving
    public void MoveInput(InputAction.CallbackContext context)
	{
		moveDirection = context.ReadValue<Vector2>();
	}

	private bool _attackQueued;
	public void AttackInput(InputAction.CallbackContext context) {
		if (context.action.WasPressedThisFrame()) {
            _attackQueued = true;
        }

		if (context.action.WasReleasedThisFrame()) {
			_attackQueued = false;
		}
		
	}

	public void Fire() {
		var projectile = GameObject.Instantiate(Projectile);
		projectile.transform.position = transform.position;
		projectile.GetComponent<ProjectileController>().AssignTargetPoint(_mouseScreenPosition);
	}

	public void Move()
	{
		if(moveDirection != Vector2.zero)
		{
			Vector3 direction = new Vector3(moveDirection.x, moveDirection.y, 0).normalized;
			transform.position += direction * (_speed * Time.deltaTime);
		}
	}

	//Looking
	public void LookInput(InputAction.CallbackContext context)
	{
		lookDirection = context.ReadValue<Vector2>();
	}

	private Vector3 _mouseScreenPosition;

	public void Look()
	{
		if(lookDirection != Vector2.zero)
		{

			Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(lookDirection);
			_mouseScreenPosition = new Vector3(mouseLocation.x, mouseLocation.y);
			mouseLocation.z = 0f;

			Vector3 direction = mouseLocation - transform.position;

			float angle = (Mathf.Atan2(direction.y, direction.x) - math.PI/2) * Mathf.Rad2Deg;
			wand.transform.RotateAround(transform.position, transform.up, 100);
			wand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		}
	}

	public void PlayerTakeDamage(float damage)
	{
		Health -= (int)damage;
		
		if(Health < 0)
		{
			Dead = true;
			//stuff for game state dead
			deathScreen.SetActive(true);
		}


	}

}
