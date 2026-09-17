using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerCont : MonoBehaviour
{

    [SerializeField] private InputManager inputManager;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    private int _health=10;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject gameOverBackground;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private ProjectTileCont projecttilePref;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayer;
    private List<ProjectTileCont> projectile = new List<ProjectTileCont>();
    private int projectileSize = 20;
    private int projectileIndex = 0;
   public bool isAlive=>_health>0;
    public int Hp => _health;
    private float cordz;
    private float cordx;
    [SerializeField] private Transform _ShutPoint;
    private float _shutRange;
    private Vector3 _moveVector;
    [SerializeField] float cameraRotationSpeed = 3f;
    [SerializeField] float cameraReturnSpeed = 3f;
    private float cameraYawSpeed;
    private float cameraPitchSpeed;
    private float cameraDefalutRotation;
    private float cameraDefalutRotation2;

   

    [SerializeField] private float minCameraPitch = -70f;
    [SerializeField] private float maxCameraPitch = 70f;


    private float lookDelta;




   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {
        healthBar.maxValue = _health;
        healthBar.value = _health;
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage отримав: " + damage);



        _health -= damage;
        healthBar.value = _health;

        Debug.Log("HP гравця після удару: " + _health);
        if (_health <= 0)
        {
            _anim.SetTrigger("Death");

            gameOverBackground.SetActive(true);
            gameOverText.gameObject.SetActive(true);
            restartButton.SetActive(true);

            EnemiesVictory();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isAlive)
            return;
        Move();
        RotatePlayer();
        //RotateShutPint();
        CheckGround();
        RotateCamera();
    }
    private void OnEnable()
    {
        InputManager.OnSpacePressed += OnSpacePress;
        InputManager.OnFPressed += OnFPress;
        InputManager.OnMovementPressed += ReadMoveInput;
        InputManager.OnLookPressed += ReadLookInput;
        InputManager.OnLeftMouseButtonPressed += OnLeftMouseButtonPress;
        for(int i=0; i<projectileSize; i++)
        {
            
            var projectille = Instantiate(projecttilePref, _ShutPoint.position, Quaternion.identity);
            projectille.gameObject.SetActive(false);
            projectile.Add(projectille);
            
        }
       
    }
    private void OnDisable()
    {
        InputManager.OnSpacePressed -= OnSpacePress;
        InputManager.OnFPressed -= OnFPress;
        InputManager.OnMovementPressed -= ReadMoveInput;
        InputManager.OnLeftMouseButtonPressed -= OnLeftMouseButtonPress;
        InputManager.OnLookPressed -= ReadLookInput;
    }
    private void OnSpacePress()
    {
        if (!isAlive)
            return;

        _anim.SetTrigger("Jump");

        _rb.linearVelocity = new Vector3(
            _rb.linearVelocity.x,
            jumpForce,
            _rb.linearVelocity.z
        );
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    isGrounded = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isGrounded = false;
    //}

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    private void OnFPress()
    {
        Debug.Log("F");
    }
    private void OnLeftMouseButtonPress()
    {
        if (!isAlive)
            return;
        _anim.SetTrigger("PlayerAttack");
        LaunchProjectile();
    }
    private void ReadMoveInput(Vector2 inputVector)
    {
        cordx = inputVector.x;
        cordz = inputVector.y;
       
    }
    private void ReadLookInput(Vector2 inputVector)
    {
        Debug.Log("LOOK: " + inputVector);
        cameraYawSpeed = inputVector.x;
        cameraPitchSpeed = inputVector.y;
    }


    private void Move()
    {
        _moveVector = transform.forward * cordz + transform.right * cordx;

        if (_moveVector.magnitude > 1f)
        {
            _moveVector.Normalize();
        }

        _anim.SetFloat("Speed", _moveVector.magnitude);
        //Debug.Log("Speed: " + _moveVector.magnitude);
        _moveVector *= _speed * Time.deltaTime;
        _rb.MovePosition(_moveVector + _rb.position);
    }
    private void RotatePlayer()
    {
        transform.Rotate(
            Vector3.up,
            cameraYawSpeed * _rotationSpeed * Time.deltaTime
        );
    }
    //private void RotateShutPint()
    //{
    //    cameraPivot.Rotate(Vector3.up * lookDelta * _rotationSpeed * Time.deltaTime);
    //}
    private void RotateCamera()
    {
        cameraDefalutRotation +=
            cameraPitchSpeed * cameraRotationSpeed * Time.deltaTime;

        cameraDefalutRotation =
            Mathf.Clamp(
                cameraDefalutRotation,
                minCameraPitch,
                maxCameraPitch
            );

        cameraPivot.localRotation =
            Quaternion.Euler(cameraDefalutRotation, 0f, 0f);
    }

    private void LaunchProjectile()
    {
        projectile[projectileIndex].transform.position = _ShutPoint.position;
        projectile[projectileIndex].gameObject.SetActive(true);

        Vector3 direction = _ShutPoint.forward;
        direction.y = 0f;
        direction.Normalize();

        projectile[projectileIndex].Initialized(1, direction);

        projectileIndex = (projectileIndex + 1) % projectile.Count;
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    private void EnemiesVictory()
    {
        Animator[] animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);

        foreach (Animator animator in animators)
        {
            if (animator != _anim)
            {
                animator.SetTrigger("Victory");
            }
        }
    }
}
