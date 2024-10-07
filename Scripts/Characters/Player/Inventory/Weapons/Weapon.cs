using Antimatter.Scripts.Weapons;
using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory.Weapons
{
  public partial class Weapon : InventoryItem, IWeapon
  {
    public Weapon() : base(Type.weapon) {
      AddToGroup("weapons");
      SetCollisionLayerValue(Settings.Physics2D.CollisionLayers.IndexOf("weapons"), true);
    }

    public void Attack() {
      throw new System.NotImplementedException();
    }
  }
}
