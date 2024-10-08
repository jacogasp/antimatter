using System;
using System.Collections.Generic;
using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory
{
  [Tool]
  public partial class InventoryNode : Node2D
  {
    private readonly Dictionary<InventoryItem.Type, InventoryGroup> _inventory = new();
    private InventoryItem _lastItem = null;

    public override void _Ready() {
      var itemTypes = Enum.GetValues(typeof(InventoryItem.Type));
      foreach (InventoryItem.Type itemType in itemTypes) {
        _inventory.Add(itemType, new InventoryGroup());
      }
    }

    public void AddItem(InventoryItem item) {
      // prevent adding the same item twice due to the deferred call
      if (item != _lastItem) {
        _lastItem = item;
        _inventory[item.InventoryType].AddItem(item);
        CallDeferred(nameof(ReparentItem), item);
      }
    }

    public InventoryGroup GetInventory(InventoryItem.Type category) {
      return _inventory[category];
    }

    private void ReparentItem(InventoryItem item) {
      item.DisableMode = CollisionObject2D.DisableModeEnum.Remove;
      item.Reparent(this);
      item.Position = Position;
      GD.Print($"Item '{item.ItemName}' of type '{item.GetInventoryType()}' added to the inventory!");
    }
  }
}
