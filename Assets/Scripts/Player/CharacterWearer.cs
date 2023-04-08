using System;
using ScriptableObjects.Weapons;
using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct ItemSocketMaps
{
    public SpriteRenderer WeaponSocket;
    public SpriteRenderer BalaclavaSocket;
    public SpriteRenderer BodySocket;
    public SpriteRenderer GooglesSocket;
    public SpriteRenderer HeadSocket;
    public SpriteRenderer HeadGearSocket;
    public SpriteRenderer StrapSocket;
    public SpriteRenderer JacketSocket;
    public SpriteRenderer MaskSocket;
    public SpriteRenderer PantsCoreSocket;
    public SpriteRenderer Pants1LeftSocket;
    public SpriteRenderer Pants1RightSocket;
    public SpriteRenderer Pants2LeftSocket;
    public SpriteRenderer Pants2RightSocket;
    public SpriteRenderer BootsLeftSocket;
    public SpriteRenderer BootsRightSocket;
    public SpriteRenderer ShirtSocket;
    public SpriteRenderer RightHand1;
    public SpriteRenderer RightHand2;
    public SpriteRenderer RightHand3;
    public SpriteRenderer LeftHand1;
    public SpriteRenderer LeftHand2;
    public SpriteRenderer LeftHand3;
    public SpriteRenderer MeleeSocket;
}



public class CharacterWearer : MonoBehaviour
{

    [SerializeField] public ItemSocketMaps itemSocketMaps;

    
    [SerializeField] private Sprite leftHandWhenMeleeSprite;
    [SerializeField] private Sprite leftHandWhenGunSprite;
    private float leftHandZRotWhenMelee = 120;
    private float leftHandZRotWhenGun = 41.309f;
    [SerializeField] private GameObject boneRightArm;
    [SerializeField] private GameObject boneLeftArm;
    private float rightArm1ZRotHeavy = 323;
    private float rightArm1ZRotPistol = 245;


    private Player _player;

    private void Start()
    {
        
        //To transfer animation notifiers
        _player = GetComponentInParent<Player>();
    }

    public void MapWeapon(Weapon weapon)
    {
        if (weapon is Melee)
        {
            itemSocketMaps.WeaponSocket.sprite = null;
            itemSocketMaps.MeleeSocket.sprite = weapon.itemSprite;
        }
        else
        {
            itemSocketMaps.WeaponSocket.sprite = weapon.itemSprite;
            itemSocketMaps.MeleeSocket.sprite = null;
        }
    }
    
    public void MapCharacterWearings(Character character)
    {
        //character wearings
        itemSocketMaps.BalaclavaSocket.sprite = character != null ? character.CharacterWearings.Balaclava : null;
        itemSocketMaps.BodySocket.sprite = character != null ? character.CharacterWearings.Body : null;
        itemSocketMaps.GooglesSocket.sprite = character != null ? character.CharacterWearings.Googles : null;
        itemSocketMaps.HeadSocket.sprite = character != null ? character.CharacterWearings.Head : null;
        itemSocketMaps.StrapSocket.sprite = character != null ? character.CharacterWearings.Strap : null;
        itemSocketMaps.HeadGearSocket.sprite = character != null ? character.CharacterWearings.HeadGear : null;
        itemSocketMaps.JacketSocket.sprite = character != null ? character.CharacterWearings.Jacket : null;
        itemSocketMaps.MaskSocket.sprite = character != null ? character.CharacterWearings.Mask : null;
        itemSocketMaps.PantsCoreSocket.sprite = character != null ? character.CharacterWearings.PantsCore : null;
        itemSocketMaps.Pants1LeftSocket.sprite = character != null ? character.CharacterWearings.Pants1Left : null;
        itemSocketMaps.Pants1RightSocket.sprite = character != null ? character.CharacterWearings.Pants1Right : null;
        itemSocketMaps.Pants2LeftSocket.sprite = character != null ? character.CharacterWearings.Pants2Left : null;
        itemSocketMaps.Pants2RightSocket.sprite = character != null ? character.CharacterWearings.Pants2Right : null;
        itemSocketMaps.BootsLeftSocket.sprite = character != null ? character.CharacterWearings.BootsLeft : null;
        itemSocketMaps.BootsRightSocket.sprite = character != null ? character.CharacterWearings.BootsRight : null;
        itemSocketMaps.ShirtSocket.sprite = character != null ? character.CharacterWearings.Shirt : null;
        itemSocketMaps.RightHand1.sprite = character != null ? character.CharacterWearings.RightHand1 : null;
        itemSocketMaps.RightHand2.sprite = character != null ? character.CharacterWearings.RightHand2 : null;
        itemSocketMaps.RightHand3.sprite = character != null ? character.CharacterWearings.RightHand3 : null;
        itemSocketMaps.LeftHand1.sprite = character != null ? character.CharacterWearings.LeftHand1 : null;
        itemSocketMaps.LeftHand2.sprite = character != null ? character.CharacterWearings.LeftHand2 : null;
        itemSocketMaps.LeftHand3.sprite = character != null ? character.CharacterWearings.LeftHand3 : null;
    }
    
   
    
