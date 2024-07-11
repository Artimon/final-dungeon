using Godot;

namespace FinalDungeon.Extensions;

public static class NodeExtension {
	public static void Remove(this Node2D node) {
		node.GetParent().RemoveChild(node);
		node.QueueFree();
	}
}