using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public abstract partial class ActorBase : Node2D {
	public int _hits;
	public int _maxHits;
	public int _speed;

	public bool isEnemy;

	public bool _lockActionTime;
	public double _actionTime; // 0...1

	public Action action;

	[Export]
	public AnimatedSprite2D _targetMarker;

	public bool IsTargeted => _targetMarker.Visible;

	public bool IsDead => _hits <= 0;

	public override void _EnterTree() {
		_targetMarker.Hide();

		// Unregister not on exit tree but on hits <= 0.
		ControllerActors.instance.Register(this);
	}

	public override void _Process(double delta) {
		if (_lockActionTime) {
			return;
		}

		var speedFactor = 0.5d + _speed * 0.1d;

		_actionTime += speedFactor * delta;
		if (_actionTime > 1d) {
			_actionTime = 1d;
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

	public abstract bool TryBeginAction();

	public abstract void ApplyDamage(float damage);

	public virtual void OnActionUpdate() { }

	public abstract void OnActionReady();

	public void ResetActionTime() {
		_actionTime = 0d;
		_lockActionTime = false;

		OnActionUpdate();
	}
}