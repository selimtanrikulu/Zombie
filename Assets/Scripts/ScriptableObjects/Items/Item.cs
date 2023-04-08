using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Field
{
    public string Name;
    public int CurrentValue;
    public int MaxValue;


    public Field(string name, int currentValue,int maxValue)
    {
        Name = name;
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }
}

public abstract class Item : ScriptableObject
{
    public string ItemName;
    public int Grade;
    public Sprite itemSprite;

    public int requiredRank;
    
    
    public abstract Item GetCopy();


    public abstract List<Field> GetAllFields();


    public abstract void Upgrade();


    public void ResetGrade()
    {
        Grade = 1;
    }

}
