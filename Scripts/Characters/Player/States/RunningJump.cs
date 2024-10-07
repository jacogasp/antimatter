using Godot;

namespace Antimatter.Scripts.Characters.Player.States
{
  public partial class RunningJump : PlayerState
  {
    [ExportGroup("Running Jump Parameters")]
    [Export] Vector2 JumpVelocity { get; set; }
    [Export(PropertyHint.Range, "0.1,1,")]
    float JumpHorizontalDump { get; set; } = 1;
    [Export] Vector2 DirectionChangeVelocity { get; set; }

    [ExportGroup("Connected States")]
    [Export] PlayerState Idle;
    [Export] PlayerState OnWall;
    [Export] PlayerState AirJump;

    private Vector2 entryAbsVelocity;
    private float _t = 0;
    private bool _detached = false;

    public override void Enter(PlayerClass player) {
      var velocity = player.Velocity;
      velocity.Y = -JumpVelocity.Y;
      player.Velocity = velocity;
      entryAbsVelocity = velocity.Abs();
      _detached = false;
      _t = 0;
      GD.Print("running jump");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (_detached && player.IsOnGround()) {
        GD.Print("on ground");
        return Idle;
      }
      if (player.IsFrontNearWall()) {
        return OnWall;
      }
      if (Input.IsActionJustPressed("jump")) {
        return AirJump;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      if (player.DirectionJustChanged) {
        velocity.X = DirectionChangeVelocity.X * PlayerClass.HorizontalInputAxis;
      } else if (Mathf.Abs(PlayerClass.HorizontalInputAxis) < Mathf.Epsilon) {
        var k = Mathf.Clamp(_t / JumpHorizontalDump, 0, 1);
        velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
        velocity.X *= player.FacingDirection.X;
        _t += delta;
      }
      player.Velocity = velocity;
      if (!player.IsOnGround()) {
        _detached = true;
      }
    }
  }
}
