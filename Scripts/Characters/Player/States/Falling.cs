using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class Falling : PlayerState
  {
    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }
    [Export] public PlayerState OnWall { get; set; }
    [Export] public float OnWallSafeMargin { get; set; }

    Vector2 _entryPosition;

    public override void Enter(PlayerClass player) {
      GD.Print("falling");
      _entryPosition = player.Position;
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (player.IsNearWall()) {
        var distanceSq = player.Position.X - _entryPosition.X;
        distanceSq *= distanceSq;
        if (distanceSq > OnWallSafeMargin) {
          return OnWall;
        }
      }
      if (player.IsOnFloorOnly()) {
        return Idle;
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
