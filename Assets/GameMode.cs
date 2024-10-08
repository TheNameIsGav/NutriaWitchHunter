using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{

    public static GameState GameState = GameState.StartScreen;
    public static Camera MainCamera;
    public GameObject Player;
    public PlayerScript PlayerScript;
    public static GameMode GM; 


    private static float _internalDifficulty = 1f;
    public static float GlobalDifficulty => _internalDifficulty <= 200f ? _internalDifficulty : 200f;

    [SerializeField]
    public static float PlayerScore = 0f;

    private void Awake() {
        MainCamera = Camera.main;
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerScript>();
        if(GM != null) {
            GameObject.Destroy(GM);
        } else {
            GM = this;
        }

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayerScore(float scoreIncrease) {
        Debug.Log($"Updated Player Score by {scoreIncrease}");
        PlayerScore += scoreIncrease;
    }

    public void DealPlayerDamage(float damage) {
        Debug.Log($"Dealt {damage} damage to player");
        Player.transform.GetComponent<PlayerScript>().PlayerTakeDamage(damage);
    }
}

public enum GameState {
    StartScreen,
    Gaming,
    Paused,
    Death
}
