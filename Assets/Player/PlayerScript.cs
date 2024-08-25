using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
  public GameObject wand;

  public float speed = 1f;
  private Vector2 inputDirection;

  

  // Start is called before the first frame update
  void Start()
  {
      
  }
  
  // Update is called once per frame
  void Update()
  {
    Move();
	}

	public void MoveInput(InputAction.CallbackContext context)
  {
		//recievedMove=true;
		Debug.Log("Recieved:: X:" + context.ReadValue<Vector2>().x + " Y: " + context.ReadValue<Vector2>().y);
    inputDirection = context.ReadValue<Vector2>();
  }


  public void Move()
  {
      Debug.Log("MOVING:: X: " + inputDirection.x + " Y: " + inputDirection.y);
		  Vector3 direction = new Vector3(inputDirection.x, inputDirection.y, 0).normalized;
		  transform.position += direction * (speed * Time.deltaTime);
	}








}
