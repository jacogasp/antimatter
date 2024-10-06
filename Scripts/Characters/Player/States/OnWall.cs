using System.Threading;
using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class OnWall : PlayerState
  {
    [ExportGroup("On Wall Parameters")]
    [Export] float WallDumping { get; set; }
    [Export] float Timeout { get; set; } = 1.0f;
    [Export] float PowerWallJumpTimeout { get; set; } = 1.0f;


    [ExportGroup("Connected States")]
    [Export] PlayerState Idle { get; set; }
    [Export] PlayerState Falling { get; set; }
    [Export] PlayerState WallJump { get; set; }
    [Export] PlayerState PowerWallJump { get; set; }

    private float _discharge;
    private float _tStartJumpPress;
    private float _t;

    public override void Enter(PlayerClass player) {
      GD.Print("on wall");
      _discharge = Timeout;
      _tStartJumpPress = -1;
      _t = 0;
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (Input.IsActionJustPressed("jump")) {
        _tStartJumpPress = _t;
        return this;
      }

      if (Input.IsActionJustReleased("jump")) {
        float dt = _t - _tStartJumpPress;
        return dt < PowerWallJumpTimeout ? WallJump : PowerWallJump;
      }

      if (!player.IsNearWall()) {
        return Falling;
      }

      if (player.IsGrounded()) {
        return Idle;
      }

      if (_tStartJumpPress < 0 && _discharge < 0) {
        GD.Print("OnWall discharged");
        return Falling;
      }

      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      if (velocity.Y > 0) {
        velocity.Y *= WallDumping;
      }
      player.Velocity = velocity;
      _discharge -= delta;
      _t += delta;
    }
  }
}
