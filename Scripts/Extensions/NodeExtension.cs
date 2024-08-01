using Godot;

namespace FinalDungeon.Extensions;

public static class NodeExtension {
	public static void Remove(this Node node) {
#if TOOLS
		var hasParent = node.GetParent() != null;
		if (!hasParent) {
			GD.PrintErr($"Trying to remove a node that was already removed: {node.Name}");

			return;
		}
#endif

		/*
		 * In case this creates a null reference exception, the method has been
		 * called on a node that was already removed from the scene tree.
		 */
		node.GetParent().RemoveChild(node);
		node.QueueFree();
	}
}