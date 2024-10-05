using Godot;

namespace Scripts.Characters.Player.States
{
  public partial class PlayerState : Node
  {
    public virtual void Enter(PlayerClass player) {
      throw new System.NotImplementedException();
    }

    public virtual PlayerState HandleInput(PlayerClass player) {
      throw new System.NotImplementedException();
    }

    public virtual void Update(PlayerClass player, float delta) {
      throw new System.NotImplementedException();
    }
  }
}
