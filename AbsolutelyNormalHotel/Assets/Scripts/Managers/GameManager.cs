
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    #endregion

    public int ChickenScore = 0;
    public int CurrentLevel = 1;
    public int LevelLength = 0;

    public int Level1Time = 120;
    public int Level2Time = 120;
    public int Level3Time = 120;

    public UnityEvent<int> OnLevelStart;
    public UnityEvent<int> OnFinishLevelWithScore;

    public bool startLevel = false;

    private void Update()
    {
        if (startLevel)
        {
            SetLevel(CurrentLevel);
            LevelStart();
        }
    }

    public void SetLevel(int levelIndex)
    {
        CurrentLevel = levelIndex;
        switch (levelIndex)
        {
            case 1:
                LevelLength = Level1Time;
                break;
            case 2: 
                LevelLength = Level2Time;  
                break;
            case 3: 
                LevelLength = Level3Time; 
                break;
        }
    }

    public void LevelStart()
    {
        OnLevelStart.Invoke(LevelLength);
    }
    
    public void FinishLevel()
    {
        CalculateScore();
    }

    private void CalculateScore()
    {
        Receptionist[] allChickens = FindObjectsOfType<Receptionist>(); // change this to chickens obviously

        float score = ChickenScore/ allChickens.Length * 100;
        int levelScore = (int)score;

        OnFinishLevelWithScore.Invoke(levelScore);
    }
}
