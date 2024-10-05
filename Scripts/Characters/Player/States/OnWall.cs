using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class OnWall : PlayerState
  {
    [ExportGroup("On Wall Parameters")]
    [Export] float WallDumping { get; set; }
    [Export] float Timeout { get; set; } = 1.0f;

    [ExportGroup("Connected States")]
    [Export] PlayerState Idle { get; set; }
    [Export] PlayerState Falling { get; set; }
    [Export] PlayerState WallJump { get; set; }

    private float _discharge;

    public override void Enter(PlayerClass player) {
      GD.Print("on wall");
      _discharge = Timeout;
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (Input.IsActionJustReleased("jump")) {
        return WallJump;
      }

      if (!player.IsNearWall()) {
        return Falling;
      }

      if (player.IsGrounded()) {
        return Idle;
      }

      if (_discharge < 0) {
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
    }
  }
}
