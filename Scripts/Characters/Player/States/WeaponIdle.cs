using Godot;

namespace Antimatter.Scripts.Characters.Player.States
{
  using Inventory.Weapons;
  using Inventory;

  public partial class WeaponIdle : PlayerState
  {
    [ExportGroup("Connected States")]
    [Export] public PlayerState Aiming { get; set; }

    public override void Enter(PlayerClass player) {
      GD.Print("ceasing fire");
    }
    public override PlayerState HandleInput(PlayerClass player) {
      if (Input.IsActionJustPressed("fire")) {
        var weapons = player.Inventory.GetInventory(InventoryItem.Type.weapon);
        var weapon = (Weapon)weapons.CurrentItem();
        if (weapon == null) {
          return this;
        }
        return Aiming;
      }
      return this;
    }

    public override void FixedUpdate(PlayerClass player, float delta) {

    }
  }
}
