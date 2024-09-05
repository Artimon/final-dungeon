using FinalDungeon.Battle.Config;
using Godot;

namespace FinalDungeon.Battle.Spells;

// @TODO Move some stuff to SpellBase.
public partial class FireSpell : Node2D {
	public const float MultiCastDelay = 0.15f;

	[Export]
	public Timer _timer;

	[Export]
	public AnimatedSprite2D _animatedSprite;

	public void Begin(ActorBase target, ActionSetup actionSetup, int nthTarget) {
		_animatedSprite.Visible = false;

		_timer.WaitTime = Mathf.Max(0.001f, MultiCastDelay * nthTarget);
		_timer.Timeout += () => {
			_animatedSprite.Visible = true;
			_animatedSprite.Play("default");
			_animatedSprite.AnimationFinished += () => {
				target.ApplyDamage(actionSetup.power);

				this.Remove();
			};
		};

		_timer.Start();
	}
}