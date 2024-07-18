using System.Collections.Generic;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ControllerAtb : Node {
	public static ControllerAtb instance;
	public Queue<ActorBase> _actorQueue = new ();

	// @TODO Stop progress while action animations are playing.
	// @TODO Use ControllerActors.Actors to trigger atb progress from here.
	// @TODO Ooooor, make everything work at the "same time", just like in Starbattle Online.
	// @TODO On stagger/hit don't reset action time, but instead lock it. Re-open the action menu after the stagger/hit.
	public override void _EnterTree() {
		instance = this;
	}

	public void Enqueue(ActorBase actor) {
		// if (_actorQueue.Count == 0) {
		// 	actor.TryBeginAction();
		// 	actor.ResetActionTime(); // @TODO Must be based on action callback.
		//
		// 	return;
		// }
		//
		// _actorQueue.Enqueue(actor);
	}
}