    public void SetHeavyWeaponPos()
    {
        itemSocketMaps.LeftHand3.sprite = leftHandWhenGunSprite;
        Vector3 ls = itemSocketMaps.LeftHand3.transform.localScale;
        ls.x = 1;
        itemSocketMaps.LeftHand3.transform.localScale = ls;
        Vector3 lr = itemSocketMaps.LeftHand3.transform.localRotation.eulerAngles;
        itemSocketMaps.LeftHand3.transform.localEulerAngles = new Vector3(lr.x,lr.y,leftHandZRotWhenGun);
        boneRightArm.transform.localEulerAngles = new Vector3(0,0,rightArm1ZRotHeavy);
    }

    public void SetPistolPos()
    {
        itemSocketMaps.LeftHand3.sprite = leftHandWhenGunSprite;
        Vector3 ls = itemSocketMaps.LeftHand3.transform.localScale;
        ls.x = 1;
        itemSocketMaps.LeftHand3.transform.localScale = ls;
        Vector3 lr = itemSocketMaps.LeftHand3.transform.localRotation.eulerAngles;
        itemSocketMaps.LeftHand3.transform.localEulerAngles = new Vector3(lr.x,lr.y,leftHandZRotWhenGun);
        boneRightArm.transform.localEulerAngles = new Vector3(0,0,rightArm1ZRotPistol);
    }

    public void SetMeleePos()
    {
        itemSocketMaps.LeftHand3.sprite = leftHandWhenMeleeSprite;
        Vector3 ls = itemSocketMaps.LeftHand3.transform.localScale;
        ls.x = -1;
        itemSocketMaps.LeftHand3.transform.localScale = ls;
        Vector3 lr = itemSocketMaps.LeftHand3.transform.localRotation.eulerAngles;
        itemSocketMaps.LeftHand3.transform.localEulerAngles = new Vector3(lr.x,lr.y,leftHandZRotWhenMelee);
        boneRightArm.transform.localEulerAngles = new Vector3(0,0,rightArm1ZRotHeavy);
    }
    
    
    public void HandleRightArmRotation(Weapon weapon)
    {

        if (weapon is Gun gun)
        {
            if (gun.gunType == GunType.Pistol)
            {
                SetPistolPos();
            }
            else if (gun.gunType is GunType.Heavy or GunType.Rifle)
            {
                SetHeavyWeaponPos();
            }
        }
        
        else if (weapon is Melee)
        {
            SetMeleePos();
        }
    }


    public void MeleeAttackOccured()
    {
        if(_player) _player.MeleeAttackOccured();
    }
    
    public void MeleeAttackFinished()
    {
        if(_player) _player.MeleeAttackFinished();
    }
    
}
