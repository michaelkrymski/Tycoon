using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();
    private Recipe[] recipeData;

    private void Start()
    {
        recipeData = Resources.LoadAll<Recipe>("Recipes");
        for(int i = 0; i < recipeData.Length; i++)
        {
            recipes.Add(recipeData[i].recipeName, recipeData[i]);
        }
    }

    public static Recipe GetRecipeAtIndex(string index)
    {
        return recipes[index];
    }
}
