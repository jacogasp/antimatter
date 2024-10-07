using Godot;
using System;

namespace Antimatter.Scripts.Characters.Player.Inventory.Potions
{
  public partial class BasicPotion : Potion
  {
    private string _name = "Basic Potion";

    public override string ItemName => _name;
  }
}
