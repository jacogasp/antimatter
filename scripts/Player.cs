using Algorithms;
using Godot;

namespace Characters
{
  public partial class Player : CharacterBody2D
  {
    [Export]
    public float Speed { get; set; } = 5.0f;

    [Export]
    public float HorizontalDump { get; set; } = 1.0f;

    [Export]
    public float JumpVelocity { get; set; } = 4.5f;

    [Export]
    public float AngularSpeed { get; set; } = 1.0f;

    private readonly StateMachine<Player> stateMachine = new();
    private Vector2 _acceleration = Vector2.Zero;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private readonly float gravity = ProjectSettings
      .GetSetting("physics/2d/default_gravity")
      .AsSingle();

    public override void _Ready()
    {
      stateMachine.CurrentState = new Idle();
      GD.Print("Player ready");
    }

    public override void _PhysicsProcess(double delta)
    {
      stateMachine.Update(this, delta);
      AddForce(Vector2.Down * gravity);
      var velocity = Velocity;
      velocity += _acceleration * (float)delta;
      velocity.X = Mathf.Lerp(Velocity.X, 0, HorizontalDump * 0.01f);
      Velocity = velocity;
      MoveAndSlide();
      _acceleration *= Vector2.Zero;
    }

    public void AddForce(Vector2 force)
    {
      _acceleration += force;
    }

    public static Vector2 InputDirection
    {
      get
      {
        return Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
      }
    }
  }

  // States

  class Idle : State<Player>
  {
    public override void Enter(Player gameObject)
    {
      GD.Print("idle");
    }

    public override State<Player> HandleInput(Player gameObject)
    {
      if (Input.IsActionJustPressed("jump") && gameObject.IsOnFloor())
      {
        return new Jumping();
      }
      if (Player.InputDirection != Vector2.Zero)
      {
        return new Running();
      }
      return this;
    }

    public override void Update(Player gameObject, float delta)
    {

    }
  }

  class Running : State<Player>
  {
    public override void Enter(Player gameObject)
    {
      GD.Print("run");
    }

    public override State<Player> HandleInput(Player gameObject)
    {
      if (Input.IsActionJustPressed("jump") && gameObject.IsOnFloor())
      {
        return new Jumping();
      }
      if (Player.InputDirection == Vector2.Zero)
      {
        return new Idle();
      }
      return this;
    }

    public override void Update(Player gameObject, float delta)
    {
      var velocity = gameObject.Velocity;
      velocity.X = Player.InputDirection.X * gameObject.Speed;
      gameObject.Velocity = velocity;
    }
  }

  class Jumping : State<Player>
  {
    public override void Enter(Player gameObject)
    {
      GD.Print("jump");
    }

    public override State<Player> HandleInput(Player gameObject)
    {
      if (gameObject.IsOnFloor())
      {
        return new Idle();
      }
      return this;
    }

    public override void Update(Player gameObject, float delta)
    {
      gameObject.AddForce(Vector2.Up * gameObject.JumpVelocity);
    }
  }
}
