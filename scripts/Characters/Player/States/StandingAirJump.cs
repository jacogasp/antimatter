using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class StandingAirJump : PlayerState
  {
    public override void Enter(Player player) {
      var velocity = player.Velocity;
      velocity.Y = -player.AirJumpVelocity.Y;
      player.Velocity = velocity;
      GD.Print("standing air jump");
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
      var velocity = player.Velocity;
      velocity.X = player.AirJumpVelocity.X * Player.HorizontalInputAxis;
      player.Velocity = velocity;
    }
  }
}
