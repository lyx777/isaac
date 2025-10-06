using Godot;
using System;

public partial class Stone : StaticBody2D
{
	[Export] public Texture2D[] RockTextures; // 6种贴图

	private Sprite2D sprite;
	private Random rnd = new Random();

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");

		if (RockTextures != null && RockTextures.Length > 0)
		{
			int index = rnd.Next(RockTextures.Length);
			sprite.Texture = RockTextures[index];
		}
	}
	public void Break()
	{
		GD.Print("墙体被炸毁！");
		QueueFree();
	}
}
