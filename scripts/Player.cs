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
    public Vector2 JumpVelocity { get; set; } = new(50.0f, 800.0f);

    [Export]
    public Vector2 JumpStandingVelocity { get; set; } = new(200, 800);

    [Export]
    public float JumpHorizontalDump { get; set; } = 0.1f;


    [Export]
    public Vector2 FallingVelocity { get; set; } = new(20f, 1000f);

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

    public static float HorizontalInputAxis
    {
      get
      {
        return Input.GetAxis("ui_left", "ui_right");
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
        return new Jumping(false);
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
        return new Jumping(true);
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
      velocity.X = Player.HorizontalInputAxis * gameObject.Speed;
      gameObject.Velocity = velocity;
    }
  }

  class Jumping : State<Player>
  {
    private Vector2 facingDirection;
    private readonly bool _wasRunning;
    private float speedX = 0;
    private float minSpeedX = 0;
    private bool directionChanged = false;
    private float t = 0;

    public Jumping(bool wasRunning)
    {
      _wasRunning = wasRunning;
    }

    public override void Enter(Player gameObject)
    {
      var velocity = gameObject.Velocity;
      facingDirection = velocity.X > 0 ? Vector2.Right : Vector2.Left;
      float jumpYSpeed;
      if (_wasRunning)
      {
        speedX = Mathf.Abs(velocity.X);
        jumpYSpeed = gameObject.JumpVelocity.Y;
      }
      else
      {
        speedX = Mathf.Abs(gameObject.JumpStandingVelocity.X);
        jumpYSpeed = gameObject.JumpStandingVelocity.Y;
      }
      velocity += Vector2.Up * jumpYSpeed;
      gameObject.Velocity = velocity;
      minSpeedX = Mathf.Abs(gameObject.JumpVelocity.X);
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
      var velocity = gameObject.Velocity;
      if (gameObject.Velocity.X * Player.HorizontalInputAxis < 0)
      {
        directionChanged = true;
        facingDirection.X *= -1;
      }

      var vX = Mathf.Abs(speedX * Player.HorizontalInputAxis);
      if (_wasRunning)
      {
        if (directionChanged)
        {
          vX = gameObject.FallingVelocity.X;
        }
        else if (vX < speedX)
        {
          var k = Mathf.Clamp(t * gameObject.JumpHorizontalDump, 0, 1);
          vX = Mathf.Lerp(speedX, minSpeedX, k);
          t += delta;
        }
      }
      vX *= facingDirection.X;
      var vY = Mathf.Clamp(velocity.Y, -Mathf.Inf, gameObject.FallingVelocity.Y);
      velocity.X = vX;
      velocity.Y = vY;
      gameObject.Velocity = velocity; 
    }
  }
}
