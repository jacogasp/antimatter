using Godot;

namespace Antimatter.Scripts.Characters.Player.States
{
  using Inventory;
  using Inventory.Weapons;

  public partial class Firing : PlayerState
  {

    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }

    public override void Enter(PlayerClass player) {
      GD.Print("firing!");
      var weapons = player.Inventory.GetInventory(InventoryItem.Type.weapon);
      var weapon = (Weapon)weapons.CurrentItem();
      weapon.Fire();
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (!Input.IsActionPressed("fire")) {
        return Idle;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {

    }
  }
}
