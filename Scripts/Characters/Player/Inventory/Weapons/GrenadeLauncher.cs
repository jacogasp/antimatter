using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory.Weapons
{
  public partial class GrenadeLauncher : Weapon
  {
    private string _itemName = "Grenade Launcher";
    public override string ItemName => _itemName;

    private bool _aimExpired = false;
    public override bool AimingExpired => _aimExpired;

    private float _aimTimer;
    private bool _aiming = false;

    public override void _Ready() {
      _aimTimer = AimingTimeout;
    }

    public override void _Process(double delta) {
      if (!_aiming) {
        return;
      }

      _aimTimer -= (float)delta;

      if (_aimTimer < 0) {
        _aimExpired = true;
      }
    }

    public override void Aim() {
      _aiming = true;
    }

    public override void Fire() {
      _aimExpired = false;
    }
  }
}
