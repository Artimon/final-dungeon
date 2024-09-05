using FinalDungeon.Battle.Config;
using Godot;

namespace FinalDungeon.Battle.Spells;

public partial class FireSpell : Node2D {
	[Export]
	public AnimatedSprite2D animatedSprite;

	public void Begin(ActorBase target, ActionSetup actionSetup, int nthTarget) {
		animatedSprite.Play("default");
		animatedSprite.AnimationFinished += () => {
			target.ApplyDamage(actionSetup.power);

			this.Remove();
		};
	}
}