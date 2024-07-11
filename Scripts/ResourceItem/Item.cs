using Godot;

namespace FinalDungeon.ResourceItem;

[Tool] //<-- with this attribute ANY CODE / LOGIC of the class also runs in the editor (incl. _ready(), _process(), _physicsProcess() ...)
[GlobalClass]
public partial class Item : Resource {
	[Export]
	public string ItemName;

	[Export]
	public int Gold;

	private ItemsList _itemsList;

	[Export]
	public ItemsList ItemsList {
		get => _itemsList;
		set {
			//Due to the [Tool] attribute, this code runs the moment you change the value in Godot inspector

			//(1) Store the previous list
			var previousItemsList = _itemsList;
			//(2) Set the new list
			_itemsList = value;

			//(3) Remove this item from the previous list
			if (previousItemsList != null)
				previousItemsList.AllItems.Remove(this);
			//(4) Add this item to the new list
			if (_itemsList != null)
				_itemsList.AllItems.Add(this);
		}
	}
}