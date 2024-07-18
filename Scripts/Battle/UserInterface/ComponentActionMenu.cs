using System;
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

	public ActorHero _actor;

	public Action _preflightAction;

	public ActorBase[] _lastTargetedActors = Array.Empty<ActorBase>();

	public bool IsTargeting => _preflightAction != null;

	public override void _EnterTree() {
		instance = this;

		_attackIcon.Hide();
	}

	public bool TryShow(ActorHero actor) {
		_actor = actor;
		_attackIcon.Show();

		return true;
	}

	public void UntargetAllActors() {
		foreach (var actor in ControllerActors.instance.Actors) {
			actor.Untarget();
		}
	}

	public void _TryConfirm(InputEvent @event) {
		if (!@event.IsActionPressed("Confirm")) {
			return;
		}

		if (!_actor.IsReady) {
			return;
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

	public void _BeginTargetSelect(Action.ActionTypes actionType) {
		_attackIcon.Hide();
		_inputAudio.Play();

		UntargetAllActors();
		_GetTargetEnemy().Target();

		_preflightAction = new Action {
			actionType = actionType
		};
	}

	private ActorBase _GetTargetEnemy() {
		if (_lastTargetedActors.Length > 0) {
			var enemy = _lastTargetedActors[0];

			if (!enemy.IsDead) {
				return enemy;
			}
		}

		return ControllerActors.instance.Enemies.First();
	}

	public void _SubmitAction() {
		var action = _preflightAction;

		action.targetActors = ControllerActors.instance.Targeted.ToArray();
		action.animation = new MeleeAnimation(_actor, action.targetActors[0], action);

		_lastTargetedActors = action.targetActors;
		_preflightAction = default;

		UntargetAllActors();

		_actor.TryBeginAction(action);
		_inputAudio.Play();
	}

	public void _SelectNextTarget(Direction direction) {
		var enemies = ControllerActors.instance.Enemies.ToList();
		if (enemies.Count == 0) {
			return;
		}

		var currentTarget = enemies.FirstOrDefault(actor => actor.IsTargeted);
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

		UntargetAllActors();
		nextTarget.Target();
	}

	public override void _Input(InputEvent @event) {
		_TryConfirm(@event);
		_TrySelect(@event);
	}
}