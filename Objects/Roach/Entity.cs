using System.Collections.Generic;
using Godot;

namespace Debugmancer.Objects.Roach
{
	public class Entity : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();

		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();

		public override void _Ready()
		{
			StatesMap.Add("Idle", GetNode("State/Idle"));
			StatesMap.Add("Chase", GetNode("State/Chase"));
			StatesMap.Add("Stagger", GetNode("State/Stagger"));

			CurrentState = (State)GetNode("State/Idle");

			foreach (Node state in StatesMap.Values)
			{
				//state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
		}

		public override void _PhysicsProcess(float delta)
		{
			CurrentState.Update(this, delta);
		}

		private void ChangeState(string stateName)
		{
			CurrentState.Exit(this);
			if (stateName == "Previous")
			{
				StateStack.Pop();
			}
			else if (stateName == "Dead")
			{
				QueueFree();
				return;
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		public void OnHealthChanged(int health)
		{
			if (health == 0)
				ChangeState("Dead");
		}

		public void Hitbox_AreaEntered(Area2D body)
		{
			Health health = (Health)GetNode("Health");
			if (body.IsInGroup("playerBullet")) health.Damage(1);

			if (body.IsInGroup("playerCritBullet")) health.Damage(2);
		}
	}
}
