using Godot;
using Algorithms;

namespace Characters.Player.States
{
  class Diving : PlayerState

  {
    private float _discharge = 100;

    public override void Enter(Player player) {
      var velocity = player.Velocity;
      velocity.X = 0;
      player.Velocity = velocity;
      GD.Print("dive");
    }

    public override IState<Player> HandleInput(Player player) {
      if (player.IsOnFloor()) {
        if (_discharge > 0) {
          --_discharge;
        } else {
          return new Idle();
        }
      }
      return this;
    }

    public override void Update(Player player, float delta) {
      var velocity = player.Velocity;
      velocity.Y = 10000;
      player.Velocity = velocity;
    }
  }
}
