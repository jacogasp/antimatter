using Godot;
using System;

namespace Antimatter.Scripts.Characters.Player.Inventory.Medikits
{
  public partial class BasicMedikit : Medikit
  {
    private string _name = "Basic Medikit";
    public override string ItemName => _name;
  }
}
