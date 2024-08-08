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
		_hits = 23f;
		_maxHits = 978f;
		_actionDuration = 5d;
		_actionCooldown = (GD.Randf() + GD.Randf()) / 2d;

		_status.SetHits(_hits, _maxHits);
	}

	public override bool TryBeginAction(Action action) {
		if (_action != null) {
			return false;
		}

		_action = action;
		_action.TryBeginAction(this);

		return true;
	}

	public override void ApplyDamage(float damage) {
		if (isInvulnerable) {
			return;
		}

		_hits -= damage;
		_damageNumber.Show(damage);

		_status.SetHits(_hits, _maxHits);

		if (IsDead) {
			_lockActionTime = true;
			_actionCooldown = 0d;
			_status.SetAtb(_actionCooldown);

			stateMachine.Force("Die");

			return;
		}

		stateMachine.Force("Hit");
	}

	public void ActionFinished(bool resetCooldown = true) {
		ResetAction(resetCooldown);
	}

	public override void OnActionUpdate() {
		_status.SetAtb(_actionCooldown);
	}

	public override void OnActionReady() {
		ComponentActionMenu.instance.TryShow(this);
	}
}