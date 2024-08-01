using Godot;

namespace FinalDungeon.Extensions;

public static class PackedSceneExtension {
	public static T Instantiate<T>(this PackedScene packedScene, Node2D parent) where T : Node2D {
		var thing = packedScene.Instantiate<T>();

		parent.AddChild(thing);
		thing.GlobalPosition = parent.GlobalPosition;

		return thing;
	}
}