using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int DamageAmount;
    public float PlayerHealth = 100;
    public Slider HealthSlider;
    [SerializeField] string nextSceneName;
    public string currentSceneName;
    [SerializeField] bool respawnInCurrentScene = true;

    bool loadingNewScene = false;

    PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        HealthSlider.maxValue = PlayerHealth;
        HealthSlider.value = PlayerHealth;
        if (respawnInCurrentScene)
            currentSceneName = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        HealthSlider.value = PlayerHealth;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Damage") || collision.gameObject.CompareTag("Enemy")) {
            if (collision.gameObject.name == "Instakill")
                PlayerHealth = 0f;
            Damage(collision.bounds.center);
            Debug.Log("Ouch");
        }
        else if (collision.gameObject.name == "Nextlevel") {
            AdvanceScene();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Damage") || collision.gameObject.CompareTag("Enemy")) {
            Damage(collision.otherCollider.bounds.center);
            Debug.Log("Ouch");
        }
    }
    public void AdvanceScene() {
        ChangeScene(nextSceneName);
    }
    public void Damage(Vector2 originPoint)
    {
        movement.Knockback(originPoint);
        PlayerHealth -= DamageAmount;
        if (PlayerHealth <= 0) {
            Debug.Log("Player is big dead");
            movement.enabled = false;
            if (!loadingNewScene) {
                ChangeScene(currentSceneName);
            }
        }
        Debug.Log("Ouchie");
    }

    public void ChangeScene(string nextScene) {
        StartCoroutine("SceneChange", nextScene);
    }
    IEnumerator SceneChange(string nextScene) {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(nextScene);
        yield break;
    }
}
