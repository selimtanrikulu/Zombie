using System;
using ScriptableObjects.Weapons;
using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameLevelUI : MonoBehaviour
{
    [SerializeField] private Button changeWeaponButton;
    [SerializeField] private Text weaponOnHandAmmoText;

    private Player _player;

    private bool _drag;
    
    public void PlayerCreated(Player player)
    {
        _player = player;
        changeWeaponButton.image.sprite = _player.GetWeaponOnHand().itemSprite;

        Weapon w = _player.GetWeaponOnHand();
        if (w is Gun gun) weaponOnHandAmmoText.text = gun.ammo.ToString();
        else weaponOnHandAmmoText.text = "";
        changeWeaponButton.onClick.AddListener(ChangeWeaponButtonOnClick);
    }
    
    public void FireDone()
    {
        Weapon w = _player.GetWeaponOnHand();
        if(w is Gun gun)weaponOnHandAmmoText.text = gun.ammo.ToString();
        else weaponOnHandAmmoText.text = "";
    }


    public void WeaponChanged()
    {
        Weapon w = _player.GetWeaponOnHand();
        changeWeaponButton.image.sprite = _player.GetWeaponOnHand().itemSprite;
        if(w is Gun gun)weaponOnHandAmmoText.text = gun.ammo.ToString();
        else weaponOnHandAmmoText.text = "";
    }
    
    private void ChangeWeaponButtonOnClick()
    {
        if (_player.GetWeaponOnHand() is Melee)
        {
            _player.HoldLastWeapon();
        }
        else
        {
            _player.HoldNextGun();
        }
    }

    public void OnFireButtonPointerDown()
    {
        _player.FireButtonTapped();
        _drag = true;
    }

    public void OnFireButtonPointerUp()
    {
        _drag = false;
    }

    public void MeleeButtonPointerDown()
    {
        _player.MeleeButtonTapped();
    }

    private void Update()
    {
        if(_drag)_player.FireButtonHold();
    }

    //for testing
    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }

    //---------------------------
}
