using Godot;

namespace FinalDungeon.Battle.Config;

[GlobalClass]
public partial class SingleMultiTargetSelect : TargetSelectBase {
	public bool _isMultiTargeting;
	public ActorBase[] _lastTargets;

	// @TODO Add Enemy/Hero selection option.

	public override void Begin(ActorBase[] lastTargets) {
		_lastTargets = lastTargets;

		_UpdateTargeting();
	}

	public override void OnInputEvent(InputEvent @event) {
		if (@event.IsActionPressed("Target Select Mode")) {
			_isMultiTargeting = !_isMultiTargeting;

			_UpdateTargeting();
		}

		if (_isMultiTargeting) {
			return;
		}

		if (@event.IsActionPressed("East")) {
			_SelectNextTarget(Direction.Right);
		}
		else if (@event.IsActionPressed("West")) {
			_SelectNextTarget(Direction.Left);
		}
	}

	public void _UpdateTargeting() {
		ControllerActors.instance.UntargetAll();

		if (_isMultiTargeting) {
			_SelectAllTargets();
		}
		else {
			_GetTargetEnemy(_lastTargets).Target();
		}
	}
}