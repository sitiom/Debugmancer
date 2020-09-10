using Godot;
using System;

public class Menu : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    private void _on_Start_pressed()
    {
        GetNode<ColorRect>("ColorRect").Show();
        GetNode<AnimationPlayer>("MenuAnimPlayer").Play("FadeOut");
    }

    private void _on_Quit_pressed()
    {
        GetTree().Quit();
    }

    private void _on_MenuAnimPlayer_finished(string anim_name)
    {
        if (anim_name == "FadeIn")
        {
            GetNode<ColorRect>("ColorRect").Hide();
        }
        if (anim_name == "FadeOut")
        {
            GetTree().ChangeScene("res://Levels/TestArena.tscn");
        }
    }
}