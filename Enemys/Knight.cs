/*
LeaveTODO:
1.Knight面朝不同方向时的动画
*/
using Godot;
using System;

public partial class Knight : Enemy
{
	 public float WalkSpeed = 80f;
	 public float DashSpeed = 200f;
	 public float DashDuration = 1.2f;

	public float DashAngle = 15f; 
	private enum KnightState { Patrol, Dash }
	private KnightState state = KnightState.Patrol;

	private Vector2 moveDir = Vector2.Right;
	private float dashTimer = 0f;

	public Player player;
	public override void _Ready()
	{
		MaxHealth = 4;
		player = GetTree().Root.GetNode<Player>("Main/Player");
		base._Ready();               
		PickRandomDirection();       // 初始化巡逻方向
	}

	public override void _Process(double delta)
	{
		if (currentHealth <= 0)
		{
			GD.Print("Knight Die");
			Die();
			return;
		}
		if (!IsActive) return;
		if (player == null) return;
		//TODO:输出当前移动方向
		//GD.Print($"Knight MoveDir: {moveDir}");
		
		switch (state)
		{
			case KnightState.Patrol:
				Patrol(delta);
				DetectPlayerForDash();
				break;

			case KnightState.Dash:
				DoDash(delta);
				break;
		}
	}

	private void Patrol(double delta)
	{
		Velocity = moveDir * WalkSpeed;
		MoveAndSlide();

		dashTimer -= (float)delta;
		if (dashTimer <= 0f|| GetSlideCollisionCount() > 0)
		{
			PickRandomDirection();
			dashTimer = 2f; // 重置计时器   
		}
	}

	private void DoDash(double delta)
	{
		Velocity = moveDir * DashSpeed;
		MoveAndSlide();

		dashTimer -= (float)delta;
		if (dashTimer <= 0f || GetSlideCollisionCount() > 0)
		{
			state = KnightState.Patrol;
			dashTimer = 1f;
			//PickRandomDirection();
		}
	}
	private bool IsPlayerInFront()
	{
		Vector2 toPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
		// 点积判断夹角
		float dot = moveDir.Dot(toPlayer);
		float cosThreshold = Mathf.Cos(Mathf.DegToRad(DashAngle));
		return dot > cosThreshold;
	}
	private void DetectPlayerForDash()
	{
		Vector2 toPlayer = player.GlobalPosition - GlobalPosition;

		if(toPlayer.Length() < 250f && IsPlayerInFront())
		{
			StartDash();
		}
	}

	private void StartDash()
	{
		state = KnightState.Dash;
		dashTimer = DashDuration;
	}

	private void PickRandomDirection()
	{
		int r = GD.RandRange(0, 3);
		moveDir = r switch
		{
			0 => Vector2.Up,
			1 => Vector2.Down,
			2 => Vector2.Left,
			_ => Vector2.Right
		};
	}

	// -------- 正面防御 ----------
	public override void TakeDamage(int dmg, Vector2 hitFrom,bool isBomb=false)
	{

		Vector2 forward = moveDir.Normalized();
		Vector2 dirFrom = (hitFrom-GlobalPosition).Normalized();

		// 如果子弹来自正面（点积 > 0），免疫
		if (!isBomb &&forward.Dot(dirFrom) > 0.5f)
		{
			GD.Print("Knight 正面防御，不受伤");
			return;
		}

		base.TakeDamage(dmg, hitFrom);   // 背后正常受伤
	}
}
