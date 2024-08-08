using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateHeroHit : StateBase {
	[Export]
	public ActorHero _actor;

	public override string StateName => "Hit";

	public override void OnEnter() {
		_actor._lockActionTime = true;

		_actor.animationPlayer.AnimationFinished += OnAnimationFinished;

		_actor.animatedSprite.Play("Hit");
		_actor.animationPlayer.Play("Hit");
	}

	public override void OnExit() {
		_actor.animationPlayer.AnimationFinished -= OnAnimationFinished;
	}

	public void OnAnimationFinished(StringName _) {
		_actor.ActionFinished(false);
		_stateMachine.TryEnter("Idle");
	}
}