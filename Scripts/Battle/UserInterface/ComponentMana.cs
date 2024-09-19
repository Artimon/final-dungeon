using Godot;

namespace FinalDungeon.Battle.UserInterface;

[GlobalClass]
public partial class ComponentMana : Control {
	[Export]
	public ElementManaIcon[] _icons;

	public int MaxMana {
		set {
			for (var i = 0; i < _icons.Length; i++) {
				_icons[i].Visible = i < value;
			}
		}
	}

	public int Mana {
		set {
			for (var i = 0; i < _icons.Length; i++) {
				_icons[i].Filled = i < value;
			}
		}
	}
}