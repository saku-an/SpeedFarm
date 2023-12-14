using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] public float MoveSpeed;
    [SerializeField] int _waterCanMax;
    [SerializeField] Grid _grid;
    [SerializeField] GameObject _soil;
    [SerializeField] GameObject _waterTrigger;
    [SerializeField] GameObject _highlight;
    [SerializeField] GameObject[] _plantPrefabs;
    [HideInInspector]public InputAction MoveAction;
    [HideInInspector] public Rigidbody2D Rb;
    [HideInInspector] public Animator PlayerAnim;
    [HideInInspector] public Vector3 FacingDir;

    private PlayerInput _playerInput;
    [HideInInspector] public IPlayerState CurrentState;
    [HideInInspector] public PlayerIdleState IdleState;
    [HideInInspector] public PlayerAttackState AttackState;
    [HideInInspector] public PlayerWateringState WateringState;

    private bool _isWatering;
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
        MoveAction = _playerInput.actions["Move"];
        Rb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponentInChildren<Animator>();
        FacingDir = Vector3.left;
        _waterInCan = _waterCanMax;

        IdleState = new PlayerIdleState(this);
        AttackState = new PlayerAttackState(this);
        WateringState = new PlayerWateringState(this);
        CurrentState = IdleState;
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        // Highlights the tile in front of the player
        Vector3 posOnGrid = _grid.LocalToCell(transform.position);
        Vector3 highlightPos = posOnGrid + new Vector3(0.5f, 0.5f, 0) + FacingDir;
        _highlight.transform.position = highlightPos;
    }

    private void FixedUpdate()
    {
        CurrentState.UpdateState();
    }


    // Player using watering can
    // Either depletes the water or fills the can, if there is a water source nearby
    public void Water(InputAction.CallbackContext context)
    {
        if (context.performed && !_isWatering)
        {
            CurrentState.Water();
            List<Collider2D> colliders = CollidersOnHighlight();
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("WaterSource"))
                {
                    _waterInCan = _waterCanMax;
                    OnWaterInCanChanged?.Invoke(_waterInCan, _waterCanMax);
                    _isWatering = true;
                    PlayerAnim.SetTrigger("Watering");

                    return;
                }
            }
            if (_waterInCan > 0)
            {
                _waterInCan--;
                OnWaterInCanChanged?.Invoke(_waterInCan, _waterCanMax);
                _isWatering = true;
                PlayerAnim.SetTrigger("Watering");


                GameObject water = Instantiate(_waterTrigger, _highlight.transform.position, Quaternion.identity);
                Destroy(water, 0.5f);
            }
        }
    }

    // Player interacting with the world
    // Picks up a grown crop or
    // destroys a dead crop
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

    // Player sowing
    // Plants a seed if there isn't a crop already
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

    // Selects the next available seed which is used in sowing
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
