using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

  public GameObject wand;

  public float speed = 1f;
  private Vector2 moveDirection;

	private Vector2 lookDirection;




	// Start is called before the first frame update
	void Start()
  {
      
  }
  
  // Update is called once per frame
  void Update()
  {
    Move();
		Look();
	}



  //Moving
	public void MoveInput(InputAction.CallbackContext context)
  {
		//Debug.Log("MoveInput Recieved:: X:" + context.ReadValue<Vector2>().x + " Y: " + context.ReadValue<Vector2>().y);
    moveDirection = context.ReadValue<Vector2>();
  }

  public void Move()
  {
		if(moveDirection != Vector2.zero)
		{
      //Debug.Log("MOVING:: X: " + moveDirection.x + " Y: " + moveDirection.y);
		  Vector3 direction = new Vector3(moveDirection.x, moveDirection.y, 0).normalized;
		  transform.position += direction * (speed * Time.deltaTime);
		}
	}

	//Looking
	public void LookInput(InputAction.CallbackContext context)
	{

		Debug.Log("LookInput Recieved:: X:" + context.ReadValue<Vector2>().x + " Y: " + context.ReadValue<Vector2>().y);
		lookDirection = context.ReadValue<Vector2>();
	}

	public void Look()
	{
		if(lookDirection != Vector2.zero)
		{
			Debug.Log("LOOKING:: X: " + lookDirection.x + " Y: " + lookDirection.y);

			Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(lookDirection);
			mouseLocation.z = 0f;

			Vector3 direction = mouseLocation - transform.position;

			float angle = (Mathf.Atan2(direction.y, direction.x) - math.PI/2) * Mathf.Rad2Deg;
			wand.transform.RotateAround(transform.position, transform.up, 100);
			wand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		}
	}



}
