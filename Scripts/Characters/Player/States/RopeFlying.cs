using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class RopeFlying : PlayerState
  {
    [ExportGroup("Connected States")]
    [Export] PlayerState Idle;

    public override void Enter(PlayerClass player) {
      GD.Print("rope flying");
    }

    public override PlayerState HandleInput(PlayerClass player) {
      if (player.IsOnGround()) {
        return Idle;
      }
      return this;
    }

    public override void Update(PlayerClass player, float delta) {

    }
  }
}
