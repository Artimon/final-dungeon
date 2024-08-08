using System.Collections.Generic;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateMachine : Node {
	public Dictionary<string, StateBase> _states = new ();

	[System.NonSerialized]
	public StateBase _state;

	public override void _Process(double delta) {
		_state.OnProcess(delta);
	}

	public void Add(StateBase state) {
		_states.Add(state.StateName, state);

		if (!state.isDefault) {
			return;
		}

		_state = state;
		_state.OnEnter();
	}

	public bool TryEnter(string stateName) {
		if (!CanEnter(stateName)) {
			return false;
		}

		Force(stateName);

		return true;
	}

	public void Force(string stateName) {
		_state.OnExit();
		_state = _states[stateName];
		_state.OnEnter();
	}

	public bool CanEnter(string stateName) {
		if (IsInState(stateName)) {
			return false;
		}

		return _state.CanExit();
	}

	public bool IsInState(string stateName) {
		return _state.StateName == stateName;
	}
}