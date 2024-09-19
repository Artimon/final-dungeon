using FinalDungeon.Battle.Spells;
using Godot;

namespace FinalDungeon.Battle.Config;

[GlobalClass]
public partial class ActionSetup : Resource {
	[Export]
	public PackedScene singleTargetEffect;

	[Export]
	public float power;

	[Export]
	public int requiredMana;

	[Export]
	public float castTime;

	[Export]
	public TargetSelectBase targetSelect;

	public void ApplyMechanic(ActorBase[] targetActors) {
		var nthTarget = 0;

		foreach (var targetActor in targetActors) {
			// @TODO: Create spell at target slot, not target actor.
			singleTargetEffect.Instantiate<FireSpell>(targetActor).Begin(targetActor, this, nthTarget);

			nthTarget += 1;
		}
	}
}