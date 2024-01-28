using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private GameManager.WeaponType _currentWeaponType;

    [SerializeField] private GameObject Shuriken; 
    [SerializeField] private GameObject Katana; 

    private void Start()
    {
        _currentWeaponType = GameManager.Instance.GetWeapon();
        if (_currentWeaponType == GameManager.WeaponType.None)
            return;
        if(_currentWeaponType == GameManager.WeaponType.Shuriken)
            Shuriken.SetActive(true);
        else
            Katana.SetActive(true);
    }

    public void SetWeapon(GameManager.WeaponType weaponType)
    {
        _currentWeaponType =weaponType;
    }
}
