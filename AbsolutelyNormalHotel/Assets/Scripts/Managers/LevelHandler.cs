
using UnityEngine;


public class LevelHandler : MonoBehaviour
{
    public static LevelHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private int _chickensKilled = 0;

    public void CountChickenKill()
    {
        _chickensKilled++;
    }

    public int GetFinalChickenKillCount() => _chickensKilled;


    public void SetWeapon(string weaponName)
    {
        switch (weaponName)
        {
            case "Shuriken":
            GameManager.Instance.SetWeapon(GameManager.WeaponType.Shuriken);
                break;
            case "Katana":
                GameManager.Instance.SetWeapon(GameManager.WeaponType.Katana);
                break;
        }
    }
}
