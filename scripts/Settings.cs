using Godot;

namespace GameSettings
{
  public static class Physics
  {
    public static readonly float Gravity = ProjectSettings
          .GetSetting("physics/2d/default_gravity")
          .AsSingle();

  }
}
