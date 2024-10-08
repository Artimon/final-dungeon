﻿using FinalDungeon.Extensions;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorEnemy : ActorBase {
	[Export]
	public EnemySetup _setup;

	[Export]
	public Sprite2D _sprite;

	[Export]
	public AnimationPlayer _animationPlayer;

	[Export]
	public ActorCompDamageNumber _damageNumber;

	[Export]
	public ShaderMaterial _actionMaterial;

	[Export]
	public ShaderMaterial _deathMaterial;

	public override void _Ready() {
		_hits = 250f;
		_maxHits = 250f;
		_actionDuration = 10d;
		_actionCooldown = (GD.Randf() + GD.Randf()) / 2d;
		GD.Print($"Initial action cooldown: {_actionCooldown}");

		isEnemy = true;

		_animationPlayer.AnimationFinished += OnAnimationFinished;
	}

	public override bool TryBeginAction(Action action) {
		_sprite.Material = _actionMaterial;
		_animationPlayer.Play("Action");
		// @TODO During action the enemy can be interrupted and the atb has to restart.

		return true;
	}

	public void UpdateTextureBasedOnHealth() {
		var healthPercentage = (float)_hits / _maxHits;
		var steps = _setup.textures.Length;

		var textureIndex = Mathf.CeilToInt(steps - (healthPercentage * steps));
		textureIndex = Mathf.Clamp(textureIndex, 0, steps - 1);

		_sprite.Texture = _setup.textures[textureIndex];
	}

	public override void ApplyDamage(float damage) {
		_hits -= damage;
		_damageNumber.Show(damage);

		UpdateTextureBasedOnHealth();
		TryRefreshTargeting();

		var isDead = TryDeath();
		if (!isDead) {
			_lockActionTime = true;
			_animationPlayer.Play("Hit");
		}
	}

	public bool TryDeath() {
		if (_hits > 0) {
			return false;
		}

		ControllerActors.instance.Unregister(this);

		SetProcess(false);

		_sprite.Material = _deathMaterial;
		_animationPlayer.Play("Death");

		ControllerBattle.instance.TryBattleEnd();

		return true;
	}

	private void Attack() {
		// @TODO Determine the hero that is currently not performing an action at the beginning of the action.
		var targetHero = ControllerActors.instance.Heroes.GetRandomElement();

		var action = new Action {
			type = Action.ActionTypes.Attack,
			targetActors = new[] { targetHero }
		};

		action.DamageTargets(5);
	}

	public void OnAnimationFinished(StringName animationName) {
		switch (animationName) {
			case "Action":
				Attack();
				ResetAction();
				break;

			case "Hit":
				_lockActionTime = false;
				break;

			case "Death":
				this.Remove();
				break;
		}
	}

	public override void OnActionReady() {
		_sprite.Material = null;

		TryBeginAction(null);
	}
}