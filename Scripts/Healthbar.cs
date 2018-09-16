using UnityEngine;

public class Healthbar : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	private Enemy enemy;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		enemy = transform.parent.parent.gameObject.GetComponent<Enemy>();
	}

	private void Update() {
        transform.localScale = new Vector3(enemy.Health / enemy.MaxHealth, transform.localScale.y, transform.localScale.z);

        if (enemy.Health > 0f && enemy.Health <= enemy.MaxHealth * 0.25f)
            spriteRenderer.color = Color.red;

        else if (enemy.Health > enemy.MaxHealth * 0.25f && enemy.Health <= enemy.MaxHealth * 0.5f)
            spriteRenderer.color = Color.yellow;

        else if (enemy.Health > enemy.MaxHealth * 0.5f)
            spriteRenderer.color = Color.green;

		if (enemy.IsDead || GameManager.Instance.GameState == GameStatus.GameOver)
			Destroy(transform.parent.gameObject);
	}
}
