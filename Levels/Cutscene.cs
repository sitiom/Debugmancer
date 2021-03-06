using Godot;

namespace Debugmancer.Levels
{
	public class Cutscene : Node2D
	{
		private int _scene = 1;
		public override void _Ready()
		{
			GetNode<Timer>("Timer").Start();
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
		}

		public override void _ExitTree()
		{
			Input.SetMouseMode(Input.MouseMode.Visible);
		}

		public override void _Process(float delta)
		{
			if (Input.IsKeyPressed((int)KeyList.E))
			{
				Engine.TimeScale = 2.5f;
				GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").PitchScale = 2.5f;
				GetNode<AnimatedSprite>("FastForwardSprite").Show();
			}
			else
			{
				if (Engine.TimeScale == 2.5f)
				{

					Engine.TimeScale = 1;
					GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").PitchScale = 1;
					GetNode<AnimatedSprite>("FastForwardSprite").Hide();
				}
			}
		}

		private void _on_Timer_timeout()
		{
			if (_scene == 1) GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play();

			GetNode<Timer>("Timer").Stop();
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			if(_scene == 5)
			{
				_scene++;
			}
		}

		private void _on_AnimationPlayer_finished(string animName)
		{
			if (animName == "FadeOut" && _scene != 6)
			{
				//hide the previous scene
				if (_scene <= 3)
				{
					GetNode<Sprite>($"Sprite{_scene}").Hide();
					GetNode<Label>($"Label{_scene}").Hide();
				}
				else
				{
					GetNode<Label>($"Label{_scene}").Hide();
				}
				//switch to the next scene
				_scene++;
				GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
				if (_scene <= 3)
				{
					GetNode<Sprite>($"Sprite{_scene}").Show();
					GetNode<Label>($"Label{_scene}").Show();
				}
				else
				{
					GetNode<Label>($"Label{_scene}").Show();
				}
				GetNode<Timer>("Timer").Start();
			}

			if(_scene == 6)
			{
				Hide();
				GetTree().ChangeScene("res://Levels/Main Menu.tscn");
			}
		}
	}
}
