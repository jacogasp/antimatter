using Godot;

namespace Antimatter.Scripts.Characters.Player.States
{
  public partial class Firing : PlayerState
  {

    [ExportGroup("Connected States")]
    [Export] public PlayerState Idle { get; set; }

    public override void Enter(PlayerClass player) {
      GD.Print("firing!");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (Input.IsActionJustReleased("fire")) {
        return Idle;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {

    }
  }
}
