using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Inventory
{
    private Dictionary<ScriptableItem, int> _items;
    [SerializeField] private int _maxItems;
    [SerializeField] private int _nbItems;

    public Inventory()
    {
        _items = new Dictionary<ScriptableItem, int>();
    }

    public void AddItem(ScriptableItem item)
    {
        Assert.IsTrue(_nbItems < _maxItems);
        _nbItems++;
        if (_items.ContainsKey(item))
        {
            _items[item]++;
        }
        else
        {
            _items.Add(item, 1);
        }
    }

    public void RemoveItem(ScriptableItem item)
    {
        Assert.IsTrue(_items.ContainsKey(item));
        _nbItems--;
        if (_items.ContainsKey(item))
        {
            _items[item]--;
            if (_items[item] == 0)
            {
                _items.Remove(item);
            }
        }
    }

    public bool Contains(ScriptableItem item) => _items.ContainsKey(item);

    public int GetNbItems(ScriptableItem item) => _items[item];

    public int GetNbItems() => _nbItems;

    public int LeftCapacity() => _maxItems - _nbItems;
}
