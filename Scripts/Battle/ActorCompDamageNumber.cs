using FinalDungeon.Battle.Effects;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorCompDamageNumber : Node2D {
	[Export]
	public PackedScene _effectDamageNumberPrefab;

	public void Show(float damage) {
		var displayDamage = Mathf.RoundToInt(damage);

		var effect = _effectDamageNumberPrefab.Instantiate<EffectDamageNumber>(this);

		effect.SetNumber(displayDamage);
		effect.GlobalPosition += new Vector2(
			GD.RandRange(-5, 5),
			GD.RandRange(-5, 5)
		);;
	}
}