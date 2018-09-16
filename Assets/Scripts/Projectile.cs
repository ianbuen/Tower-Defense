using UnityEngine;

public enum ProjectileType { Rock, Arrow, Fireball }

public class Projectile : MonoBehaviour {

    [SerializeField] private ProjectileType projectileType;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;

	public bool ReachedTarget { get; private set; }

	public Enemy TargetEnemy { get; private set; }

	public ProjectileType ProjectileType { get { return projectileType; } }

	public float AttackPower { get { return attackPower; } }
    public float AttackSpeed { get { return attackSpeed; } }

	void Start() {
		ReachedTarget = false;
	}

	void Update() {
		if (transform.parent == null || TargetEnemy == null)
			Destroy(gameObject);
	}

	public void SetTarget(Enemy enemy) {
		TargetEnemy = enemy;
	}

	private void OnTriggerEnter2D(Collider2D collision) {

		if (collision.gameObject.tag == "Enemy") {
			ReachedTarget = true;
			Destroy(gameObject);
		}
	}
}
