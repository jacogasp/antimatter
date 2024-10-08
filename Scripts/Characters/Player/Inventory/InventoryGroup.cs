using System.Collections.Generic;
using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory
{
  public partial class InventoryGroup
  {
    [Export]
    public int MaxItems { get; set; }
    private readonly List<InventoryItem> _items = new();
    private int _currentItemIndex = 0;

    public void AddItem(InventoryItem item) {
      _items.Add(item);
    }

    public void DropItem(InventoryItem item) {
      throw new System.NotImplementedException();
      if (_currentItemIndex >= _items.Count) {
        _currentItemIndex = _items.Count - 1;
      }
    }

    public void DropCurrentItem() {
      DropItem(CurrentItem());
    }

    public InventoryItem CurrentItem() {
      return _items.Count > 0 ? _items[_currentItemIndex] : null;
    }

    public InventoryItem NextItem() {
      ++_currentItemIndex;
      if (_currentItemIndex == _items.Count) {
        _currentItemIndex = 0;
      }
      return CurrentItem();
    }

    public InventoryItem PreviousItem() {
      --_currentItemIndex;
      if (_currentItemIndex < 0) {
        _currentItemIndex = _items.Count - 1;
      }
      return CurrentItem();
    }

    public int Size() {
      return _items.Count;
    }
  }
}
