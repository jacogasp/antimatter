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
      var directionChanged = player.FacingDirection.X * Player.HorizontalInputAxis < 0;
      if (Input.IsActionJustReleased("jump") && directionChanged) {
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
      if (velocity.Y > 0) {
        velocity.Y *= player.WallDumping;
      }
      player.Velocity = velocity;

    }
  }
}
