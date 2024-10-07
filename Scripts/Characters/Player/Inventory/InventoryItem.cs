using Godot;

namespace Antimatter.Scripts.Characters.Player.Inventory
{
  public partial class InventoryItem : Area2D
  {
    public enum Type
    {
      weapon,
      medikit,
      potion
    }
    private Type _type;
    public Type InventoryType { get => _type; }

    public InventoryItem(Type type) {
      _type = type;
      AddToGroup("collectables");
    }

    public virtual string ItemName {
      get => throw new System.NotImplementedException();
    }

    public string GetInventoryType() {
      return _type switch {
        Type.weapon => "weapon",
        Type.medikit => "medikit",
        Type.potion => "potion",
        _ => "unknown",
      };
    }
  }
}
