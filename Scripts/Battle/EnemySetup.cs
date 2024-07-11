using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class EnemySetup : Resource {
	[Export]
	public Texture2D[] textures;
}