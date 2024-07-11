namespace FinalDungeon.Battle;

public class Action {
	// @TODO Convert to Resource actions.
	public enum ActionTypes {
		Attack
	}

	public ActionTypes actionType;

	public ActorBase[] targetActors;

	public bool isUsed;
}