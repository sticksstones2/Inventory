using UnityEngine;
using System.Collections;

// MVVM design pattern (Model = Inventory, View = Grid, View-Model = this) 
public class InventoryGrid : MonoBehaviour
{
    Inventory inventory;
    Grid grid;

    void Start()
    {
        this.inventory = GetComponent<Inventory>();
        this.grid = GetComponent<Grid>();
       
        int wide = grid.numberWide;
        int high = grid.numberHigh;
        for (int i = 0; i < inventory.items.Length; i++)
        {
            InventoryItem item = inventory.items[i];
            item.transform.position = grid.PositionAtIndex(i);
        }
    }

    void Update()
    {

    }

}
