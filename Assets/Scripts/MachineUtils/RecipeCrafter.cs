using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeCrafter : MonoBehaviour
{
    private MachineInventory inventory;
    [SerializeField] private Recipe selectedRecipe;
    private bool isCrafting;
    private bool canCraft;

    private void Start()
    {
        inventory = GetComponent<MachineInventory>();
        inventory.SetupInventory(new ItemData[]{ItemManager.GetItemAtIndex("BlackSquareTest")}, new ItemData[]{ItemManager.GetItemAtIndex("RedSquareTest")});
    }

    private void Update()
    {
        canCraft = true;
        foreach(ItemData ingredient in selectedRecipe.ingredients)
        {
            if (inventory.GetAmountOfItem(ingredient) < selectedRecipe.ingredientAmounts[selectedRecipe.ingredients.ToList().IndexOf(ingredient)])
            {
                canCraft = false;
            }
        }
        foreach(ItemData product in selectedRecipe.results)
        {
            if (inventory.GetIsFull(product, selectedRecipe.resultAmounts[selectedRecipe.results.ToList().IndexOf(product)]))
            {
                canCraft = false;
            }
        }
        if(canCraft && !isCrafting)
        {
            StartCoroutine(CraftItem());
        }
    }

    private IEnumerator CraftItem()
    {
        isCrafting = true;
        float time = 0;
        while(time < selectedRecipe.craftingTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        foreach(ItemData item in selectedRecipe.ingredients)
        {
            inventory.DecreaseItem(item, selectedRecipe.ingredientAmounts[selectedRecipe.ingredients.ToList().IndexOf(item)]);
        }
        foreach(ItemData item in selectedRecipe.results)
        {
            inventory.AddInventory(item, selectedRecipe.resultAmounts[selectedRecipe.results.ToList().IndexOf(item)]);
        }
        isCrafting = false;
    }
}
