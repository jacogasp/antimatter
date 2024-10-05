using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class Diving : PlayerState
  {
    [ExportGroup("Diving Parameters")]
    [Export] Vector2 DiveVelocity { get; set; } = Vector2.One;
    [Export] float DischargeTime { get; set; } = 100;

    [ExportGroup("Connected States")]
    [Export] PlayerState Idle { get; set; }
    private float _discharge;

    public override void Enter(PlayerClass player) {
      var velocity = player.Velocity;
      velocity.X = 0;
      player.Velocity = velocity;
      _discharge = DischargeTime;
      GD.Print("dive");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (player.IsOnGround()) {
        if (_discharge > 0) {
          --_discharge;
        } else {
          return Idle;
        }
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {
      var velocity = player.Velocity;
      velocity.Y = DiveVelocity.Y;
      player.Velocity = velocity;
    }
  }
}
