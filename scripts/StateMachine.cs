
namespace Algorithms
{
  public abstract class State<T>
  {
    public abstract void Enter(T gameObject);
    public abstract State<T> HandleInput(T gameObject);
    public abstract void Update(T gameObject, float delta);
  }

  public class StateMachine<T>
  {
    public State<T> CurrentState { get; set; }

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
