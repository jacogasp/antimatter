using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class StateMachine : Node
  {
    [Export] public PlayerState CurrentState { get; set; }

    private PlayerClass _player;

    public override void _Ready() {
      _player = GetParent<PlayerClass>();
      CurrentState.Enter(_player);
    }

    public override void _PhysicsProcess(double delta) {
      var nextState = CurrentState.HandleInput(_player);
      if (nextState == null) {
        return;
      }
      if (nextState != CurrentState) {
        nextState.Enter(_player);
      }
      CurrentState = nextState;
      CurrentState.HandleInput(_player);
      CurrentState.Update(_player, (float)delta);
    }
  }
}
