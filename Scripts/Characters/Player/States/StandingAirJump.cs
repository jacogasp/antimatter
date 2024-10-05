using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class StandingAirJump : PlayerState
  {
    [ExportGroup("Standing Air Jump Parameters")]
    [Export] Vector2 AirJumpVelocity { get; set; } = Vector2.One;

    [ExportGroup("Connected States")]
    [Export] PlayerState Idle { get; set; }
    [Export] PlayerState OnWall { get; set; }

    public override void Enter(PlayerClass player) {
      var velocity = player.Velocity;
      velocity.Y = -AirJumpVelocity.Y;
      player.Velocity = velocity;
      GD.Print("standing air jump");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (player.IsOnFloor()) {
        return Idle;
      }
      if (player.IsNearWall()) {
        return OnWall;
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
