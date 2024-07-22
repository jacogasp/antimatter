using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class RunningJump : PlayerState
  {
    private Vector2 entryAbsVelocity;
    private bool directionChanged = false;
    private float t = 0;

    public override void Enter(Player player) {
      var velocity = player.Velocity;
      velocity.Y = -player.JumpVelocity.Y;
      player.Velocity = velocity;
      entryAbsVelocity = velocity.Abs();
      GD.Print("running jump");
    }

    public override IState<Player> HandleInput(Player player) {
      if (player.IsOnFloorOnly()) {
        return new Idle();
      }
      if (player.IsNearWall()) {
        return new OnWall();
      }
      if (Input.IsActionJustPressed("jump")) {
        return new AirJump();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      if (player.DirectionJustChanged) {
        directionChanged = true;
      }
      if (directionChanged) {
        velocity.X = player.FallingVelocity.X * Player.HorizontalInputAxis;
      } else if (Mathf.Abs(Player.HorizontalInputAxis) < float.Epsilon) {
        var k = Mathf.Clamp(t / player.JumpHorizontalDump, 0, 1);
        velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
        velocity.X *= player.FacingDirection.X;
        t += delta;
      } else {
        velocity.X = player.Speed * Player.HorizontalInputAxis;
      }
      player.Velocity = velocity;
    }
  }
}
