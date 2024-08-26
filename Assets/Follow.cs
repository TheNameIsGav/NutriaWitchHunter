using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = GameMode.GM.Player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, -10);
    }
}
