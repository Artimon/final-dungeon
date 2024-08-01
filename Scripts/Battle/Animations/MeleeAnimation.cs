using Godot;

namespace FinalDungeon.Battle;

public class MeleeAnimation : IAnimation {
	public const float AttackDuration = 0.5f;
	public const float JumpDuration = 0.5f;
	public const float JumpHeight = 50;

	public enum AnimationState {
		JumpToTarget,
		Attack,
		JumpBack
	}

	public AnimationState _state;

	public ActorHero _actor;
	public ActorBase _targetActor;
	public Action _action;

	public Vector2 _startPosition;
	public Vector2 _targetPosition;
	public float _elapsedTime;

	public MeleeAnimation(ActorHero actor, ActorBase targetActor, Action action) {
		_actor = actor;
		_targetActor = targetActor;
		_action = action;

		_startPosition = _actor.display.GlobalPosition;
		_targetPosition = _targetActor.GlobalPosition;

		_state = AnimationState.JumpToTarget;
		_actor.animatedSprite.Play("Jump");
		_actor.jumpAudio.Play();

		_UpdateActorDirection();
	}

	public void OnEnter() { }

	public void _UpdateActorDirection() {
		var direction = _startPosition - _targetPosition;
		var scaleX = Mathf.Sign(direction.X);

		_actor.display.Scale = new Vector2(scaleX, 1);
	}

	public void OnProcess(double delta) {
		switch (_state) {
			case AnimationState.JumpToTarget:
				_ProcessJumpToTarget(delta);
				break;

			case AnimationState.Attack:
				_ProcessAttack(delta);
				break;

			case AnimationState.JumpBack:
				_ProcessJumpToTarget(delta);
				break;
		}
	}

	public void _ProcessJumpToTarget(double delta) {
		_elapsedTime = Mathf.Min(1f, _elapsedTime + (float)delta / JumpDuration);

		var currentPosition = _startPosition.Lerp(_targetPosition, _elapsedTime);
		currentPosition.Y -= Mathf.Sin(_elapsedTime * Mathf.Pi) * JumpHeight;

		_actor.display.GlobalPosition = currentPosition;

		if (_elapsedTime < 1f) {
			return;
		}

		_elapsedTime = 0f;

		if (_state == AnimationState.JumpBack) {
			_actor.ActionFinished();

			return;
		}

		_state = AnimationState.Attack;
		_actor.animatedSprite.Play("Slash");
		_actor.weapon.Play();

		_action.DamageTargets(150f);
	}

	public void _ProcessAttack(double delta) {
		_elapsedTime = Mathf.Min(1f, _elapsedTime + (float)delta / AttackDuration);

		if (_elapsedTime < 1f) {
			return;
		}

		_elapsedTime = 0f;

		_state = AnimationState.JumpBack;
		_actor.animatedSprite.Play("Jump");
		_actor.jumpAudio.Play();

		// Switch positions for jump back.
		(_startPosition, _targetPosition) = (_targetPosition, _startPosition);

		_UpdateActorDirection();
	}

	public void OnAnimationFinished(StringName _) { }
}