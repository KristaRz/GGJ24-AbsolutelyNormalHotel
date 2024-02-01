
using UnityEngine;
using UnityEngine.Events;

public class WeaponHandler : MonoBehaviour
{
    private GameManager.WeaponType _currentWeaponType;

    [SerializeField] private GameObject Shuriken; 
    [SerializeField] private GameObject Katana;

    public UnityEvent<GameManager.WeaponType> OnWeaponTypeChanged;

    private void Start()
    {
        OnWeaponTypeChanged.AddListener(ActivateWeapon);

        _currentWeaponType = GameManager.Instance.GetWeapon();
        OnWeaponTypeChanged.Invoke(_currentWeaponType);
    }

    public void SetWeapon(GameManager.WeaponType weaponType)
    {
        _currentWeaponType = weaponType;
        OnWeaponTypeChanged?.Invoke(_currentWeaponType);
    }

    public void ActivateWeapon(GameManager.WeaponType weaponType)
    {
        if (_currentWeaponType == GameManager.WeaponType.None)
            return;
        if (_currentWeaponType == GameManager.WeaponType.Shuriken)
            Shuriken.SetActive(true);
        else
            Katana.SetActive(true);
    }
}
