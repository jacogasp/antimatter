using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class Falling : PlayerState
  {
    public override void Enter(Player player) {
      GD.Print("falling");
    }

    public override IState<Player> HandleInput(Player player) {
      if (player.IsOnFloorOnly()) {
        return new Idle();
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      velocity.X = player.Speed * Player.HorizontalInputAxis;
      player.Velocity = velocity;
    }
  }
}
