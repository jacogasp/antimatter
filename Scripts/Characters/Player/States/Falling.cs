using Godot;

namespace Antimatter.Scripts.Characters.Player.States
{
  public partial class Falling : PlayerState
  {
    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }
    [Export] public PlayerState OnWall { get; set; }
    [Export] public float OnWallSafeMargin { get; set; }

    Vector2 _entryPosition;
    Vector2 _entryVelocity;

    public override void Enter(PlayerClass player) {
      GD.Print("falling");
      _entryPosition = player.Position;
      _entryVelocity = player.Velocity.Abs();
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (player.IsNearWall()) {
        var distanceSq = player.Position.X - _entryPosition.X;
        distanceSq *= distanceSq;
        if (distanceSq > OnWallSafeMargin) {
          return OnWall;
        }
      }
      if (player.IsGrounded()) {
        return Idle;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      velocity.X = _entryVelocity.X * PlayerClass.HorizontalInputAxis;
      player.Velocity = velocity;
    }
  }
}
