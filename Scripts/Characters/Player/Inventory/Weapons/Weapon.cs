using Antimatter.Scripts.Weapons;
using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory.Weapons
{
  public partial class Weapon : InventoryItem, IWeapon
  {
    [Export] public float AimingTimeout { get; set; } = 0;

    public virtual bool AimingExpired { get; }

    public Weapon() : base(Type.weapon) {
      AddToGroup("weapons");
      SetCollisionLayerValue(Settings.Physics2D.CollisionLayers.IndexOf("weapons"), true);
    }

    public virtual void Aim() {
      throw new System.NotImplementedException();
    }

    public virtual void Fire() {
      throw new System.NotImplementedException();
    }
  }
}
