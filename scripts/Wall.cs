using Godot;

namespace Environment
{
  [Tool]
  public partial class Wall : StaticBody2D
  {

    private CollisionShape2D _collisionShape2D;
    private RectangleShape2D _rectangleShape2D;
    private MeshInstance2D _meshInstance2D;
    private QuadMesh _quadMesh;

    public override void _Ready() {
      _collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
      _rectangleShape2D = (RectangleShape2D)_collisionShape2D.Shape.Duplicate();
      _collisionShape2D.Shape = _rectangleShape2D; // individual edit workaround
      _rectangleShape2D.Changed += UpdateMesh;
      _quadMesh = new();
      _meshInstance2D = new() {
        Name = "MeshInstance2D",
        Mesh = _quadMesh,
      };
      UpdateMesh();
      AddChild(_meshInstance2D);
    }

    private void UpdateMesh() {
      _meshInstance2D.Scale = _rectangleShape2D.Size;
      _meshInstance2D.Position = _collisionShape2D.Position;
    }
  }
}
