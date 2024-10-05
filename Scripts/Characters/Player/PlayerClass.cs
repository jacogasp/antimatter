using Godot;
using Antimatter.Scripts.Settings;
using System.Runtime.CompilerServices;

namespace Scripts.Characters.Player
{
  public partial class PlayerClass : CharacterBody2D
  {
    [Export] public float GravityModifier { get; set; } = 1.0f;
    [Export] public Vector2 FallingVelocity { get; set; } = new(20f, 1000f);
    [Export] public float AngularSpeed { get; set; } = 1.0f;

    private Vector2 _facingDirection = Vector2.Right;
    private float _startGravityModifier = 1.0f;
    private RayCast2D _frontWallRay;
    private RayCast2D _backWallRay;
    private Vector2 _frontWallRayTarget = Vector2.Zero;
    private Vector2 _backWallRayTarget = Vector2.Zero;
    private Vector2 _acceleration = Vector2.Zero;
    private Vector2 _hookTarget = Vector2.Zero;
    private bool _hookAcquired = false;

    public override void _Ready() {
      _startGravityModifier = GravityModifier;
      _frontWallRay = GetNode<RayCast2D>("FrontWallRay");
      _backWallRay = GetNode<RayCast2D>("BackWallRay");
      _frontWallRayTarget = _frontWallRay.TargetPosition;
      _backWallRayTarget = _backWallRay.TargetPosition;
      GD.Print("Player ready");
    }

    public override void _PhysicsProcess(double delta) {
      AddForce(Vector2.Down * Physics.Gravity * GravityModifier);
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

    public bool DirectionJustChanged => (Velocity.X * HorizontalInputAxis) < 0;
    public Vector2 FacingDirection => _facingDirection;
    public bool HookAcquired => _hookAcquired;

    private void CheckFacingDirection() {
      if (HorizontalInputAxis > 0) {
        _facingDirection = Vector2.Right;
      } else if (HorizontalInputAxis < 0) {
        _facingDirection = Vector2.Left;
      }
      _frontWallRay.TargetPosition = _frontWallRayTarget * _facingDirection;
      _backWallRay.TargetPosition = _backWallRayTarget * _facingDirection;
    }

    public bool IsFrontNearWall() => _frontWallRay.IsColliding();
    public bool IsBackNearWall() => _backWallRay.IsColliding();
    public bool IsNearWall() => IsFrontNearWall() || IsBackNearWall();

    public void ResetGravityModifier() {
      GravityModifier = _startGravityModifier;
    }

    public void OnAreaEntered(Area2D area) {
      GD.Print("entered!");
      _hookTarget = area.GlobalPosition;
      _hookAcquired = true;
    }

    public void OnAreaExited(Area2D area) {
      _hookAcquired = false;
    }

    public override void _Draw() {
      DrawLine(Vector2.Zero, Velocity * 0.33f, Color.Color8(255, 0, 0), 8);
      DrawLine(Vector2.Zero, _frontWallRay.TargetPosition.Normalized() * 100, Color.Color8(0, 0, 255), 8);
      DrawLine(Vector2.Zero, _backWallRay.TargetPosition.Normalized() * 100, Color.Color8(0, 255, 0), 8);
      if (_hookAcquired) {
        DrawLine(Vector2.Zero, _hookTarget - GlobalPosition, Color.Color8(255, 255, 0), 8);
      }
    }
  }
}
