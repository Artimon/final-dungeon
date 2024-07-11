using FinalDungeon.Extensions;
using Godot;

namespace FinalDungeon.Battle;

public partial class ActorEnemy : ActorBase {
	[Export]
	public EnemySetup _setup;

	[Export]
	public Sprite2D _sprite;

	[Export]
	public AnimationPlayer _animationPlayer;

	public override void _Ready() {
		_hits = 250;
		_maxHits = 250;
		_speed = 3;
		isEnemy = true;

		_animationPlayer.AnimationFinished += OnDeathAnimationFinished;
	}

	public override bool TryBeginAction() {
		throw new System.NotImplementedException();
	}

	public void UpdateTextureBasedOnHealth() {
		var healthPercentage = (float)_hits / _maxHits;
		var steps = _setup.textures.Length;

		var textureIndex = Mathf.CeilToInt(steps - (healthPercentage * steps));
		textureIndex = Mathf.Clamp(textureIndex, 0, steps - 1);

		_sprite.Texture = _setup.textures[textureIndex];
	}

	public override void ApplyDamage(float damage) {
		_hits -= Mathf.RoundToInt(damage);

		UpdateTextureBasedOnHealth();

		TryDeath();
	}

	public void TryDeath() {
		if (_hits > 0) {
			return;
		}

		ControllerActors.instance.Unregister(this);

		_animationPlayer.Play("Death");
	}

	public void OnDeathAnimationFinished(StringName animationName) {
		switch (animationName) {
			case "Death":
				this.Remove();
				break;
		}
	}

	public override void OnActionReady() {
		GD.Print("Enemy Attack rdy");

		ResetActionTime();
	}
}