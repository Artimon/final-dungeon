using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public abstract partial class ActorBase : Node2D {
	public float _hits;
	public float _maxHits;

	public bool isEnemy;
	public bool isInvulnerable;

	public bool _lockActionTime;

	public double _actionDuration;
	public double _actionCooldown; // 0...1

	[Export]
	public StateMachine stateMachine;

	/**
	 * The current action that the actor is performing.
	 * Is reset to null when the action is finished.
	 */
	public Action _action;

	public Action Action => _action;

	[Export]
	public AnimatedSprite2D _targetMarker;

	public bool IsReady => _action == null && _actionCooldown >= 1d;

	public bool IsActionTimeLocked => _lockActionTime || _action != null;

	public bool IsTargeted => _targetMarker.Visible;

	public bool CanBeTargeted => !IsDead;

	public bool IsDead => _hits <= 0;

	public override void _EnterTree() {
		_targetMarker.Hide();

		// Unregister not on exit tree but on hits <= 0.
		ControllerActors.instance.Register(this);
	}

	public virtual void _OnProcess(double delta) { }

	public override void _Process(double delta) {
		_OnProcess(delta);

		if (IsActionTimeLocked) {
			return;
		}

		var speedFactor = 1f / _actionDuration;

		_actionCooldown += speedFactor * delta;
		if (_actionCooldown > 1d) {
			_actionCooldown = 1d;
			_lockActionTime = true;

			OnActionUpdate();
			OnActionReady();
		}

		OnActionUpdate();
	}

	public void Target() {
		_targetMarker.Show();
	}

	public void Untarget() {
		_targetMarker.Hide();
	}

	public bool TryRefreshTargeting() {
		if (!IsTargeted) {
			return false;
		}

		if (CanBeTargeted) {
			return false;
		}

		Untarget();

		ComponentActionMenu.instance.RefreshTargeting();

		return true;
	}

	public abstract bool TryBeginAction(Action action);

	public abstract void ApplyDamage(float damage);

	public virtual void OnActionUpdate() { }

	public abstract void OnActionReady();

	public void ResetAction(bool resetCooldown = true) {
		_action = null;
		_lockActionTime = false;

		if (resetCooldown) {
			_actionCooldown = 0d;
		}

		OnActionUpdate();
	}
}