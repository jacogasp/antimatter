using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class OnWall : PlayerState

  {
    private float _discharge = 50;

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

      if (_discharge > 0) {
        --_discharge;
      } else {
        return new Falling();
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
