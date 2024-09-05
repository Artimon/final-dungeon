using Godot;

namespace FinalDungeon.Battle.Config;

[GlobalClass]
public partial class SingleTargetSelect : TargetSelectBase {
	// @TODO Add Enemy/Hero selection option.

	public override void Begin(ActorBase[] lastTargets) {
		ControllerActors.instance.UntargetAll();

		_GetTargetEnemy(lastTargets).Target();
	}

	public override void OnInputEvent(InputEvent @event) {
		if (@event.IsActionPressed("East")) {
			_SelectNextTarget(Direction.Right);
		}
		else if (@event.IsActionPressed("West")) {
			_SelectNextTarget(Direction.Left);
		}
	}
}