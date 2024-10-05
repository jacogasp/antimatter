using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class StandingJump : PlayerState
  {
    [ExportGroup("Standing Jump Parameters")]
    [Export] public Vector2 JumpVelocity { get; set; } = Vector2.One;

    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }
    [Export] public PlayerState OnWall { get; set; }
    [Export] public PlayerState StandingAirJump { get; set; }

    public override void Enter(PlayerClass player) {
      var velocity = player.Velocity;
      velocity.Y = -JumpVelocity.Y;
      player.Velocity = velocity;
      GD.Print("standing jump");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (player.IsOnFloor()) {
        return Idle;
      }
      if (player.IsNearWall()) {
        return OnWall;
      }
      if (Input.IsActionJustPressed("jump")) {
        return StandingAirJump;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      velocity.X = player.FallingVelocity.X * PlayerClass.HorizontalInputAxis;
      player.Velocity = velocity;
    }
  }
}
