namespace FinalDungeon.Battle;

public class Action {
	// @TODO Convert to Resource actions.
	public enum ActionTypes {
		Attack
	}

	public ActionTypes actionType;

	public ActorBase[] targetActors;

	public void DamageTargets(float damage) {
		foreach (var actor in targetActors) {
			actor.ApplyDamage(damage);
		}
	}

	public bool TryBeginAction(ActorBase actor) {
		switch (actionType) {
			case ActionTypes.Attack:
				return actor.stateMachine.TryEnter("Attack");

			default:
				return false;
		}
	}
}