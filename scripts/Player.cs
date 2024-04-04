using Algorithms;
using Godot;

namespace Characters
{
  public partial class Player : CharacterBody2D
  {
    [Export]
    public float Speed { get; set; } = 5.0f;

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
      HandleMovement();
      var velocity = Velocity;
      velocity += _acceleration * (float)delta;
      velocity.X = Mathf.Lerp(Velocity.X, 0, 0.1f);
      Velocity = velocity;
      MoveAndSlide();
      _acceleration *= Vector2.Zero;
    }

    void AddForce(Vector2 force)
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


    void HandleMovement()
    {
      var target = Position + InputDirection;
      var desired = target - Position;
      desired = desired.Normalized() * Speed;
      var steering = desired - Velocity;
      AddForce(steering * AngularSpeed);
    }
  }

  // States

  class Idle : State<Player>
  {
    public override void Enter(Player gameObject)
    {
      GD.Print("enter idle");
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
      GD.Print("enter running");
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
      GD.Print("enter jumping");
      var velocity = gameObject.Velocity;
      velocity.Y = gameObject.JumpVelocity;
      gameObject.Velocity = velocity;
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

    }
  }
}
