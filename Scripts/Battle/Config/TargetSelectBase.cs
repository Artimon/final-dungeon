using System.Linq;
using Godot;

namespace FinalDungeon.Battle.Config;

public abstract partial class TargetSelectBase : Resource {

	public enum Direction {
		Left,
		Right,
		// Future directions can be added here, e.g., Up, Down
	}

	public abstract void Begin(ActorBase[] lastTargets);

	public abstract void OnInputEvent(InputEvent @event);

	public ActorBase _GetTargetEnemy(ActorBase[] targets) {
		if (targets.Length > 0) {
			var enemy = targets[0];

			if (enemy.CanBeTargeted) {
				return enemy;
			}
		}

		return ControllerActors.instance.Enemies.FirstOrDefault(actor => actor.CanBeTargeted);
	}

	public void _SelectAllTargets() {
		ControllerActors.instance.TargetAllEnemies();
	}

	public void _SelectNextTarget(Direction direction) {
		var enemies = ControllerActors.instance.Enemies.ToList();
		if (enemies.Count == 0) {
			return;
		}

		var currentTarget =
			enemies.FirstOrDefault(actor => actor.IsTargeted) ??
			enemies.First();

		var currentPosition = currentTarget.Position;

		var nextTarget = (ActorBase)null;
		var closestDistance = float.MaxValue;

		foreach (var enemy in enemies) {
			var isDesiredDirection = direction switch {
				Direction.Right => enemy.Position.X > currentPosition.X,
				Direction.Left => enemy.Position.X < currentPosition.X,
				_ => false
			};

			if (!isDesiredDirection) {
				continue;
			}

			var distance = currentPosition.DistanceTo(enemy.Position);
			if (distance > closestDistance) {
				continue;
			}

			closestDistance = distance;
			nextTarget = enemy;
		}

		if (nextTarget == null) {
			return;
		}

		ControllerActors.instance.UntargetAll();
		nextTarget.Target();
	}
}