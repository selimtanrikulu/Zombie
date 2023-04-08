using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Weapons.Melees
{

    [CreateAssetMenu( menuName = "ScriptableObjects/Melee")]
    public class Melee : Weapon
    {
        
        [FormerlySerializedAs("meleeName")] public MeleeID meleeID;
        public AudioClip hitSound;

        
        public override Item GetCopy()
        {
            Melee copy = CreateInstance<Melee>();
            copy.meleeID = meleeID;
            copy.itemSprite = itemSprite;
            copy.hitSound = hitSound;
            copy.damage = damage;
            copy.criticalChance = criticalChance;
            copy.Grade = Grade;
            return copy;
        }

        public override List<Field> GetAllFields()
        {
            List<Field> result = new List<Field>();
            result.Add(new Field("Damage",damage,(int)WeaponConfig.MaxDamage));
            result.Add(new Field("Critical Chance",criticalChance,(int)WeaponConfig.MaxCriticalChance));
            return result;
        }

        public override void Upgrade()
        {
            Grade++;
        }
    }
}