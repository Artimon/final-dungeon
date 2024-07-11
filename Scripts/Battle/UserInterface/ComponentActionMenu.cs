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

	public ActorHero _actor;

	public Action _preflightAction;

	public override void _EnterTree() {
		instance = this;

		_attackIcon.Hide();
	}

	public bool TryShow(ActorHero actor) {
		_actor = actor;
		_attackIcon.Show();

		return true;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Battle Confirm")) {
			if (_preflightAction != null) {
				_SubmitAction();

				return;
			}

			_BeginTargetSelect(Action.ActionTypes.Attack);
		}
	}

	public void UntargetAllActors() {
		foreach (var actor in ControllerActors.instance.Actors) {
			actor.Untarget();
		}
	}

	public void _BeginTargetSelect(Action.ActionTypes actionType) {
		_attackIcon.Hide();
		_inputAudio.Play();

		UntargetAllActors();

		// @TODO Load last confirmed enemy list, if available.
		ControllerActors.instance.Enemies.First().Target();

		_preflightAction = new Action {
			actionType = actionType
		};
	}

	public void _SubmitAction() {
		var action = _preflightAction;

		action.targetActors = ControllerActors.instance.Targeted.ToArray();

		_actor.action = action;
		_preflightAction = default;
		_inputAudio.Play();

		UntargetAllActors();

		ControllerAtb.instance.Enqueue(_actor);

		// @TODO Save last confirmed enemy list.
	}
}