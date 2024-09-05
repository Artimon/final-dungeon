using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateHeroCast : StateBase {
	[Export]
	public ActorHero _actor;

	[Export]
	private Timer _castTimer;

	public override string StateName => "Cast";

	public override void OnEnter() {
		_actor.animatedSprite.Play("Cast");

		_castTimer.WaitTime = _actor.Action.setup.castTime;
		_castTimer.Timeout += OnCastTimerTimeout;
		_castTimer.Start();
	}

	public override void OnExit() {
		_castTimer.Timeout -= OnCastTimerTimeout;
		_castTimer.Stop();
	}

	private void OnCastTimerTimeout() {
		_actor.Action.setup.ApplyMechanic(_actor.Action.targetActors);

		_actor.ActionFinished();
		_stateMachine.TryEnter("Idle");
	}
}