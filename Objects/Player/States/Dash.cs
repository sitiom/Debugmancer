using System;
using Godot;

namespace Debugmancer.Objects.Player.States
{
	public class Dash : State
	{
		private readonly Vector2 _dashSpeed = new Vector2(215, 215);
		private Vector2 _inputDirection;
		private KinematicBody2D _host;

		public override void Enter(KinematicBody2D host)
		{
			host.GetNode<Timer>("DashTimer").Start();
			host.GetNode<CollisionPolygon2D>("Hitbox/CollisionPolygon2D").Disabled = true;
			host.GetNode<AnimationPlayer>("AnimationPlayer")
				.Play(!host.GetNode<Sprite>("Sprite").FlipH ? "Dash" : "Dash_Flipped");
			_inputDirection = new Vector2(
				Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
				Convert.ToInt32(Input.IsActionPressed("move_down")) - Convert.ToInt32(Input.IsActionPressed("move_up")));
			
			// Visual feedback for dashing
			host.Modulate = Color.Color8(100, 100, 100);
			_host = host;
		}

		public override void Exit(KinematicBody2D host)
		{
			host.GetNode<CollisionPolygon2D>("Hitbox/CollisionPolygon2D").Disabled = false;
		}

		public override void HandleInput(KinematicBody2D host, InputEvent @event)
		{
			// Nothing to do here
		}

		public void DashTimer_timeout()
		{
			_host.GetNode<Timer>("DashTimer").Stop();
			_host.Modulate = new Color(1, 1, 1);
			EmitSignal(nameof(Finished), "Move");
		}

		public override void Update(KinematicBody2D host, float delta)
		{
			host.MoveAndSlide(_inputDirection.Normalized() * _dashSpeed, Vector2.Zero);
		}
	}
}
