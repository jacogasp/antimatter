using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory.Weapons
{
  public partial class GrenadeLauncher : Weapon
  {
    private string _itemName = "Basic Grenade";
    public override string ItemName => _itemName;

  }
}
