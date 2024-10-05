using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class WallJump : PlayerState
  {
    [ExportGroup("Wall Jump Parameters")]
    [Export] Vector2 JumpVelocity { get; set; } = Vector2.One;
    [Export] float WallSafeMargin { get; set; } = 1.0f;

    [ExportGroup("Connected States")]
    [Export] PlayerState Idle { get; set; }
    [Export] PlayerState AirJump { get; set; }
    [Export] PlayerState Diving { get; set; }
    [Export] PlayerState OnWall { get; set; }

    private bool _wallLost = false;
    private Vector2 _lastWallPosition;

    public override void Enter(PlayerClass player) {
      GD.Print("wall jump");
      var velocity = JumpVelocity;
      velocity.X *= PlayerClass.HorizontalInputAxis;
      velocity.Y *= -1;
      player.Velocity = velocity;
      _wallLost = false;
      _lastWallPosition = player.Position;
    }

    public override PlayerState HandleInput(PlayerClass player) {
      var distance = player.Position - _lastWallPosition;
      if (player.IsNearWall() && distance.LengthSquared() > WallSafeMargin) {
        return OnWall;
      }

      if (player.IsGrounded()) {
        return Idle;
      }
      if (Input.IsActionJustPressed("jump")) {
        return AirJump;
      }
      if (Input.IsActionJustPressed("down")) {
        return Diving;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      velocity.X = JumpVelocity.X * PlayerClass.HorizontalInputAxis;
      player.Velocity = velocity;
      var position = player.Position;
      position.X += 10f * PlayerClass.HorizontalInputAxis;
      player.Position = position;
    }
  }
}
