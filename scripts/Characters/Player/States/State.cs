using Algorithms;

namespace Characters.Player.States
{
  class PlayerState : IState<Player>
  {
    public virtual void Enter(Player player) {
      throw new System.NotImplementedException();
    }

    public virtual IState<Player> HandleInput(Player player) {
      throw new System.NotImplementedException();
    }

    public virtual void Update(Player player, float delta) {
      throw new System.NotImplementedException();
    }
  }
}
