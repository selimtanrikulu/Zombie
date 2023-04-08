using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public struct CharacterWearings
{
    public Sprite Balaclava;
    public Sprite Body;
    public Sprite Googles;
    public Sprite Head;
    public Sprite HeadGear;
    public Sprite Strap;
    public Sprite Jacket;
    public Sprite Mask;
    public Sprite PantsCore;
    public Sprite Pants1Left;
    public Sprite Pants1Right;
    public Sprite Pants2Left;
    public Sprite Pants2Right;
    public Sprite BootsLeft;
    public Sprite BootsRight;
    public Sprite Shirt;
    public Sprite RightHand1;
    public Sprite RightHand2;
    
    public Sprite RightHand3;
    public Sprite LeftHand1;
    public Sprite LeftHand2;
    public Sprite LeftHand3;
}

public enum CharacterConfig
{
    MaxMovementSpeed = 100,
    MaxEvade = 100,
    MaxHp = 5000,
}

public static class CharacterGradeConfig
{
    public const float GradeHpFactor = 1.1f;
    public const float GradeMovementSpeedFactor = 1.1f;
    public const float GradeEvadeFactor = 1.1f;
}

[CreateAssetMenu( menuName = "ScriptableObjects/Character")]
public class Character : Item
{
    [SerializeField] public CharacterID characterID;
    [SerializeField] public CharacterWearings CharacterWearings;
    [SerializeField] public int movementSpeed;
    [SerializeField] public int evade;
    [SerializeField] public int hp;


    public override Item GetCopy()
    {
        Character copy = CreateInstance<Character>();
        copy.characterID = characterID;
        copy.CharacterWearings = CharacterWearings;
        copy.movementSpeed = movementSpeed;
        copy.evade = evade;
        copy.hp = hp;
        copy.itemSprite = itemSprite;
        copy.Grade = Grade;
        return copy;
    }

    public override List<Field> GetAllFields()
    {
        List<Field> result = new List<Field>();
        result.Add(new Field("HP",hp,(int)CharacterConfig.MaxHp));
        result.Add(new Field("Movement Speed",movementSpeed,(int)CharacterConfig.MaxMovementSpeed));
        result.Add(new Field("Evade",evade,(int)CharacterConfig.MaxEvade));
        return result;
    }

    public override void Upgrade()
    {
        Grade++;
    }

    
    
}

