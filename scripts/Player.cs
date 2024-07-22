using Algorithms;
using Godot;
using GameSettings;

namespace Characters
{
  namespace Player
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
        stateMachine.CurrentState = new States.Idle();
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

    namespace States
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

      class Idle : PlayerState
      {
        private float t = 0;

        public override void Enter(Player player) {
          player.ResetGravityModifier();
          GD.Print("idle");
        }

        public override IState<Player> HandleInput(Player player) {
          if (Input.IsActionJustReleased("jump") && player.IsOnFloor()) {
            return new StandingJump();
          }
          if (player.Velocity.Y > 0) {
            return new Falling();
          }
          if (Player.InputDirection != Vector2.Zero) {
            return new Running();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          var speedX = Mathf.Abs(velocity.X);
          var k = Mathf.Clamp(t / player.HorizontalDump, 0, 1);
          velocity.X = Mathf.Lerp(Mathf.Abs(speedX), 0, k);
          velocity.X *= player.FacingDirection.X;
          player.Velocity = velocity;
          t += delta;
        }
      }

      class Falling : PlayerState
      {
        public override void Enter(Player player) {
          GD.Print("falling");
        }

        public override IState<Player> HandleInput(Player player) {
          if (player.IsOnFloorOnly()) {
            return new Idle();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          velocity.X = player.Speed * Player.HorizontalInputAxis;
          player.Velocity = velocity;
        }
      }

      class Running : PlayerState
      {
        public override void Enter(Player player) {
          GD.Print("run");
        }

        public override IState<Player> HandleInput(Player player) {
          if (Input.IsActionJustReleased("jump")) {
            if (player.IsOnWall()) {
              return new StandingJump();
            }
            return new RunningJump();
          }
          if (player.Velocity.Y > 0) {
            return new Falling();
          }
          if (Player.InputDirection == Vector2.Zero) {
            return new Idle();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          velocity.X = Player.HorizontalInputAxis * player.Speed;
          player.Velocity = velocity;
        }
      }

      class StandingJump : PlayerState
      {
        public override void Enter(Player player) {
          var velocity = player.Velocity;
          velocity.Y = -player.StandingJumpVelocity.Y;
          player.Velocity = velocity;
          GD.Print("standing jump");
        }

        public override IState<Player> HandleInput(Player player) {
          if (player.IsOnFloor()) {
            return new Idle();
          }
          if (player.IsNearWall()) {
            return new OnWall();
          }
          if (Input.IsActionJustPressed("jump")) {
            return new StandingAirJump();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          velocity.X = player.StandingJumpVelocity.X * Player.HorizontalInputAxis;
          player.Velocity = velocity;
        }
      }

      class RunningJump : PlayerState
      {
        private Vector2 entryAbsVelocity;
        private bool directionChanged = false;
        private float t = 0;

        public override void Enter(Player player) {
          var velocity = player.Velocity;
          velocity.Y = -player.JumpVelocity.Y;
          player.Velocity = velocity;
          entryAbsVelocity = velocity.Abs();
          GD.Print("running jump");
        }

        public override IState<Player> HandleInput(Player player) {
          if (player.IsOnFloorOnly()) {
            return new Idle();
          }
          if (player.IsNearWall()) {
            return new OnWall();
          }
          if (Input.IsActionJustPressed("jump")) {
            return new AirJump();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          if (player.DirectionJustChanged) {
            directionChanged = true;
          }
          if (directionChanged) {
            velocity.X = player.FallingVelocity.X * Player.HorizontalInputAxis;
          } else if (Mathf.Abs(Player.HorizontalInputAxis) < float.Epsilon) {
            var k = Mathf.Clamp(t / player.JumpHorizontalDump, 0, 1);
            velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
            velocity.X *= player.FacingDirection.X;
            t += delta;
          } else {
            velocity.X = player.Speed * Player.HorizontalInputAxis;
          }
          player.Velocity = velocity;
        }
      }

      class StandingAirJump : PlayerState
      {
        public override void Enter(Player player) {
          var velocity = player.Velocity;
          velocity.Y = -player.AirJumpVelocity.Y;
          player.Velocity = velocity;
          GD.Print("standing air jump");
        }

        public override IState<Player> HandleInput(Player player) {
          if (player.IsOnFloor()) {
            return new Idle();
          }
          if (player.IsNearWall()) {
            return new OnWall();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          velocity.X = player.AirJumpVelocity.X * Player.HorizontalInputAxis;
          player.Velocity = velocity;
        }
      }
      class AirJump : PlayerState
      {
        private Vector2 entryAbsVelocity;
        private float t = 0;

        public override void Enter(Player player) {
          var velocity = player.Velocity;
          if (velocity.Y > -player.AirJumpVelocity.Y) {
            velocity.Y = -player.AirJumpVelocity.Y;
          }
          player.Velocity = velocity;
          entryAbsVelocity = velocity.Abs();
          GD.Print("air jump");
        }

        public override IState<Player> HandleInput(Player player) {
          if (player.IsOnFloor()) {
            return new Idle();
          }
          if (player.IsNearWall()) {
            return new OnWall();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          if (Mathf.Abs(Player.HorizontalInputAxis) < float.Epsilon) {
            var velocity = player.Velocity;
            var k = Mathf.Clamp(t / player.JumpHorizontalDump, 0, 1);
            velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
            velocity.X *= player.FacingDirection.X;
            t += delta;
            player.Velocity = velocity;
          }
        }
      }

      class OnWall : PlayerState
      {
        public override void Enter(Player player) {
          GD.Print("on wall");
        }
        public override IState<Player> HandleInput(Player player) {
          if (Input.IsActionJustReleased("jump")) {
            return new WallJump();
          }

          if (!player.IsNearWall()) {
            return new Falling();
          }

          if (player.IsOnFloor()) {
            return new Idle();
          }
          return this;
        }
        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          velocity.Y *= player.WallDumping;
          player.Velocity = velocity;

        }
      }
      class WallJump : PlayerState
      {
        public override void Enter(Player player) {
          GD.Print("wall jump");
          var velocity = player.WallJumpVelocity;
          velocity.X *= Player.HorizontalInputAxis;
          velocity.Y *= -1;
          player.Velocity = velocity;
        }

        public override IState<Player> HandleInput(Player player) {
          if (player.IsNearWall()) {
            return new OnWall();
          }
          if (player.IsOnFloor()) {
            return new Idle();
          }
          if (Input.IsActionJustPressed("jump")) {
            return new AirJump();
          }
          return this;
        }

        public override void Update(Player player, float delta) {
          var velocity = player.Velocity;
          velocity.X = player.WallJumpVelocity.X * Player.HorizontalInputAxis;
          player.Velocity = velocity;
        }
      }
    }
  }
}
