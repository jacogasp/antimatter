using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class StandingJump : PlayerState
  {
    public override void Enter(Player player) {
      var velocity = player.Velocity;
      velocity.Y = -player.StandingJumpVelocity.Y;
      player.Velocity = velocity;
      GD.Print("standing jump");
    }

    public override IState<Player> HandleInput(Player player) {
      if (player.IsOnFloor()) {
        return new Idle();
      }
      if (player.IsNearWall()) {
        return new OnWall();
      }
      if (Input.IsActionJustPressed("jump")) {
        return new StandingAirJump();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      velocity.X = player.StandingJumpVelocity.X * Player.HorizontalInputAxis;
      player.Velocity = velocity;
    }
  }
}
