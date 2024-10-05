using Godot;

namespace Antimatter.Scripts.Scenes
{
  public partial class Main : Node2D
  {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      PrintTreePretty();
      GD.Print("Main Scene ready");
    }
  }
}
