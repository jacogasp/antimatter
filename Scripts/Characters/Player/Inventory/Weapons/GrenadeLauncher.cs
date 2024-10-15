using Antimatter.Scripts.Settings;
using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory.Weapons
{
  public partial class GrenadeLauncher : Weapon
  {
    [Export]
    public float BulletSpeed { get; set; } = 1000f;
    private string _itemName = "Grenade Launcher";
    public override string ItemName => _itemName;

    private bool _aimExpired = false;
    public override bool AimingExpired => _aimExpired;

    private float _aimTimer;
    private bool _aiming = false;
    private Vector2 _target = Vector2.Zero;

    private Vector2[] _trajectory = new Vector2[96];
    private int _lastDot = 0;
    private TileMapLayer _tilemap;

    public override void _Ready() {
      Reset();
      _lastDot = _trajectory.Length;
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
      var direction = (_target - Position).Normalized();
      var velocity = BulletSpeed * Vector2.One;
      float step = 0.05f;
      float t = 0;
      for (var i = 0; i < _trajectory.Length; ++i) {
        t += step;
        _trajectory[i] = ProjectileTrajectory(Position, velocity, direction, t);
        if (CheckIntersection(_trajectory[i] + GlobalPosition)) {
          _lastDot = i;
          break;
        }
      }
      QueueRedraw();
    }

    public override void Fire() {
      Reset();
    }

    public override void _Draw() {
      if (_aiming) {
        DrawLine(Position, _target, Color.Color8(255, 255, 255), 3);
        for (var i = 0; i < _lastDot; ++i) {
          var point = _trajectory[i];
          DrawCircle(point, 5, Color.Color8(255, 255, 255));
        }
      }
    }

    private Vector2 ProjectileTrajectory(
        Vector2 initialPosition,
        Vector2 initialVelocity,
        Vector2 direction,
        float t
      ) {
      var projectilePosition = initialPosition + initialVelocity * direction * t;
      projectilePosition.Y += 0.5f * Physics2D.Gravity * t * t;
      return projectilePosition;
    }

    private bool CheckIntersection(Vector2 position) {
      var spaceState = GetWorld2D().DirectSpaceState;
      var parameters = new PhysicsPointQueryParameters2D {
        Position = position,
        CollisionMask = 4294967294 // everything but the player
      };
      var result = spaceState.IntersectPoint(parameters);
      return result.Count > 0;
    }
  }
}
