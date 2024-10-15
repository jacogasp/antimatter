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
      var lookDirection = new Vector2(
        Input.GetAxis("aim_left", "aim_right"),
        Input.GetAxis("aim_up", "aim_down")
      );
      if (lookDirection.LengthSquared() < Mathf.Epsilon) {
        lookDirection = new Vector2(player.FacingDirection.X, -0.5f);
      }
      var target = lookDirection.Normalized() * 100;
      target.X += Mathf.Epsilon * player.FacingDirection.X;
      var weapons = player.Inventory.GetInventory(InventoryItem.Type.weapon);
      var weapon = (Weapon)weapons.CurrentItem();
      weapon.Aim(target);
    }
  }
}
