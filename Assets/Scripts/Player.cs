using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] int _waterCanMax;
    [SerializeField] Grid _grid;
    [SerializeField] GameObject _soil;
    [SerializeField] GameObject _waterTrigger;
    [SerializeField] GameObject _highlight;
    [SerializeField] GameObject[] _plantPrefabs;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Rigidbody2D _rb;
    private Animator _playerAnim;

    private bool _isWatering;
    private Vector3 _facingDir;
    private int _money;
    private int _selectedSeed;
    private int _waterInCan;

    public bool IsWatering { get => _isWatering; set => _isWatering = value; }
    public enum Direction
    {
        Up, Down, Left, Right
    }
    public Dictionary<Direction, Vector3> dirToVector = new Dictionary<Direction, Vector3>();

    public static event Action<Sprite> OnSeedChanged;
    public static event Action<int> OnMoneyChanged;
    public static event Action<int, int> OnWaterInCanChanged;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _rb = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponentInChildren<Animator>();
        _facingDir = Vector3.left;
        _waterInCan = _waterCanMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posOnGrid = _grid.LocalToCell(transform.position);
        Vector3 highlightPos = posOnGrid + new Vector3(0.5f, 0.5f, 0) + _facingDir;
        _highlight.transform.position = highlightPos;
    }

    private void FixedUpdate()
    {
        if (_isWatering)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        Vector2 move = _moveAction.ReadValue<Vector2>();
        _playerAnim.SetFloat("X", move.x);
        _playerAnim.SetFloat("Y", move.y);

        if (move == Vector2.zero)
        {
            _playerAnim.SetTrigger("Idle");
            _rb.velocity = Vector2.zero;
            return;
        }


        if (Math.Abs(move.y) > Math.Abs(move.x))
        {
            if (move.y > 0)
                _facingDir = Vector3.up;
            else
                _facingDir = Vector3.down;
        }
        else
        {
            if (move.x > 0)
                _facingDir = Vector3.right;
            else
                _facingDir = Vector3.left;
        }

        //_rb.position += _moveSpeed * _moveAction.ReadValue<Vector2>() * Time.deltaTime;
        _rb.velocity = _moveSpeed * move;
    }



    public void Water(InputAction.CallbackContext context)
    {
        if (context.performed && !_isWatering)
        {
            List<Collider2D> colliders = CollidersOnHighlight();
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("WaterSource"))
                {
                    _waterInCan = _waterCanMax;
                    OnWaterInCanChanged?.Invoke(_waterInCan, _waterCanMax);
                    _isWatering = true;
                    _playerAnim.SetTrigger("Watering");

                    return;
                }
            }
            if (_waterInCan > 0)
            {
                _waterInCan--;
                OnWaterInCanChanged?.Invoke(_waterInCan, _waterCanMax);
                _isWatering = true;
                _playerAnim.SetTrigger("Watering");


                GameObject water = Instantiate(_waterTrigger, _highlight.transform.position, Quaternion.identity);
                Destroy(water, 0.5f);
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            List<Collider2D> colliders = CollidersOnHighlight();
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent<Plant>(out Plant plant))
                {
                    if (plant.FullyGrown)
                    {
                        _money += plant.Worth;
                        OnMoneyChanged?.Invoke(_money);
                        Destroy(collider.gameObject);
                        Debug.Log(_money);
                        return;
                    }
                    else if (plant.Ded)
                    {
                        Destroy(collider.gameObject);
                        return;
                    }
                }
            }
                
        }
    }

    public void Sow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Tilemap soilTilemap = _soil.GetComponent<Tilemap>();
            if (soilTilemap.HasTile(soilTilemap.LocalToCell(_highlight.transform.position)))
            {
                if (CollidersOnHighlight().Count == 0)
                    Instantiate(_plantPrefabs[_selectedSeed], _highlight.transform.position, Quaternion.identity);
            }
        }
    }

    public void SwitchSeed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _selectedSeed = _selectedSeed == _plantPrefabs.Length - 1 ? 0 : _selectedSeed + 1;
            OnSeedChanged?.Invoke(_plantPrefabs[_selectedSeed].GetComponent<Plant>().SeedIcon);
            Debug.Log(_selectedSeed);
        }
    }

    public List<Collider2D> CollidersOnHighlight()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_highlight.GetComponent<BoxCollider2D>(), new ContactFilter2D().NoFilter(), colliders);
        return colliders;
    }
}
