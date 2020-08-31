using Godot;
using System;

public class Bullet : RigidBody2D
{
	public void _on_Bullet_body_entered(RigidBody2D body)
	{
		if (!body.IsInGroup("player")) QueueFree();
	}
}






