using Godot;

namespace FinalDungeon.Battle.UserInterface;

[GlobalClass]
public partial class ElementManaIcon : Control {
	[Export]
	public TextureRect _icon;

	public bool Filled {
		get => _icon.Visible;
		set => _icon.Visible = value;
	}
}