using Godot;
namespace Antimatter.Scripts.Characters.Player.States
{
  using Antimatter.Scripts.Characters.Player.Inventory.Weapons;
  using Inventory;
  public partial class Aiming : PlayerState
  {
    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }
    [Export] public PlayerState Firing { get; set; }

    public override void Enter(PlayerClass player) {
      GD.Print("aiming");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      var weapons = player.Inventory.GetInventory(InventoryItem.Type.weapon);
      var weapon = (Weapon)weapons.CurrentItem();
      if (weapon.AimingExpired || Input.IsActionJustReleased("fire")) {
        return Firing;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var weapons = player.Inventory.GetInventory(InventoryItem.Type.weapon);
      var weapon = (Weapon)weapons.CurrentItem();
      var target = player.GetLocalMousePosition();
      weapon.Aim(target);
    }
  }
}
