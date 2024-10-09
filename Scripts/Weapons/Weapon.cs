using Godot;

namespace Antimatter.Scripts.Weapons
{
  public interface IWeapon
  {
    public abstract void Fire();
    public abstract void Aim(Vector2 target);
  }
}
