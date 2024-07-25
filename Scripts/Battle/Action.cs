namespace FinalDungeon.Battle;

public class Action {
	// @TODO Convert to Resource actions.
	public enum ActionTypes {
		Attack
	}

	public ActionTypes actionType;
	public IAnimation animation;

	public ActorBase[] targetActors;

	public void DamageTargets(float damage) {
		foreach (var actor in targetActors) {
			actor.ApplyDamage(damage);
		}
	}
}