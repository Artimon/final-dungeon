using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace FinalDungeon.Battle.UserInterface;

[GlobalClass]
public partial class ComponentActionMenu : Control {
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

	public Func<InputEvent, bool>[] _inputHandlers;

	public bool _isShuttingDown;

	public override void _EnterTree() {
		instance = this;

		_inputHandlers = new [] {
			_TryAction,
			_TryConfirm,
			_TryCancel,
			_TrySelect,
			_TrySwap
		};

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

	public void Shutdown() {
		_isShuttingDown = true;

		Hide();

		ControllerActors.instance.UntargetAll();

		_currentActor = null;
		_actors.Clear();
	}

	public void _Begin(ActorHero actor) {
		if (_isShuttingDown) {
			return;
		}

		_currentActor = actor;
		_currentActor.Target(); // No need to untarget later, since confirm untargets all.

		Show();
	}

	public bool _TryGetAction(InputEvent @event, out Action action) {
		action = default;

		if (@event.IsActionPressed("Bottom Action")) {
			action = new Action {
				type = Action.ActionTypes.Attack,
				setup = _currentActor.actions[0]
			};

			return true;
		}

		if (@event.IsActionPressed("Left Action")) {
			var castAction = _currentActor.actions[1];
			if (!_currentActor.CanCast(castAction)) {
				return false;
			}

			action = new Action {
				type = Action.ActionTypes.Cast,
				setup = castAction
			};

			return true;
		}

		return false;
	}

	public bool _TryAction(InputEvent @event) {
		if (IsTargeting) {
			return false;
		}

		var hasAction = _TryGetAction(@event, out var action);
		if (!hasAction) {
			return false;
		}

		if (_currentActor == null) {
			return false;
		}

		if (!_currentActor.IsReady) {
			return false; // Maybe switch to next in queue.
		}

		_BeginTargetSelect(action);

		return true;
	}

	public bool _TryConfirm(InputEvent @event) {
		if (!IsTargeting) {
			return false;
		}

		if (!@event.IsActionPressed("Bottom Action")) {
			return false;
		}

		_SubmitAction();

		return true;
	}

	public bool _TryCancel(InputEvent @event) {
		if (!IsTargeting) {
			return false;
		}

		if (!@event.IsActionPressed("Right Action")) {
			return false;
		}

		_preflightAction = default;

		ControllerActors.instance.UntargetAll();
		_currentActor.Target();

		Show();

		return true;
	}

	public bool _TrySelect(InputEvent @event) {
		if (!IsTargeting) {
			return false;
		}

		_preflightAction.setup.targetSelect.OnInputEvent(@event);

		return true;
	}

	public bool _TrySwap(InputEvent @event) {
		if (@event.IsActionPressed("Prev")) {
			TrySwap(-1);

			return true;
		}

		if (@event.IsActionPressed("Next")) {
			TrySwap(1);

			return true;
		}

		return false;
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

		ControllerActors.instance.UntargetAll();
		_Begin(actor);

		return true;
	}

	public void _BeginTargetSelect(Action action) {
		Hide();

		_inputAudio.Play();

		_preflightAction = action;
		_preflightAction.setup.targetSelect.Begin(_lastTargetedActors);
	}

	public void RefreshTargeting() {
		if (!IsTargeting) {
			return;
		}

		_preflightAction.setup.targetSelect.Begin(_lastTargetedActors);
	}

	public void _SubmitAction() {
		var action = _preflightAction;

		action.targetActors = ControllerActors.instance.Targeted.ToArray();

		_lastTargetedActors = action.targetActors;
		_preflightAction = default;

		ControllerActors.instance.UntargetAll();

		// @TODO Change to queue action, to wait for hit animation(s) to finish.
		_currentActor.TryBeginAction(action);
		_currentActor = null;

		_inputAudio.Play();

		var hasNext = _actors.TryDequeue(out var nextActor);
		if (hasNext) {
			_Begin(nextActor);
		}
	}

	public override void _Input(InputEvent @event) {
		foreach (var inputHandler in _inputHandlers) {
			if (inputHandler(@event)) {
				break;
			}
		}
	}
}