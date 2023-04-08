using UnityEngine;

namespace ScriptableObjects.Weapons
{
    public enum WeaponConfig
    {
        MaxDamage = 5000,
        MaxCriticalChance = 100,
    }
    

    
    public abstract class Weapon : Item
    {
        public int damage;
        public int criticalChance;
        

        
    }
    
    
    
    
}




