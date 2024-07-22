using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class AirJump : PlayerState
  {
    private Vector2 entryAbsVelocity;
    private float t = 0;

    public override void Enter(Player player) {
      var velocity = player.Velocity;
      if (velocity.Y > -player.AirJumpVelocity.Y) {
        velocity.Y = -player.AirJumpVelocity.Y;
      }
      player.Velocity = velocity;
      entryAbsVelocity = velocity.Abs();
      GD.Print("air jump");
    }

    public override IState<Player> HandleInput(Player player) {
      if (player.IsOnFloor()) {
        return new Idle();
      }
      if (player.IsNearWall()) {
        return new OnWall();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      if (Mathf.Abs(Player.HorizontalInputAxis) < float.Epsilon) {
        var velocity = player.Velocity;
        var k = Mathf.Clamp(t / player.JumpHorizontalDump, 0, 1);
        velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
        velocity.X *= player.FacingDirection.X;
        t += delta;
        player.Velocity = velocity;
      }
    }
  }
}
