using System.Collections.Generic;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ControllerAtb : Node {
	public static ControllerAtb instance;
	public Queue<ActorBase> _actorQueue = new ();

	public override void _EnterTree() {
		instance = this;
	}

	public void Enqueue(ActorBase actor) {
		if (_actorQueue.Count == 0) {
			actor.TryBeginAction();
			actor.ResetActionTime(); // @TODO Must be based on action callback.

			return;
		}

		_actorQueue.Enqueue(actor);
	}
}