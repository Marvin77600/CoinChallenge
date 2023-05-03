using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Dictionary<string, Item> items;

    public Dictionary<string, Item> Items => items;

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<string, Item>();
    }

    public void AddItem(Item _item)
    {
        items.Add(_item.ItemName, _item);
    }

    public bool HaveItem(string _itemName)
    {
        bool flag = false;
        if (items.ContainsKey(_itemName))
        {
            flag = true;
        }
        return flag;
    }

    public void Clear() => items.Clear();
}