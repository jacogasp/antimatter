using Algorithms;
using Godot;
using GameSettings;
using Characters.Player.States;

namespace Characters.Player
{
  public partial class Player : CharacterBody2D
  {
    [Export]
    public float Speed { get; set; } = 5.0f;

    [Export]
    public float GravityModifier { get; set; } = 1.0f;

    [Export]
    public float HorizontalDump { get; set; } = 1.0f;

    [Export]
    public Vector2 JumpVelocity { get; set; } = new(50.0f, 800.0f);

    [Export]
    public Vector2 StandingJumpVelocity { get; set; } = new(200, 800);

    [Export]
    public Vector2 AirJumpVelocity { get; set; } = new Vector2(200, 300);

    [Export]
    public Vector2 WallJumpVelocity { get; set; } = new Vector2(200, 800);

    [Export]
    public float JumpHorizontalDump { get; set; } = 1f;

    [Export]
    public Vector2 FallingVelocity { get; set; } = new(20f, 1000f);

    [Export]
    public float WallDumping { get; set; } = 1.0f;

    [Export]
    public float AngularSpeed { get; set; } = 1.0f;

    private readonly StateMachine<Player> stateMachine = new();
    private Vector2 _facingDirection;
    private float _startGravityModifier;
    private RayCast2D _rayWall;
    private Vector2 _rayWallTarget = Vector2.Zero;
    private Vector2 _acceleration = Vector2.Zero;


    public override void _Ready() {
      _startGravityModifier = GravityModifier;
      stateMachine.CurrentState = new Idle();
      _rayWall = GetNode<RayCast2D>("WallRay");
      _rayWallTarget = _rayWall.TargetPosition;
      GD.Print("Player ready");
    }

    public override void _PhysicsProcess(double delta) {
      AddForce(Vector2.Down * Physics.Gravity * GravityModifier);
      stateMachine.Update(this, delta);
      var velocity = Velocity;
      velocity += _acceleration * (float)delta;
      velocity.Y = Mathf.Clamp(velocity.Y, -Mathf.Inf, FallingVelocity.Y);
      Velocity = velocity;
      CheckFacingDirection();
      MoveAndSlide();
      _acceleration *= Vector2.Zero;
      QueueRedraw();
    }

    public void AddForce(Vector2 force) {
      _acceleration += force;
    }

    public static Vector2 InputDirection =>
      Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

    public static float HorizontalInputAxis =>
      Input.GetAxis("ui_left", "ui_right");

    public bool DirectionJustChanged => Velocity.X * HorizontalInputAxis < 0;
    public Vector2 FacingDirection => _facingDirection;

    private void CheckFacingDirection() {
      if (Velocity.X > 0) {
        _facingDirection = Vector2.Right;
      } else if (Velocity.X < 0) {
        _facingDirection = Vector2.Left;
      }
      _rayWall.TargetPosition = _rayWallTarget * _facingDirection;
    }

    public bool IsNearWall() {
      return _rayWall.IsColliding();
    }

    public void ResetGravityModifier() {
      GravityModifier = _startGravityModifier;
    }

    public override void _Draw() {
      DrawLine(Vector2.Zero, Velocity * 0.33f, Color.Color8(255, 0, 0), 8);
      DrawLine(Vector2.Zero, _rayWall.TargetPosition, Color.Color8(0, 0, 255), 8);
    }
  }
}

