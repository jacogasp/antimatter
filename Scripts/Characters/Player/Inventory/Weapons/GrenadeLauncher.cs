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
    private Vector2 _target = Vector2.Zero;

    public override void _Ready() {
      Reset();
    }

    public override void _Process(double delta) {
      if (_aiming) {
        _aimTimer -= (float)delta;
        if (_aimTimer < 0) {
          _aimExpired = true;
          _aiming = false;
          QueueRedraw();
        }
      }
    }

    private void Reset() {
      _aimTimer = AimingTimeout;
      _aimExpired = false;
      _aiming = false;
      QueueRedraw();
    }

    public override void Aim(Vector2 target) {
      _aiming = true;
      _target = target;
      QueueRedraw();
    }

    public override void Fire() {
      Reset();
    }

    public override void _Draw() {
      if (_aiming) {
        DrawLine(Position, _target, Color.Color8(255, 255, 255), 3);
      }
    }
  }
}
