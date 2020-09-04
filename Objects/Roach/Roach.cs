using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.Roach.States;
using Godot;

namespace Debugmancer.Objects.Roach
{
	public class Roach : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();

		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();

		public override void _Ready()
		{
			StatesMap.Add("Chase", GetNode("States/Chase"));
			StatesMap.Add("Stagger", GetNode("States/Stagger"));

			CurrentState = (State)GetNode("States/Chase");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			StateStack.Push((State)StatesMap["Chase"]);
			ChangeState("Chase");
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
			else if (stateName == "Stagger")
			{
				StateStack.Push((State)StatesMap[stateName]);
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();

			// Pass target to Chase State
			if (stateName == "Chase")
			{
				((Chase)CurrentState).Init((Player.Player)GetParent().GetNode<KinematicBody2D>("Player"));
			}

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		public async void OnHealthChanged(int health)
		{
			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
				ChangeState("Dead");
		}

		public void Hitbox_BodyEntered(Node body)
		{
			Health health = (Health)GetNode("Health");
			if (body.IsInGroup("playerBullet")) health.Damage(1);

			if (body.IsInGroup("playerCritBullet") && health.CurrentHealth <= 0)
			{
				health.Damage(2);
				ChangeState("Stagger");
			}
		}
	}
}