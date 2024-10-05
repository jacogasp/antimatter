using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class Idle : PlayerState
  {
    [ExportGroup("Idle Parameters")]
    [Export] public float HorizontalDump { get; set; } = 1;

    [ExportGroup("Connected States")]
    [Export] public PlayerState Falling { get; set; }
    [Export] public PlayerState Running { get; set; }
    [Export] public PlayerState StandingJump { get; set; }

    private float t = 0;

    public override void Enter(PlayerClass player) {
      GD.Print("idle");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (Input.IsActionJustReleased("jump") && player.IsOnFloor()) {
        return StandingJump;
      }
      if (player.Velocity.Y > 0) {
        return Falling;
      }
      if (PlayerClass.InputDirection != Vector2.Zero) {
        return Running;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      var speedX = Mathf.Abs(velocity.X);
      var k = Mathf.Clamp(t / HorizontalDump, 0, 1);
      velocity.X = Mathf.Lerp(Mathf.Abs(speedX), 0, k);
      velocity.X *= player.FacingDirection.X;
      player.Velocity = velocity;
      t += delta;
    }
  }
}
