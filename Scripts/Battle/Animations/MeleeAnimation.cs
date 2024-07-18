using Godot;

namespace FinalDungeon.Battle;

public class MeleeAnimation : IAnimation {
	public const float Duration = 0.5f;
	public const float JumpHeight = 50;

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

		// (_startPosition, _targetPosition) = (_targetPosition, _startPosition);
	}

	public void Process(double delta) {
		_elapsedTime = Mathf.Min(1f, _elapsedTime + (float)delta / Duration);

		var currentPosition = _startPosition.Lerp(_targetPosition, _elapsedTime);
		currentPosition.Y -= Mathf.Sin(_elapsedTime * Mathf.Pi) * JumpHeight;

		_actor.display.GlobalPosition = currentPosition;

		if (_elapsedTime >= 1f) {
			var animationName = new StringName("Jump");

			_actor.OnAttackAnimationFinished(animationName);
		}
	}
}