using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{

    [Header("Components")]
    [SerializeField] CharacterController Controller;
    [SerializeField] GameObject MuzzleFlash;

    [Header("Player Attributes")]
    [Header("--------------------------")]
    public RangedInt Health;

    Vector3 Move;
    Vector3 PlayerVelocity;
    float OriginalSpeed; 
    bool IsSprinting = false;
    [Range(3, 6)] [SerializeField] float PlayerSpeed;
    [Range(1.5f, 4.0f)] [SerializeField] float SprintMultiplyer; 

    int TimesJumped;
    [Range(1, 4)] [SerializeField] int MaxJumps;
    [Range(6, 10)] [SerializeField] float JumpHeight;
    [Range(0, 30)] [SerializeField] float GravityValue;
    [SerializeField] bool CanShoot = true;


    [Header("Player Weapon Stats")]
    [Header("-------------------------")]
    [Range(0.1f, 3.0f)] [SerializeField] float ShootRate;
    [Range(1, 10)] [SerializeField] int WeaponDamage;

    [Header("Effects")]
    [Header("--------------------------")]
    [SerializeField] GameObject HitEffectSpark;

    // Use this region to track anything that needs to be reset at respawn
    #region Respawn Values

    private Vector3 SpawnPosition;
    private Quaternion SpawnRotation;

    #endregion

    private void Start()
    {
        SpawnPosition = transform.position;
        SpawnRotation = transform.rotation;
        
        // Attaches the player to the GameManager
        GameManager.Instance.Player = this.gameObject;
        GameManager.Instance.PlayerScript = this;

        // Record the original speed
        OriginalSpeed = PlayerSpeed;
    }

    void Update()
    {
        if (GameManager.Instance.Paused == false && GameManager.Instance.PlayerIsDead == false) {
            MovePlayer();
            Sprint();
            StartCoroutine(Shoot());
        }
    }

    private void MovePlayer()
    {
        if ((Controller.collisionFlags & CollisionFlags.Above) != 0) {
            PlayerVelocity.y -= 2;
        }

        // if we land - reset player velocity and reset jump counter
        if (Controller.isGrounded && PlayerVelocity.y < 0)
        {
            TimesJumped = 0;
            PlayerVelocity.y = 0f;
        }

        // receive input from Unity input manager
        Move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        // Add our move vector into the character controller move
        Controller.Move(Move * Time.deltaTime * PlayerSpeed);

        // Make player jump and increment jump counter
        if (Input.GetButtonDown("Jump") && TimesJumped < MaxJumps)
        {
            TimesJumped++;
            PlayerVelocity.y = JumpHeight;
        }

        // add gravity
        PlayerVelocity.y -= GravityValue * Time.deltaTime;

        // add gravity back into the character controller move
        Controller.Move(PlayerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        // on down press of 'Sprint' key, increase player speed
        if (Input.GetButtonDown("Sprint"))
        {
            IsSprinting = true;
            PlayerSpeed = PlayerSpeed * SprintMultiplyer;
        }
        // on release of 'sprint' ket, return player speed to normal
        else if (Input.GetButtonUp("Sprint") && IsSprinting)
        {
            IsSprinting = false;
            PlayerSpeed = OriginalSpeed;
        }
    }

    IEnumerator Shoot()
    {
        // get input for shooting
        if (Input.GetButton("Shoot") && CanShoot)
        {
            // turns shooting off so it cant be immediately executed again
            CanShoot = false;

            // Casts a ray from the player camera and performs an action where the ray hits
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out RaycastHit hit))
            {
                // play spark effect where ray hits
                Instantiate(HitEffectSpark, hit.point, HitEffectSpark.transform.rotation);

                // if the target is damageable, it takes damage
                if (hit.collider.GetComponent<IDamagable>() != null)
                {
                    // get target
                    IDamagable isDamageable = hit.collider.GetComponent<IDamagable>();

                    // check for body shot or head shot
                    if (hit.collider is SphereCollider) // apply damage for head shot
                    {
                        isDamageable.TakeDamage(10000);
                    }
                    else
                    {
                        isDamageable.TakeDamage(WeaponDamage); // apply damage for body shot
                    }
                }
            }

            MuzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            MuzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            MuzzleFlash.SetActive(false);

            // Timer runs, based off of player shootRate, and then re-enables shooting
            yield return new WaitForSeconds(ShootRate);
            CanShoot = true;
        }
    }

    public void TakeDamage(int dmg) // function for taking damage
    {
        Health.Current -= dmg; // apply damage
        StartCoroutine(FlashDamage());
        GameManager.Instance.UpdateHealthbar(Health);
        if (Health.Current <= 0) // if player health is 0 or lower, kill player
        {
            // kill player
            GameManager.Instance.PlayerDied();
        }
    }

    IEnumerator FlashDamage() {
        GameManager.Instance.PlayerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.PlayerDamageFlash.SetActive(false);
    }

    public void Heal(int amount) {
        Health.Current += amount;
        GameManager.Instance.UpdateHealthbar(Health);
    }

    public void Respawn() {
        Health.Current = Health.GetMax();
        Controller.enabled = false;
        transform.position = SpawnPosition;
        transform.rotation = SpawnRotation;
        Controller.enabled = true;
        GameManager.Instance.UpdateHealthbar(Health);
    }
}
