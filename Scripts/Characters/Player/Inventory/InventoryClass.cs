using System.Reflection.Metadata.Ecma335;
using Godot;
using Godot.Collections;

namespace Antimatter.Scripts.Characters.Player.Inventory
{
  public partial class InventoryClass : Node2D
  {
    private InventoryItem _lastItem = null;
    private void ReparentItem(InventoryItem item) {
      item.DisableMode = CollisionObject2D.DisableModeEnum.Remove;
      item.Reparent(this);
      item.Position = Position;
      GD.Print($"Item '{item.ItemName}' of type '{item.GetInventoryType()}' added to the inventory!");
    }

    public void AddItem(InventoryItem item) {
      // prevent adding the same item twice due to the deferred call
      if (item != _lastItem) {
        CallDeferred(nameof(ReparentItem), item);
        _lastItem = item;
      }
    }
  }
}
