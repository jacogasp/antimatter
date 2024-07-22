using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class OnWall : PlayerState
  {
    public override void Enter(Player player) {
      GD.Print("on wall");
    }
    public override IState<Player> HandleInput(Player player) {
      if (Input.IsActionJustReleased("jump")) {
        return new WallJump();
      }

      if (!player.IsNearWall()) {
        return new Falling();
      }

      if (player.IsOnFloor()) {
        return new Idle();
      }
      return this;
    }
    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      velocity.Y *= player.WallDumping;
      player.Velocity = velocity;

    }
  }
}
