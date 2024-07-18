using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorHero : ActorBase {
	[Export]
	public Node2D display;

	[Export]
	public ElementHeroStatus _status;

	[Export]
	public AnimationPlayer _animationPlayer;

	public override void _Ready() {
		_hits = 713;
		_maxHits = 978;
		_speed = 5;

		_status.SetHits(_hits, _maxHits);
		_animationPlayer.AnimationFinished += OnAttackAnimationFinished;
	}

	public override void _OnProcess(double delta) {
		_action?.animation.Process(delta);
	}

	public override bool TryBeginAction(Action action) {
		if (_action != null) {
			return false;
		}

		_action = action;

		// _animationPlayer.Play("Attack");

		return true;
	}

	public override void ApplyDamage(float damage) {
		throw new System.NotImplementedException();
	}

	public void OnAttackAnimationFinished(StringName animationName) {
		switch (animationName) {
			case "Jump":
				foreach (var actor in _action.targetActors) {
					actor.ApplyDamage(150f);
				}

				ResetActionTime();
				break;
		}
	}

	public override void OnActionUpdate() {
		_status.SetAtb(_actionTime);
	}

	public override void OnActionReady() {
		GD.Print("Player Attack rdy");

		// ResetActionTime();

		ComponentActionMenu.instance.TryShow(this);
	}
}