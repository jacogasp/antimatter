using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class PowerWallJump : PlayerState
  {
    [ExportGroup("Power Wall Jump Parameters")]
    [Export] public Vector2 JumpVelocity { get; set; }
    [Export] public float Duration { get; set; }

    [ExportGroup("Connected States")]
    [Export] PlayerState Falling;

    private float _t;

    public override void Enter(PlayerClass player) {
      GD.Print("power wall jump");
      _t = 0;
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (!player.IsBackNearWall() && player.IsOnGround() || _t > Duration) {
        return Falling;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      var k = _t / Duration;
      velocity.X = Mathf.Lerp(0, JumpVelocity.X * PlayerClass.HorizontalInputAxis, 1 - k);
      player.Velocity = velocity;
      _t += delta;
    }
  }
}
