using System;
using ScriptableObjects.Weapons;
using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using UnityEngine;



public enum GunID
{
    Ak47 = 1,
    AssaultRifle1 = 2,
    AssaultRifle2 = 3,
    GrenadeLauncher = 4,
    Shotgun1 = 5,
    Shotgun2 = 6,
    Shotgun3 = 7,
    Sniper = 8,
    NoobPistol = 9,
    M4A1 = 10,
    Bazooka = 11,
    
}


public enum MeleeID
{
    Melee1 = 1,
    Melee2 = 2,
    Melee3 = 3,
}

public enum CharacterID
{
    Character1 = 1,
    Character2 = 2,
    Character3 = 3,
    Character4 = 4,
    Character5 = 5,
    Character6 = 6,
    Character7 = 7,
    Character8 = 8,
    Character9 = 9,
    Character10 = 10,
    Character11 = 11,
    Character12 = 12,
    
}

//Current weared items
public class Equipment
{
    public Melee MeleeSocket;
    public Gun GunSocket1;
    public Gun GunSocket2;
    public Gun GunSocket3;
    public Character Character;
    
    
    //For instancing
    public Equipment GetCopy()
    {
        Equipment copy = new Equipment();
        copy.MeleeSocket = MeleeSocket != null && MeleeSocket.GetCopy() is Melee melee ? melee : null;
        copy.GunSocket1 = GunSocket1 != null && GunSocket1.GetCopy() is Gun gun1 ? gun1 : null;
        copy.GunSocket2 = GunSocket2 != null && GunSocket2.GetCopy() is Gun gun2 ? gun2 : null;
        copy.GunSocket3 = GunSocket3 != null && GunSocket3.GetCopy() is Gun gun3 ? gun3 : null;
        copy.Character = Character != null &&Character.GetCopy() is Character character ? character : null;
        return copy;
    }
    
}

public enum GunSocketID
{
    Socket1,
    Socket2,
    Socket3,
}

public interface IItemManager
{
    void WearGun(GunID gunID,GunSocketID gunSocketID);
    void UnWearGun(Gun gun);

    bool GunEquippedOtherThan(Gun gun);

    void WearMelee(MeleeID meleeID);

    void WearCharacter(CharacterID characterID);
    

    Equipment GetEquipment();

    void SetSelectedItem(Item item);
    Item GetSelectedItem();

    bool IsItemEquipped(Item item);
    
    void UpgradeItem(Item item);


    ItemPack GetItemPack();


    void ResetItemGrades();
}

public class ItemManager:IItemManager
{
    private readonly ItemPack _itemPack;
    private Equipment _equipment = new Equipment();

    private Item _selectedItem;
    
    
    ItemManager(ItemPack itemPack)
    {
        _itemPack = itemPack;
        
        
        WearCharacter(CharacterID.Character1);
        WearGun(GunID.NoobPistol,GunSocketID.Socket1);
        WearMelee(MeleeID.Melee1);
    }

    
    public void WearGun(GunID gunID,GunSocketID gunSocketID)
    {
        Gun weapon = _itemPack.Guns.Find(x => x.gunID == gunID);

        switch (gunSocketID)
        {
            case GunSocketID.Socket1:
                _equipment.GunSocket1 = weapon;
                break;
            case GunSocketID.Socket2:
                _equipment.GunSocket2 = weapon;
                break;
            case GunSocketID.Socket3:
                _equipment.GunSocket3 = weapon;
                break;
            default:
                Debug.LogError("Weapon socket unknown error");
                break;
        }
    }

    public bool GunEquippedOtherThan(Gun gun)
    {
        if (_equipment.GunSocket1 == gun)
        {
            return _equipment.GunSocket2 != null || _equipment.GunSocket3 != null;
        }
        if (_equipment.GunSocket2 == gun)
        {
            return _equipment.GunSocket1 != null || _equipment.GunSocket3 != null;
        }
        if (_equipment.GunSocket3 == gun)
        {
            return _equipment.GunSocket1 != null || _equipment.GunSocket2 != null;
        }
        Debug.LogError("Gun is not equipped");
        return false;
    }
    
    public void UnWearGun(Gun gun)
    {
        if (_equipment.GunSocket1 == gun)
        {
            _equipment.GunSocket1 = null;
        }
        else if (_equipment.GunSocket2 == gun)
        {
            _equipment.GunSocket2 = null;
        }
        else if (_equipment.GunSocket3 == gun)
        {
            _equipment.GunSocket3 = null;
        }
        else
        {
            Debug.LogError("Gun is not equipped");
        }
    }

    public void WearMelee(MeleeID meleeID)
    {
        Melee melee = _itemPack.Melees.Find(x=>x.meleeID == meleeID);
        _equipment.MeleeSocket = melee;
    }

    public void WearCharacter(CharacterID characterID)
    {
        Character character = _itemPack.Characters.Find(x=>x.characterID == characterID);
        _equipment.Character = character;
    }

    public Equipment GetEquipment()
    {
        return _equipment;
    }


    public void SetSelectedItem(Item item)
    {
        _selectedItem = item;
    }

    public Item GetSelectedItem()
    {
        return _selectedItem;
    }

    public bool IsItemEquipped(Item item)
    {
        return (item == _equipment.GunSocket1 || item == _equipment.GunSocket2 || item == _equipment.GunSocket3 ||
                item == _equipment.Character || item == _equipment.MeleeSocket);
    }

    public void UpgradeItem(Item item)
    {
        item.Upgrade();
    }

    public ItemPack GetItemPack()
    {
        return _itemPack;
    }

    public void ResetItemGrades()
    {
        foreach (Character character in _itemPack.Characters)
        {
            character.ResetGrade();
        }

        foreach (Gun gun in _itemPack.Guns)
        {
            gun.ResetGrade();
        }

        foreach (Melee melee in _itemPack.Melees)
        {
            melee.ResetGrade();
        }
        
    }
}
