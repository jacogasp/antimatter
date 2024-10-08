namespace Antimatter.Scripts.Characters.Player.States
{
  public partial class WeaponMachine : StateMachine
  {
    public override void _Process(double delta) {
      ProcessStateMachine(delta);
    }
  }
}
