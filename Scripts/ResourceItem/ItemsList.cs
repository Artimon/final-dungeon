using Godot;
using Godot.Collections;

namespace FinalDungeon.ResourceItem;

[Tool]
[GlobalClass]
public partial class ItemsList : Resource {
	[Export]
	public Array<Item> AllItems = new();
}