using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class Running : PlayerState
  {
    public override void Enter(Player player) {
      GD.Print("run");
    }

    public override IState<Player> HandleInput(Player player) {
      if (Input.IsActionJustReleased("jump")) {
        if (player.IsOnWall()) {
          return new StandingJump();
        }
        return new RunningJump();
      }
      if (player.Velocity.Y > 0) {
        return new Falling();
      }
      if (Player.InputDirection == Vector2.Zero) {
        return new Idle();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      velocity.X = Player.HorizontalInputAxis * player.Speed;
      player.Velocity = velocity;
    }
  }
}
