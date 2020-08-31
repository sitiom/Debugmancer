using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
	[Export] public int Speed = 150;
	private Vector2 _inputVector = Vector2.Zero;
	private bool _isDashing;
	private bool _canDash = true;
	
	public override void _Process(float delta)
	{
		// TODO: Future stuff here
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_isDashing)
		{
			MoveAndSlide(_inputVector.Normalized() * new Vector2(1000, 1000));
		}
		else
		{
			_inputVector = MoveAndSlide(GetInput());
		}
		Sprite weapon = GetNode<Sprite>("Gun");
		if (weapon.Rotation >= -1.4 && weapon.Rotation <= 1.4) TurnRight();
		if (weapon.Rotation < -1.4 || weapon.Rotation > 1.4) TurnLeft();
		if (weapon.Rotation > 4 || weapon.Rotation < 4) weapon.Rotation = 0;
		if (Input.IsActionPressed("click")) GD.Print(weapon.Rotation);

	}
	private Vector2 GetInput()
	{
		Vector2 velocity = new Vector2();
		velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		if (Input.IsActionJustPressed("dash") && _canDash) Dash();
		
		return velocity * Speed;
	}
	private void TurnLeft()
	{
		Sprite weapon = GetNode<Sprite>("Gun");
		weapon.Position = new Vector2(
			x: -Mathf.Abs(weapon.Position.x),
			y: weapon.Position.y
		);
		weapon.FlipV = true;
		Sprite player = GetNode<Sprite>("Sprite");
		player.FlipH = true;
	}
	private void TurnRight()
	{
		Sprite weapon = GetNode<Sprite>("Gun");
		weapon.Position = new Vector2(
			x: Mathf.Abs(weapon.Position.x),
			y: weapon.Position.y
		);
		weapon.FlipV = false;
		Sprite player = GetNode<Sprite>("Sprite");
		player.FlipH = false;
	}
	public async void Dash()
	{
		_isDashing = true;
		// Visual feedback for dashing
		Modulate = Color.Color8(100, 100, 100);
		await ToSignal(GetTree().CreateTimer(.07f), "timeout");
		Modulate = new Color(1, 1, 1);
		_isDashing = false;
		DashTimer();
	}

	public async void DashTimer()
	{
		_canDash = false;
		await ToSignal(GetTree().CreateTimer(3), "timeout");
		_canDash = true;
	}
	
}
