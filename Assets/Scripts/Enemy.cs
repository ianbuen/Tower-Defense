using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float healthPoints;
    [SerializeField] private int goldDropAmount;

    [SerializeField] private float navigationUpdate;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float movementSpeed = 1f;
    private int targetWaypointIndex;
    private float navigationTime;

    private Animator animator;
    private Collider2D collider;
    private SpriteRenderer renderer;

    private bool isDead;
    public float MaxHealth { get; private set; }
	public float Health { get { return healthPoints; } }
    public bool IsDead { get { return isDead; } }
    private bool GotHit { get; set; }

	private void Start() {
		GameManager.Instance.AddEnemy(this);
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();
		renderer = GetComponent<SpriteRenderer>();
		MaxHealth = healthPoints;
	}

	private void Update() {

		if (GameManager.Instance.GameState == GameStatus.Play && waypoints != null && !isDead) {
            navigationTime += Time.deltaTime;

			if (navigationTime > navigationUpdate) {
				if (targetWaypointIndex < waypoints.Length)
					transform.position = Vector2.MoveTowards(transform.position, waypoints[targetWaypointIndex].transform.position, navigationTime * movementSpeed);
				else
					transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Exit.transform.position, navigationTime * movementSpeed);

				navigationTime = 0;
			}

		} else if (GameManager.Instance.GameState == GameStatus.GameOver) {
			animator.SetTrigger("Taunt");
			healthPoints = MaxHealth;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Waypoint") {
			targetWaypointIndex++;
		} else if (other.gameObject.tag == "Finish") {
			GameManager.Instance.RemoveEnemy(this);
			GameManager.Instance.AddEscape();
		} else if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile>().TargetEnemy == this) {
			Projectile component = other.gameObject.GetComponent<Projectile>();
			SoundManager.Instance.Play(SFX.Hit);
			TakeDamage(component.AttackPower);
		}
	}

	private void TakeDamage(float hitPoints) {

        healthPoints -= hitPoints;

		if (healthPoints > 0f) 
			animator.Play("Hurt");
		else 
			Die();

	}

	private void Die() {
		isDead = true;
		collider.enabled = false;
		animator.SetTrigger("Death");
        renderer.sortingOrder = 0;
		SoundManager.Instance.Play(SFX.Death);
		GameManager.Instance.UpdateGold(goldDropAmount);
		GameManager.Instance.RemoveEnemy(this);
	}

	private void OnDestroy() {
		if (IsDead)
			GameManager.Instance.AddKill();
	}
}
