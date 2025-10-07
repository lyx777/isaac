using Godot;
using System;

public partial class Items : Area2D
{
    [Export] TextureRect textureRect;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }
    protected virtual void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			textureRect.Visible = true;
		}
	}
	protected virtual void OnBodyExited(Node body)
	{
		if (body is Player player)
		{
			textureRect.Visible = false;
		}
	}
}
