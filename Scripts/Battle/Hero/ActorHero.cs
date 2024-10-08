﻿using FinalDungeon.Battle.Config;
using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorHero : ActorBase {
	[Export]
	public string name;

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

	[Export]
	public ActionSetup[] actions;

	public override void _Ready() {
		_hits = 713f;
		_maxHits = 978f;

		_mana = 4;
		_maxMana = 4;

		_actionDuration = 5d;
		_actionCooldown = (GD.Randf() + GD.Randf()) / 2d;

		_status.ShowName(name);
		_status.SetHits(_hits, _maxHits);

		_status.mana.Mana = _mana;
		_status.mana.MaxMana = _maxMana;

		OnManaChanged += () => {
			_status.mana.Mana = _mana;
		};
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

		TryRefreshTargeting();

		if (IsDead) {
			_lockActionTime = true;
			_actionCooldown = 0d;
			_status.SetAtb(_actionCooldown);

			stateMachine.Force("Die");

			ControllerBattle.instance.TryBattleEnd();

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
		ComponentActionMenu.instance.Add(this);
	}
}