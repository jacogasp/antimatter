using Godot;
using System;
using System.Collections.Generic;

namespace Antimatter.Scripts.Settings
{
  public static class Physics2D
  {
    public static readonly float Gravity = ProjectSettings
          .GetSetting("physics/2d/default_gravity")
          .AsSingle();

    public static List<string> CollisionLayers {
      get {
        List<string> layers = new();
        for (var i = 0; i < 32; ++i) {
          var layer = ProjectSettings.GetSetting("layer_names/2d_physics/layer_" + (i + 1)).AsString();
          layers.Add(layer);
        }
        return layers;
      }
    }
  }
}
