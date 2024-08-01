using Godot;

namespace FinalDungeon.Battle;

public class HitAnimation : IAnimation {
	public ActorHero _actor;

	public HitAnimation(ActorHero actor) {
		_actor = actor;
	}

	public void OnEnter() {
		_actor.animatedSprite.Play("Hit");
		_actor.animationPlayer.Play("Hit");
	}

	public void OnProcess(double delta) {
		_actor._lockActionTime = true;
	}

	public void OnAnimationFinished(StringName name) {
		if (name == "Hit") {
			_actor.ActionFinished(false);
		}
	}
}