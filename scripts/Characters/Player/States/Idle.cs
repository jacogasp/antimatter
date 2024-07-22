using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class Idle : PlayerState
  {
    private float t = 0;

    public override void Enter(Player player) {
      player.ResetGravityModifier();
      GD.Print("idle");
    }

    public override IState<Player> HandleInput(Player player) {
      if (Input.IsActionJustReleased("jump") && player.IsOnFloor()) {
        return new StandingJump();
      }
      if (player.Velocity.Y > 0) {
        return new Falling();
      }
      if (Player.InputDirection != Vector2.Zero) {
        return new Running();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      var speedX = Mathf.Abs(velocity.X);
      var k = Mathf.Clamp(t / player.HorizontalDump, 0, 1);
      velocity.X = Mathf.Lerp(Mathf.Abs(speedX), 0, k);
      velocity.X *= player.FacingDirection.X;
      player.Velocity = velocity;
      t += delta;
    }
  }
}
