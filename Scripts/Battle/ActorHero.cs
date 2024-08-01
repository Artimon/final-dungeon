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
	public ActorCompDamageNumber _damageNumber;

	[Export]
	public AudioStreamPlayer jumpAudio;

	[Export]
	public Weapon weapon;

	public override void _Ready() {
		_hits = 713f;
		_maxHits = 978f;
		_actionDuration = 5d;
		_actionCooldown = (GD.Randf() + GD.Randf()) / 2d;

		_status.SetHits(_hits, _maxHits);

		animationPlayer.AnimationFinished += OnAnimationFinished;
	}

	public void OnAnimationFinished(StringName name) {
		_action?.animation.OnAnimationFinished(name);
	}

	public override void _OnProcess(double delta) {
		_action?.animation.OnProcess(delta);
	}

	public override bool TryBeginAction(Action action) {
		if (_action != null) {
			return false;
		}

		_action = action;

		return true;
	}

	public override void ApplyDamage(float damage) {
		_hits -= damage;
		_damageNumber.Show(damage);

		_status.SetHits(_hits, _maxHits);

		if (IsDead) {
			return;
		}

		_action = new Action {
			animation = new HitAnimation(this)
		};
		_action.animation.OnEnter();
	}

	public void ActionFinished(bool resetCooldown = true) {
		animatedSprite.Play("default");

		ResetAction(resetCooldown);
	}

	public override void OnActionUpdate() {
		_status.SetAtb(_actionCooldown);
	}

	public override void OnActionReady() {
		ComponentActionMenu.instance.TryShow(this);
	}
}