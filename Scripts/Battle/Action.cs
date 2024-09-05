using FinalDungeon.Battle.Config;

namespace FinalDungeon.Battle;

public class Action {
	// @TODO Convert to Resource actions.
	public enum ActionTypes {
		Attack,
		Cast
	}

	public ActionTypes type;
	public ActionSetup setup;

	public ActorBase[] targetActors;

	public void DamageTargets(float damage) {
		foreach (var actor in targetActors) {
			actor.ApplyDamage(damage);
		}
	}

	public bool TryBeginAction(ActorBase actor) {
		switch (type) {
			case ActionTypes.Attack:
				return actor.stateMachine.TryEnter("Attack");

			case ActionTypes.Cast:
				return actor.stateMachine.TryEnter("Cast");

			default:
				return false;
		}
	}
}