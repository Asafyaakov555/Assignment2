using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    [SerializeField] GameObject PlayerToInstanite;
    [SerializeField] GameObject playerPos;
    [SerializeField] GameObject EnemyNormal;
    [SerializeField] GameObject EnemyStrong;

    [SerializeField] GameObject enemyPos;
    GameObject Player;
    [SerializeField] TMP_Text winText;
    [SerializeField] TMP_Text loseText;
    
    [SerializeField]
    private GameObject MainMenu;
    public static GameManager Instance { get; private set; }
    bool gameStarted=false;//to update the value in playermovement only once

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);

        PauseGame();

    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        MainMenu.SetActive(false);
        if(!gameStarted)
        {
            Player=Instantiate(PlayerToInstanite,playerPos.transform.position,Quaternion.identity);
            PlayerMovement playermovement=Player.GetComponent<PlayerMovement>();
            playermovement.gameStarted();
            float x = UnityEngine.Random.value;
            if(x<=0.5)// random between enemies
            {
                Instantiate(EnemyNormal,enemyPos.transform.position,Quaternion.identity);
            }
            else
            Instantiate(EnemyStrong,enemyPos.transform.position,Quaternion.identity);
        
        }
            
        else return; //to make sure that the line undearneath run once
        gameStarted=true;   
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        MainMenu.SetActive(true);
       
    }
    public void turnOnTEXT(bool winORlose)
    {
        if(winORlose)
        {
            winText.gameObject.SetActive(true);
            Invoke("StopGame",5f);
            
        }
        else
        {
            loseText.gameObject.SetActive(true);
            Time.timeScale = 0;// if lose stop the game
            
        }
        
    }
    void StopGame()
    {
        Time.timeScale = 0;
    }




    
}
