using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorHero : ActorBase {
	[Export]
	public Node2D display;

	[Export]
	public ElementHeroStatus _status;

	[Export]
	public AnimationPlayer animationPlayer;

	[Export]
	public AnimatedSprite2D animatedSprite;

	[Export]
	public AudioStreamPlayer jumpAudio;

	[Export]
	public Weapon weapon;

	public override void _Ready() {
		_hits = 713;
		_maxHits = 978;
		_actionDuration = 5d;
		_actionCooldown = (GD.Randf() + GD.Randf()) / 2d;

		_status.SetHits(_hits, _maxHits);
	}

	public override void _OnProcess(double delta) {
		_action?.animation.Process(delta);
	}

	public override bool TryBeginAction(Action action) {
		if (_action != null) {
			return false;
		}

		_action = action;

		return true;
	}

	public override void ApplyDamage(float damage) {
		throw new System.NotImplementedException();
	}

	public void ActionFinished() {
		animatedSprite.Play("default");

		ResetAction();
	}

	public override void OnActionUpdate() {
		_status.SetAtb(_actionCooldown);
	}

	public override void OnActionReady() {
		ComponentActionMenu.instance.TryShow(this);
	}
}