using System.Reflection.Metadata.Ecma335;
using Godot;
using Godot.Collections;

namespace Antimatter.Scripts.Characters.Player.Inventory
{
  public partial class InventoryClass : Node2D
  {
    private Array<InventoryItem> _items = new();

    private void ReparentItem(InventoryItem item) {
      item.DisableMode = CollisionObject2D.DisableModeEnum.Remove;
      item.Reparent(this);
      item.Position = Position;
      GD.Print($"Item '{item.ItemName}' of type '{item.GetInventoryType()}' added to the inventory!");
    }

    public void AddItem(InventoryItem item) {
      // prevent adding the same element twice because of the deferred call
      if (_items.Count == 0 || item != _items[^1]) {
        CallDeferred(nameof(ReparentItem), item);
        _items.Add(item);
      }
    }
    public void ListItems() {
      GD.Print(_items);
    }

  }
}
