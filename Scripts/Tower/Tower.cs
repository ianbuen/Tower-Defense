using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRadius;

    [SerializeField] private TowerRange range;
    private CircleCollider2D rangeCollider;

	[SerializeField] private Projectile projectile;

	private Enemy previousTarget;
    private float attackCounter;
    private bool isAttacking;

	public Enemy TargetEnemy { get; private set; }

	public Projectile TowerProjectile { get { return projectile; } }

	void Start() {
		range = Instantiate(range);
        rangeCollider = range.GetComponent<CircleCollider2D>();
		rangeCollider.transform.position = transform.position;
		rangeCollider.radius = attackRadius;
		range.transform.parent = transform;
	}

	void Update() {

        // if enemies in range
        if (GameManager.Instance.GameState == GameStatus.Play && range.EnemiesInRange.Count > 0) {

            // if no target, get nearest one
            if (TargetEnemy == null)
			    TargetEnemy = GetNearestEnemyInRange();

            // if target in range, attack
            else if (range.EnemiesInRange.Contains(TargetEnemy)) 
				if (Time.time > attackCounter) {
					attackCounter = Time.time + attackDelay;
					isAttacking = true;
				}
			
            // if enemy gets out of radius, set it as previous
			else {
				previousTarget = TargetEnemy;
				TargetEnemy = null;
			}

		}
	}

	void FixedUpdate() {

        if (isAttacking) {
			Attack();
		}
	}

	public void Attack() {

		isAttacking = false;

		Projectile projectile = Instantiate(this.projectile) as Projectile;
		projectile.transform.position = new Vector2(transform.position.x, transform.position.y + 0.75f);
		projectile.transform.parent = transform;
		projectile.SetTarget(TargetEnemy);

		switch (this.projectile.ProjectileType) {
		    case ProjectileType.Arrow:
			    SoundManager.Instance.Play(SFX.Arrow);
			    break;
		    case ProjectileType.Fireball:
			    SoundManager.Instance.Play(SFX.Fireball);
			    break;
		    case ProjectileType.Rock:
			    SoundManager.Instance.Play(SFX.Rock);
			    break;
		}

        StartCoroutine(FireProjectile(projectile));
	}

	private IEnumerator FireProjectile(Projectile projectile) {

		Enemy target = TargetEnemy;

		if (target == null) 
			target = previousTarget;
		
		while (projectile != null && target != null && !projectile.ReachedTarget) {
			Vector2 direction = target.transform.localPosition - transform.localPosition;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			projectile.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward * 3f);
			projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, target.transform.position, Time.deltaTime * projectile.AttackSpeed);

			yield return null;
		}
	}

	private Enemy GetNearestEnemyInRange() {

		Enemy result = null;
		float num = float.PositiveInfinity;

		foreach (Enemy enemy in range.EnemiesInRange.ToArray()) {
			if (enemy != null && range.EnemiesInRange.Contains(enemy)) {
				float sqrMagnitude = (enemy.transform.position - transform.position).sqrMagnitude;

				if (sqrMagnitude < num) {
					num = sqrMagnitude;
					result = enemy;
				}
			}
		}

		return result;
	}
}
