using Algorithms;
using Godot;
using GameSettings;
using System;
using System.Runtime.InteropServices;

namespace Characters
{
  namespace Player
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
      public Vector2 StandingJumpVelocity { get; set; } = new(200, 800);

      [Export]
      public Vector2 AirJumpVelocity { get; set; } = new Vector2(200, 300);

      [Export]
      public float JumpHorizontalDump { get; set; } = 1f;

      [Export]
      public Vector2 FallingVelocity { get; set; } = new(20f, 1000f);

      [Export]
      public float AngularSpeed { get; set; } = 1.0f;

      private readonly StateMachine<Player> stateMachine = new();
      private Vector2 _acceleration = Vector2.Zero;

      public override void _Ready() {
        stateMachine.CurrentState = new States.Idle();
        GD.Print("Player ready");
      }

      public override void _PhysicsProcess(double delta) {
        stateMachine.Update(this, delta);
        AddForce(Vector2.Down * Physics.Gravity);
        var velocity = Velocity;
        velocity += _acceleration * (float)delta;
        velocity.Y = Mathf.Clamp(velocity.Y, -Mathf.Inf, FallingVelocity.Y);
        Velocity = velocity;
        MoveAndSlide();
        _acceleration *= Vector2.Zero;
      }

      public void AddForce(Vector2 force) {
        _acceleration += force;
      }

      public static Vector2 InputDirection =>
        Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

      public static float HorizontalInputAxis =>
        Input.GetAxis("ui_left", "ui_right");

      public bool DirectionJustChanged => Velocity.X * HorizontalInputAxis < 0;
      public Vector2 FacingDirection => Velocity.X < 0 ? Vector2.Left : Vector2.Right;
    }

    namespace States
    {
      class Idle : State<Player>
      {
        private float t = 0;

        public override void Enter(Player gameObject) {
          GD.Print("idle");
        }

        public override State<Player> HandleInput(Player gameObject) {
          if (Input.IsActionJustPressed("jump") && gameObject.IsOnFloor()) {
            return new StandingJump();
          }
          if (gameObject.Velocity.Y > 0) {
            return new Falling();
          }
          if (Player.InputDirection != Vector2.Zero) {
            return new Running();
          }
          return this;
        }

        public override void Update(Player gameObject, float delta) {
          var velocity = gameObject.Velocity;
          var speedX = Mathf.Abs(velocity.X);
          var k = Mathf.Clamp(t / gameObject.HorizontalDump, 0, 1);
          velocity.X = Mathf.Lerp(Mathf.Abs(speedX), 0, k);
          velocity.X *= gameObject.FacingDirection.X;
          gameObject.Velocity = velocity;
          t += delta;
        }
      }

      class Falling : State<Player>
      {
        public override void Enter(Player gameObject) {
          GD.Print("falling");
        }

        public override State<Player> HandleInput(Player gameObject) {
          if (gameObject.IsOnFloor()) {
            return new Idle();
          }
          return this;
        }

        public override void Update(Player gameObject, float delta) {
          var velocity = gameObject.Velocity;
          velocity.X = gameObject.FallingVelocity.X * Player.HorizontalInputAxis;
          gameObject.Velocity = velocity;
        }
      }

      class Running : State<Player>
      {
        public override void Enter(Player gameObject) {
          GD.Print("run");
        }

        public override State<Player> HandleInput(Player gameObject) {
          if (Input.IsActionJustPressed("jump") && gameObject.IsOnFloor()) {
            return new RunningJump();
          }
          if (gameObject.Velocity.Y > 0) {
            return new Falling();
          }
          if (Player.InputDirection == Vector2.Zero) {
            return new Idle();
          }
          return this;
        }

        public override void Update(Player gameObject, float delta) {
          var velocity = gameObject.Velocity;
          velocity.X = Player.HorizontalInputAxis * gameObject.Speed;
          gameObject.Velocity = velocity;
        }
      }

      class StandingJump : State<Player>
      {
        public override void Enter(Player gameObject) {
          var velocity = gameObject.Velocity;
          velocity.Y = -gameObject.StandingJumpVelocity.Y;
          gameObject.Velocity = velocity;
          GD.Print("standing jump");
        }

        public override State<Player> HandleInput(Player gameObject) {
          if (gameObject.IsOnFloor()) {
            return new Idle();
          }
          if (Input.IsActionJustPressed("jump")) {
            return new AirJump();
          }
          return this;
        }

        public override void Update(Player gameObject, float delta) {
          var velocity = gameObject.Velocity;
          velocity.X = gameObject.StandingJumpVelocity.X * Player.HorizontalInputAxis;
          gameObject.Velocity = velocity;
        }
      }

      class RunningJump : State<Player>
      {
        private Vector2 entryAbsVelocity;
        private bool directionChanged = false;
        private float t = 0;

        public override void Enter(Player gameObject) {
          var velocity = gameObject.Velocity;
          velocity.Y = -gameObject.JumpVelocity.Y;
          gameObject.Velocity = velocity;
          entryAbsVelocity = velocity.Abs();
          GD.Print("running jump");
        }

        public override State<Player> HandleInput(Player gameObject) {
          if (gameObject.IsOnFloor()) {
            return new Idle();
          }
          if (Input.IsActionJustPressed("jump")) {
            return new AirJump();
          }
          return this;
        }

        public override void Update(Player gameObject, float delta) {
          var velocity = gameObject.Velocity;
          if (gameObject.DirectionJustChanged) {
            directionChanged = true;
          }
          if (directionChanged) {
            velocity.X = gameObject.FallingVelocity.X * Player.HorizontalInputAxis;
          } else if (Mathf.Abs(Player.HorizontalInputAxis) == 0) {
            var k = Mathf.Clamp(t / gameObject.JumpHorizontalDump, 0, 1);
            velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
            velocity.X *= gameObject.FacingDirection.X;
            t += delta;
          } else {
            velocity.X = gameObject.FallingVelocity.X * Player.HorizontalInputAxis;
          }
          gameObject.Velocity = velocity;
        }
      }

      class AirJump : State<Player>
      {
        private Vector2 entryAbsVelocity;
        private float t = 0;

        public override void Enter(Player gameObject) {
          var velocity = gameObject.Velocity;
          if (velocity.Y > -gameObject.AirJumpVelocity.Y) {
            velocity.Y = -gameObject.AirJumpVelocity.Y;
          }
          gameObject.Velocity = velocity;
          entryAbsVelocity = velocity.Abs();
          GD.Print("air jump");
        }

        public override State<Player> HandleInput(Player gameObject) {
          if (gameObject.IsOnFloor()) {
            return new Idle();
          }
          return this;
        }

        public override void Update(Player gameObject, float delta) {
          if (Mathf.Abs(Player.HorizontalInputAxis) == 0) {
            var velocity = gameObject.Velocity;
            var k = Mathf.Clamp(t / gameObject.JumpHorizontalDump, 0, 1);
            velocity.X = Mathf.Lerp(Mathf.Abs(entryAbsVelocity.X), 0, k);
            velocity.X *= gameObject.FacingDirection.X;
            t += delta;
            gameObject.Velocity = velocity;
          }
        }
      }
    }
  }
}
