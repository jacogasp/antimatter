
namespace Algorithms
{
  public interface IState<T>
  {
    public void Enter(T gameObject);
    public abstract IState<T> HandleInput(T gameObject);
    public abstract void Update(T gameObject, float delta);
  }

  public class StateMachine<T>
  {
    public IState<T> CurrentState { get; set; }

    public void Update(T gameObject, double delta) {
      var nextState = CurrentState.HandleInput(gameObject);
      if (nextState == null) {
        return;
      }
      if (nextState != CurrentState) {
        nextState.Enter(gameObject);
      }
      CurrentState = nextState;
      CurrentState.HandleInput(gameObject);
      CurrentState.Update(gameObject, (float)delta);
    }
  }
}
