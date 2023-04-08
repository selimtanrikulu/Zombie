using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Weapons.Guns
{
    public enum GunType
    {
        Rifle,
        Heavy,
        Pistol,
    }


    public enum GunConfig
    {
        MaxAmmo = 240,
        MaxFireRate = 600,
    }

    
    [CreateAssetMenu( menuName = "ScriptableObjects/Gun")]
    public class Gun : Weapon
    {
        public GunType gunType;
        public GunID gunID;
        public AudioClip fireSound;
        [SerializeField] public int ammo;
        [SerializeField] public int fireRate;
        private DateTime _lastTimeFired;

        //Handles input, cooldown, deltatime information and returns whether time to attack or not
        public bool FireReady()
        {
            float cd = 60f / fireRate;
            DateTime now = DateTime.UtcNow;
            if ((now-_lastTimeFired).TotalSeconds > cd)
            {
                _lastTimeFired = now;
                ammo--;
                return true;
            }
            return false;
        }
        
        public override Item GetCopy()
        {
            Gun copy = CreateInstance<Gun>();
            copy.gunType = gunType;
            copy.gunID = gunID;
            copy.itemSprite = itemSprite;
            copy.fireSound = fireSound;
            copy.damage = damage;
            copy.fireRate = fireRate;
            copy.criticalChance = criticalChance;
            copy.ammo = ammo;
            copy.Grade = Grade;
            return copy;
        }

        public override List<Field> GetAllFields()
        {
            List<Field> result = new List<Field>();
            result.Add(new Field("Damage",damage,(int)WeaponConfig.MaxDamage));
            result.Add(new Field("Critical Chance",criticalChance,(int)WeaponConfig.MaxCriticalChance));
            result.Add(new Field("Fire Rate",fireRate,(int)GunConfig.MaxFireRate));
            result.Add(new Field("Ammo",ammo,(int)GunConfig.MaxAmmo));
            return result;
        }

        public override void Upgrade()
        {
            Grade++;
        }


        
    }
}