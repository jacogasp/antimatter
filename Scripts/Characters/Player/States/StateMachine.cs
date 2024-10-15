using Godot;

namespace Antimatter.Scripts.Characters.Player.States
{
  public partial class StateMachine : Node
  {
    [Export] public PlayerState CurrentState { get; set; }

    private PlayerClass _player;

    public override void _Ready() {
      _player = GetParent<PlayerClass>();
      CurrentState.Enter(_player);
    }

    public void HandleInput() {
      var nextState = CurrentState.HandleInput(_player);
      if (nextState == null) {
        return;
      }
      if (nextState != CurrentState) {
        nextState.Enter(_player);
      }
      CurrentState = nextState;
      CurrentState.HandleInput(_player);
    }

    public override void _Process(double delta) {
      HandleInput();
      CurrentState.Update(_player, (float)delta);
    }

    public override void _PhysicsProcess(double delta) {
      CurrentState.FixedUpdate(_player, (float)delta);
    }
  }
}
