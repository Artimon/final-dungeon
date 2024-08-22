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

		_castTimer.WaitTime = 4f; // @TODO: Get from spell/stats.
		_castTimer.Timeout += OnCastTimerTimeout;
		_castTimer.Start();
	}

	public override void OnExit() {
		_castTimer.Timeout -= OnCastTimerTimeout;
		_castTimer.Stop();
	}

	private void OnCastTimerTimeout() {
		GD.Print("Do spell cast!");

		_actor.ActionFinished(true);
		_stateMachine.TryEnter("Idle");
	}
}