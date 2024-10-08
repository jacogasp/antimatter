namespace Antimatter.Scripts.Characters.Player.States
{
  public partial class MovementMachine : StateMachine
  {
    public override void _PhysicsProcess(double delta) {
      ProcessStateMachine(delta);
    }
  }
}
