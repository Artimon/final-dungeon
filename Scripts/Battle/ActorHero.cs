using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorHero : ActorBase {
	[Export]
	public ElementHeroStatus _status;

	[Export]
	public AnimationPlayer _animationPlayer;

	public override void _Ready() {
		_hits = 713;
		_maxHits = 978;
		_speed = 5;

		_status.SetHits(_hits, _maxHits);
	}

	public override bool TryBeginAction() {
		if (action == null) {
			return false;
		}

		if (action.isUsed) {
			return false;
		}

		action.isUsed = true;

		_animationPlayer.Play("Attack");
		foreach (var actor in action.targetActors) {
			actor.ApplyDamage(150f);
		}

		return true;
	}

	public override void ApplyDamage(float damage) {
		throw new System.NotImplementedException();
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