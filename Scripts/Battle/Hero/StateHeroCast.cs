using FinalDungeon.Battle.Effects;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateHeroCast : StateBase {
	[Export]
	public ActorHero _actor;

	[Export]
	private Timer _castTimer;

	[Export]
	public SpellCastEffect _spellCastEffect;

	public override string StateName => "Cast";

	public override void OnEnter() {
		_actor.animatedSprite.Play("Cast");

		_castTimer.WaitTime = _actor.Action.setup.castTime;
		_castTimer.Timeout += OnCastTimerTimeout;
		_castTimer.Start();

		_spellCastEffect.Playing = true;
	}

	public override void OnExit() {
		_castTimer.Timeout -= OnCastTimerTimeout;
		_castTimer.Stop();

		_spellCastEffect.Playing = false;
	}

	private void OnCastTimerTimeout() {
		_actor.Action.setup.ApplyMechanic(_actor.Action.targetActors);

		_actor.ActionFinished();
		_stateMachine.TryEnter("Idle");
	}
}