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

    bool invuln = false;
    [SerializeField] float invulnBlinkSpeed = 4f;
    [SerializeField] float invulnTime = 1f;
    [SerializeField] Material invulnBlinkRed;
    [SerializeField] Material invulnBlinkNormal;
    [SerializeField] Renderer[] blinkMeshes;

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

    private void Update() {
        if (invuln) {
            bool blinkOn = (int)Mathf.Floor(Time.time * invulnBlinkSpeed) % 2 == 0f;
            ChangeBlinkTexture(blinkOn);
        }

    }
    void ChangeBlinkTexture(bool blinkOn) {
        foreach (Renderer m in blinkMeshes) {
            if (blinkOn) {
                m.material = invulnBlinkRed;
            }
            else {
                m.material = invulnBlinkNormal;
            }
        }
    }
    void LateUpdate()
    {
        HealthSlider.value = PlayerHealth;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Nextlevel") {
            AdvanceScene();
        }
        else {
            CompareCollision(collision);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        CompareCollision(collision.collider);
    }
    void CompareCollision(Collider2D c) {
        Debug.Log("Collision: " + c.gameObject.tag);
        if (c.gameObject.CompareTag("Damage") || c.gameObject.CompareTag("Enemy")) {
            if (c.gameObject.name == "Instakill")
                PlayerHealth = 0f;
            EnemyHealth eh = c.gameObject.GetComponent<EnemyHealth>();
            Rigidbody2D erb = c.gameObject.GetComponent<Rigidbody2D>();
            if ((eh && eh.damagePlayerOnContact) && !invuln || !eh)
                Damage(c.bounds.center);
        }
    }
    public void AdvanceScene() {
        ChangeScene(nextSceneName);
    }
    public void Damage(Vector2 originPoint)
    {
        movement.Knockback(originPoint);
        if (!invuln) {
            PlayerHealth -= DamageAmount;
            if (PlayerHealth <= 0) {
                Debug.Log("Player is big dead");
                movement.enabled = false;
                if (!loadingNewScene) {
                    ChangeScene(currentSceneName);
                }
            }
            invuln = true;
            StartCoroutine("InvulnRoutine");
        }
    }

    IEnumerator InvulnRoutine() {
        yield return new WaitForSeconds(invulnTime);
        invuln = false;
        ChangeBlinkTexture(false);
        yield break;
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
