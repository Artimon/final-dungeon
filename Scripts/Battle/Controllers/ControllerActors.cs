using System.Collections.Generic;
using System.Linq;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ControllerActors : Node {
	public static ControllerActors instance;
	public List<ActorBase> _actors = new ();

	public IEnumerable<ActorBase> Actors => _actors.ToArray();
	public IEnumerable<ActorBase> Heroes => _actors.Where(actor => !actor.isEnemy);
	public IEnumerable<ActorBase> Enemies => _actors.Where(actor => actor.isEnemy);
	public IEnumerable<ActorBase> Targeted => _actors.Where(actor => actor.IsTargeted);

	public override void _EnterTree() {
		instance = this;
	}

	public void Register(ActorBase actor) {
		if (!_actors.Contains(actor)) {
			_actors.Add(actor);
		}
	}

	public void Unregister(ActorBase actor) {
		_actors.Remove(actor);
	}

	public ActorBase GetFirst(bool isEnemy) {
		foreach (var actor in _actors) {
			if (actor.isEnemy == isEnemy) {
				return actor;
			}
		}

		return null;
	}

	public override void _ExitTree() {
		_actors.Clear();
	}
}