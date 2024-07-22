using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class WallJump : PlayerState
  {
    public override void Enter(Player player) {
      GD.Print("wall jump");
      var velocity = player.WallJumpVelocity;
      velocity.X *= Player.HorizontalInputAxis;
      velocity.Y *= -1;
      player.Velocity = velocity;
    }

    public override IState<Player> HandleInput(Player player) {
      if (player.IsNearWall()) {
        return new OnWall();
      }
      if (player.IsOnFloor()) {
        return new Idle();
      }
      if (Input.IsActionJustPressed("jump")) {
        return new AirJump();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      velocity.X = player.WallJumpVelocity.X * Player.HorizontalInputAxis;
      player.Velocity = velocity;
    }
  }
}
