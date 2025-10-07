using Godot;
using System;
using System.Collections.Generic;

public partial class Spikeweed : Area2D
{
	[Export] public int Damage = 1;                // 伤害
	[Export] public float DamageInterval = 1.0f;   // 持续踩着时的伤害间隔

	private Dictionary<CombatActor, float> damageCooldown = new Dictionary<CombatActor, float>();

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is CombatActor actor)
		{
			actor.TakeDamage(Damage);
			damageCooldown[actor] = DamageInterval;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is CombatActor actor)
		{
			if (damageCooldown.ContainsKey(actor))
				damageCooldown.Remove(actor);
		}
	}

	public override void _Process(double delta)
	{
		var keys = new List<CombatActor>(damageCooldown.Keys);
		foreach (var body in keys)
		{
			damageCooldown[body] -= (float)delta;
			if (damageCooldown[body] <= 0)
			{
				body.TakeDamage(Damage);
				damageCooldown[body] = DamageInterval;
			}
		}
	}
}
