using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public ItemData[] ingredients;
    public int[] ingredientAmounts;
    public ItemData[] results;
    public int[] resultAmounts;
    public int craftingTime;
    public bool isCraftable;
    public bool isSmeltable;
}
