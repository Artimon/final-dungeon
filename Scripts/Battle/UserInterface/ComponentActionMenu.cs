using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace FinalDungeon.Battle.UserInterface;

[GlobalClass]
public partial class ComponentActionMenu : Control {
	public enum Direction {
		Left,
		Right,
		// Future directions can be added here, e.g., Up, Down
	}

	public static ComponentActionMenu instance;

	[Export]
	public TextureRect _attackIcon;

	[Export]
	public AudioStreamPlayer _inputAudio;

	public ActorHero _currentActor;
	public readonly Queue<ActorHero> _actors = new ();

	public Action _preflightAction;

	public ActorBase[] _lastTargetedActors = Array.Empty<ActorBase>();

	public bool IsTargeting => _preflightAction != null;

	public override void _EnterTree() {
		instance = this;

		Hide();
	}

	/**
	 * Note: After being hit, the actor will call this method again, even if already queued.
	 */
	public void Add(ActorHero actor) {
		if (_currentActor == null) {
			_Begin(actor);

			return;
		}

		if (_currentActor == actor) {
			return;
		}

		if (_actors.Contains(actor)) {
			return;
		}

		_actors.Enqueue(actor);
	}

	public void _Begin(ActorHero actor) {
		_currentActor = actor;
		_currentActor.Target(); // No need to untarget later, since confirm untargets all.

		Show();
	}

	public void _UntargetAllActors() {
		foreach (var actor in ControllerActors.instance.Actors) {
			actor.Untarget();
		}
	}

	public void _TryConfirm(InputEvent @event) {
		if (!@event.IsActionPressed("Confirm")) {
			return;
		}

		if (_currentActor == null) {
			return;
		}

		if (!_currentActor.IsReady) {
			return; // Maybe switch to next in queue.
		}

		if (_preflightAction != null) {
			_SubmitAction();

			return;
		}

		_BeginTargetSelect(Action.ActionTypes.Attack);
	}

	public bool _TrySelect(InputEvent @event) {
		if (!IsTargeting) {
			return false;
		}

		if (@event.IsActionPressed("East")) {
			_SelectNextTarget(Direction.Right);
		}
		else if (@event.IsActionPressed("West")) {
			_SelectNextTarget(Direction.Left);
		}

		return true;
	}

	public void _TrySwap(InputEvent @event) {
		if (@event.IsActionPressed("Prev")) {
			TrySwap(-1);
		}
		else if (@event.IsActionPressed("Next")) {
			TrySwap(1);
		}
	}

	public bool TrySwap(int direction) {
		if (_actors.Count == 0) {
			return false;
		}

		var actor = _currentActor;

		var heroes = ControllerActors.instance.Heroes.ToList();
		while (true) {
			var currentIndex = heroes.IndexOf(actor);
			var nextIndex = Mathf.Abs((currentIndex + direction) % heroes.Count);

			actor = (ActorHero)heroes[nextIndex];

			if (_actors.Contains(actor)) {
				break;
			}
		}

		_preflightAction = null;

		/* Begin ugly part to swap the current actor. */
		var tempList = _actors.ToList();
		tempList.Remove(actor);
		_actors.Clear();

		foreach (var remainingActor in tempList) {
			_actors.Enqueue(remainingActor);
		}

		_actors.Enqueue(_currentActor);
		_currentActor = null;
		/* End ugly part to swap the current actor. */

		_UntargetAllActors();
		_Begin(actor);

		return true;
	}

	public void _BeginTargetSelect(Action.ActionTypes actionType) {
		Hide();

		_inputAudio.Play();

		_UntargetAllActors();
		_GetTargetEnemy().Target();

		_preflightAction = new Action {
			actionType = actionType
		};
	}

	public void RefreshTargeting() {
		if (!IsTargeting) {
			return;
		}

		_UntargetAllActors();
		_GetTargetEnemy().Target();
	}

	private ActorBase _GetTargetEnemy() {
		if (_lastTargetedActors.Length > 0) {
			var enemy = _lastTargetedActors[0];

			if (enemy.CanBeTargeted) {
				return enemy;
			}
		}

		return ControllerActors.instance.Enemies.FirstOrDefault(actor => actor.CanBeTargeted);
	}

	public void _SubmitAction() {
		var action = _preflightAction;

		action.targetActors = ControllerActors.instance.Targeted.ToArray();

		_lastTargetedActors = action.targetActors;
		_preflightAction = default;

		_UntargetAllActors();

		// @TODO Change to queue action, to wait for hit animation(s) to finish.
		_currentActor.TryBeginAction(action);
		_currentActor = null;

		_inputAudio.Play();

		var hasNext = _actors.TryDequeue(out var nextActor);
		if (hasNext) {
			_Begin(nextActor);
		}
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

		_UntargetAllActors();
		nextTarget.Target();
	}

	public override void _Input(InputEvent @event) {
		_TryConfirm(@event);
		_TrySelect(@event);
		_TrySwap(@event);
	}
}