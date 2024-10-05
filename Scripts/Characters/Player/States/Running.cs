using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class Running : PlayerState
  {
    [ExportGroup("Running Parameters")]
    [Export] public float Speed { get; set; } = 1.0f;

    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }
    [Export] public PlayerState RunningJump { get; set; }
    [Export] public PlayerState OnWall { get; set; }
    [Export] public PlayerState Falling { get; set; }

    public override void Enter(PlayerClass player) {
      GD.Print("run");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (Input.IsActionJustReleased("jump")) {
        if (player.IsNearWall()) {
          return OnWall;
        }
        return RunningJump;
      }

      if (player.HookAcquired && Input.IsActionJustPressed("hook")) {
      }

      if (player.Velocity.Y > 0) {
        return Falling;
      }
      if (PlayerClass.InputDirection == Vector2.Zero) {
        return Idle;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      velocity.X = PlayerClass.HorizontalInputAxis * Speed;
      player.Velocity = velocity;
    }
  }
}
