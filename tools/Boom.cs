using Godot;
using System;

public partial class Boom : Node2D
{
	public float Radius = 100f;

	public float Duration = 1.5f;
	public int Damage = 10;

	public override void _Ready()
	{
		var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		anim.Play("boom");
	}

	public void Explode()
	{
		var bodies = new Godot.Collections.Array<Node>();
		bodies.AddRange(GetTree().GetNodesInGroup("enemy"));
		bodies.AddRange(GetTree().GetNodesInGroup("player"));
		bodies.AddRange(GetTree().GetNodesInGroup("breakable"));
		foreach (var node in bodies)
		{
			if (node is Node2D body)
			{
				float distance = GlobalPosition.DistanceTo(body.GlobalPosition);

				if (distance <= Radius)
				{
					if (body is CombatActor actor)
					{
						if (!actor.IsInGroup("player")) actor.TakeDamage(Damage, GlobalPosition,true);
						else actor.TakeDamage(2);
						GD.Print($"{actor.Name} took {Damage} damage from boom!");
					}
					else if(body is Stone stone)
					{
						stone.Break();
					}
				}
			}
			
		}
	}
	public override void _Process(double delta)
	{
		Duration -= (float)delta;
		if (Duration <= 0)
		{
			Explode();
			QueueFree();
		}
	}

}
