using Godot;
using System;

public partial class BossHP : Control
{
	private TextureProgressBar hpBar;

	public override void _Ready()
	{
		hpBar = GetNode<TextureProgressBar>("BossHPBar");
		hpBar.Visible = false; // 初始隐藏
	}

	public void BindBoss(Monstro boss)
	{
		hpBar.Visible = true;
		hpBar.MaxValue = boss.MaxHealth;
		hpBar.Value = boss.MaxHealth;

		boss.HealthChanged += OnBossHealthChanged;
		boss.BossDied += OnBossDied;
	}

	private void OnBossHealthChanged(int current, int max)
	{
		GD.Print($"更新血量: {current}/{max}");
		hpBar.MaxValue = max;
		hpBar.Value = current;
	}

	private void OnBossDied()
	{
		hpBar.Visible = false;
	}
}
