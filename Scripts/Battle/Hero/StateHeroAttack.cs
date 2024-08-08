using System.Linq;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateHeroAttack : StateBase {
	public const float AttackDuration = 0.5f;
	public const float JumpDuration = 0.5f;
	public const float JumpHeight = 50;

	public enum AnimationState {
		JumpToTarget,
		Attack,
		JumpBack
	}

	public AnimationState _state;

	[Export]
	public ActorHero _actor;
	public ActorBase _targetActor;
	public Action _action;

	public Vector2 _startPosition;
	public Vector2 _targetPosition;
	public float _elapsedTime;

	public override string StateName => "Attack";

	public override void OnEnter() {
		_actor._lockActionTime = true;

		_action = _actor._action;
		_targetActor = _action.targetActors.FirstOrDefault();

		if (_targetActor == null) {
			_actor.ActionFinished(false);
			_stateMachine.TryEnter("Idle");

			return;
		}

		_startPosition = _actor.display.GlobalPosition;
		_targetPosition = _targetActor.GlobalPosition;

		_state = AnimationState.JumpToTarget;

		_actor.animatedSprite.Play("Jump");
		_actor.jumpAudio.Play();
		_actor.isInvulnerable = true;

		_UpdateActorDirection();
	}

	public override void OnProcess(double delta) {
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

	public override void OnExit() {
		_actor.isInvulnerable = false;
	}

	public void _UpdateActorDirection() {
		var direction = _startPosition - _targetPosition;
		var scaleX = Mathf.Sign(direction.X);

		_actor.display.Scale = new Vector2(scaleX, 1);
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
			_stateMachine.TryEnter("Idle");
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
}