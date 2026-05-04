using UnityEngine;

public class TrayMealTracker : MonoBehaviour
{
    public FoodItemData mainItem;
    public FoodItemData side1Item;
    public FoodItemData side2Item;
    public FoodItemData drinkItem;

    public void RegisterItem(FoodSlotType slotType, FoodItemData item)
    {
        switch (slotType)
        {
            case FoodSlotType.Main:
                mainItem = item;
                break;
            case FoodSlotType.Side1:
                side1Item = item;
                break;
            case FoodSlotType.Side2:
                side2Item = item;
                break;
            case FoodSlotType.Drink:
                drinkItem = item;
                break;
        }
    }

    public bool HasRequiredFood()
    {
        return mainItem != null && side1Item != null && side2Item != null;
    }
    public void ClearItem(FoodSlotType slotType)
    {
        switch (slotType)
        {
            case FoodSlotType.Main:
                mainItem = null;
                break;
            case FoodSlotType.Side1:
                side1Item = null;
                break;
            case FoodSlotType.Side2:
                side2Item = null;
                break;
            case FoodSlotType.Drink:
                drinkItem = null;
                break;
        }
    }

    public bool HasDrink()
    {
        return drinkItem != null;
    }

    public bool IsMealSafe()
    {
        if (mainItem == null || side1Item == null || side2Item == null || drinkItem == null)
            return false;

        return !mainItem.isPoisoned &&
            !side1Item.isPoisoned &&
            !side2Item.isPoisoned &&
            !drinkItem.isPoisoned;
    }
}