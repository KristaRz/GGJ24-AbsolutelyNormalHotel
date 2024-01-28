
using Unity.XR.CoreUtils;
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

    public enum WeaponType { None, Shuriken, Katana }

    public int ChickenScore = 0;
    public int CurrentLevel = 1;
    public int LevelLength = 0;

    public int Level1Time = 120;
    public int Level2Time = 120;
    public int Level3Time = 120;

    public UnityEvent<int> OnLevelStart;
    public UnityEvent<int> OnFinishLevelWithScore;

    private bool _weaponSet = false;
    public WeaponType WeaponCurrent;
    public void SetWeapon(WeaponType weaponType)
    {
        WeaponCurrent = weaponType;
        _weaponSet = true;
        FindObjectOfType<WeaponHandler>().SetWeapon(weaponType);
    }

    public WeaponType GetWeapon()
    {
        if (!_weaponSet)
            return WeaponType.None;
        else
            return WeaponCurrent;
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
            case 0: LevelLength = 0; break;
        }
    }

    public void LevelStart()
    {
        OnLevelStart.Invoke(LevelLength);
    }
    
    public void LevelStart(int delayTime)
    {
        Invoke("LevelStart", delayTime);
    }
    
    public void FinishLevel()
    {
        ViewFader viewFader = FindObjectOfType<ViewFader>();
        viewFader.OnScreenOff.AddListener(CalculateScore);
        viewFader.BlinkScreen();
        CalculateScore();
    }

    private void CalculateScore()
    {
        ChickenGame[] allChickens = FindObjectsOfType<ChickenGame>();
        ChickenScore = LevelHandler.Instance.GetFinalChickenKillCount();

        float score = ChickenScore/ allChickens.Length * 100;
        int levelScore = (int)score;

        FindObjectOfType<TeleportPlayer>().TeleportTo(GameObject.Find("ElevatorPlayerTarget").transform);
        OnFinishLevelWithScore.Invoke(levelScore);
    }
}
