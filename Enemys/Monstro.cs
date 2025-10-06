using Godot;
using System;

public partial class Monstro : Enemy
{
	private CollisionShape2D collisionShape;

	public float JumpSpeed = 180f;           // 短跳速度
	public float BigJumpDelay = 1.5f;        // 大跳消失时间
	public float BigJumpLandingRadius = 150f;
	public int ProjectilesPerShot = 25;       // 大跳落地后发射的泪弹数量
	public float ShortJumpCooldown = 3.0f;   // 短跳冷却
	public float BigJumpCooldown = 10.0f;     // 大跳冷却

	private Vector2 TargetPosition;
	private float shortJumpTimer = 0f;
	private float bigJumpTimer = 0f;
	private bool isBigJumping = false;

	private bool isSmallJumping = false;
	private Random rnd = new Random();
	public Player player;
	public override void _Ready()
	{
		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");
		BulletSpeed = 200f;
		player = GetTree().Root.GetNode<Player>("Main/Player");
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		Speed = 50f; 
		MaxHealth = 50;
		base._Ready();
		shortJumpTimer = ShortJumpCooldown;
		bigJumpTimer = BigJumpCooldown;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player == null) return;

		shortJumpTimer -= (float)delta;
		bigJumpTimer -= (float)delta;
		if (isBigJumping) return; // 大跳时不做其他动作
		if (isSmallJumping)
		{
			Vector2 dir = (TargetPosition - GlobalPosition).Normalized();
			Velocity = dir * JumpSpeed;
			MoveAndSlide();

			// 接近目标点就停止
			if (GlobalPosition.DistanceTo(TargetPosition) < 10f|| GetSlideCollisionCount() > 0)
			{
				isSmallJumping = false;
				Velocity = Vector2.Zero;
				IsControlledExternally = false; // 恢复普通寻路
				ShootTears();// 短跳后攻击
				GD.Print("短跳结束");
			}
			shortJumpTimer = ShortJumpCooldown;
			return; // 短跳期间不执行普通移动
		}
		// 根据距离和计时器决定动作
		float distanceToPlayer = (player.GlobalPosition - GlobalPosition).Length();
		if (bigJumpTimer <= 0f)
		{
			StartBigJump();
			bigJumpTimer = BigJumpCooldown;
		}
		else if (distanceToPlayer < 250f && shortJumpTimer <= 0f)
		{
			// 近距离且短跳冷却完毕，执行短跳
			DoShortJump();
			
		}
		

	}

	private void DoShortJump()
	{
		// 朝玩家短距离移动
		TargetPosition = player.GlobalPosition;
		
		isSmallJumping = true;
		IsControlledExternally = true;

		// LeaveTODO：播放跳跃帧
	}

	private async void StartBigJump()
	{
		isBigJumping = true;
		GD.Print("Boss 大跳开始！");
		Visible = false;            // 消失
		if (collisionShape != null)collisionShape.Disabled = true;      // 关闭碰撞
		SetPhysicsProcess(false);   // 停止普通移动
		TargetPosition = player.GlobalPosition;
		await ToSignal(GetTree().CreateTimer(BigJumpDelay), "timeout");
		// 落地在玩家当前位置
		GlobalPosition = TargetPosition;
		Visible = true;
		if (collisionShape != null)collisionShape.Disabled = false;
		SetPhysicsProcess(true);
		isBigJumping = false;
		
		GD.Print("Boss 大跳落地！");
		shortJumpTimer = ShortJumpCooldown;
		
		EmitProjectilesAround();
	}

	private void ShootTears()
	{
		// 向玩家方向发射多颗子弹
		Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
	   
		for (int i = -2; i <= 2; i++)
		{
			
			float angle = i * Mathf.DegToRad(10); // 小角度散射
			Vector2 shootDir = dir.Rotated(angle);
			shootTimer = 0f;
			Shoot(shootDir, Faction.Enemy);
		}
	}

	private void EmitProjectilesAround()
	{
		float angleStep = (float)(Mathf.Tau / ProjectilesPerShot);
		for (int i = 0; i < ProjectilesPerShot; i++)
		{
			float baseAngle = i * angleStep;
			
			for (int j = 0; j < 2; j++)
			{
				float randomOffset = (GD.Randf() - 0.5f) * Mathf.DegToRad(20); 
				float angle = baseAngle + randomOffset;
				Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
				shootTimer = 0f;
				Shoot(dir, Faction.Enemy);
			}
		}
	}

}
