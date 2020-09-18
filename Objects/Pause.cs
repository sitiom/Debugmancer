using Godot;

namespace Debugmancer.Objects
{
	public class Pause : Control
	{		
		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionJustPressed("pause"))
			{
				GetTree().Paused = !GetTree().Paused;
				Visible = GetTree().Paused;
				GetParent().GetParent().GetNode("ModuleDropper").SetProcessInput(!GetTree().Paused);
			}
		}
		public void _on_ResumeButton_button_up()
		{
			GetParent().GetParent().GetNode("ModuleDropper").SetProcessInput(true);
			GetTree().Paused = false;
			Visible = false;	
		}
		public void _on_QuitButton_button_up()
		{
			GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play(12.47f);
			GetTree().Paused = false;
			GetTree().ChangeScene("res://Levels/Main Menu.tscn");
		}
	}
}
