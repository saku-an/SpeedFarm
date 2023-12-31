using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public static List<Transform> GrowingPlants = new List<Transform>();

    public Sprite SeedIcon;
    [SerializeField] private Sprite[] _growthStages;
    [SerializeField] private float _growthTime;
    [SerializeField] private float _waterTime;
    [SerializeField] private float _deathTime;
    [SerializeField] private int _worth;
    private int _currentStage;
    private SpriteRenderer _spriteRenderer;
    private bool _isWatered;
    private bool _ded;
    private bool _growthStarted;
    private Coroutine _growthTimer;
    private Coroutine _waterTimer;
    private Coroutine _deathTimer;

    public bool FullyGrown { get => _currentStage == _growthStages.Length - 1; }
    public int Worth { get => _worth; }
    public bool Ded { get => _ded; }

    public static event Action<Vector3> OnWatered;
    public static event Action<Vector3> OnNotWatered;
    public static event Action<Transform> OnPlantDestroyed;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when plant gets watered
        if (!_ded && !FullyGrown && collision.CompareTag("WaterTrigger"))
        {
            if (!_isWatered)
                OnWatered?.Invoke(transform.position);

            _isWatered = true;
            if (_deathTimer != null)
                StopCoroutine(_deathTimer);
            if (_waterTimer != null)
                StopCoroutine(_waterTimer);
            _waterTimer = StartCoroutine(WaterTimer());
            if (!_growthStarted)
            {
                _growthStarted = true;
                _growthTimer = StartCoroutine(GrowthTimer());
            }
            Debug.Log("Watered");
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // Plant grows as long as it is watered
    private IEnumerator GrowthTimer()
    {
        for (int i  = 0; i < _growthStages.Length-1; i++)
        {
            yield return new WaitForSeconds(_growthTime);
            while (!_isWatered)
                yield return new WaitForSeconds(0.5f);
            Grow();
        }
        if (_deathTimer != null)
            StopCoroutine(_deathTimer);
        if (_waterTimer != null)
            StopCoroutine(_waterTimer);
        OnNotWatered?.Invoke(transform.position);
    }

    // How long plant stays watered
    private IEnumerator WaterTimer()
    {
        yield return new WaitForSeconds(_waterTime);
        OnNotWatered?.Invoke(transform.position);
        _isWatered = false;
        _deathTimer = StartCoroutine(DeathTimer());
    }

    // How long after not being watered the plant dies
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(_deathTime);
        Die();
    }

    private void Grow()
    {
        _currentStage = Mathf.Min(_currentStage + 1, _growthStages.Length - 1);
        _spriteRenderer.sprite = _growthStages[_currentStage];
        if (_currentStage == 1)
            GrowingPlants.Add(this.transform);
        Debug.Log("Stage: " + _currentStage);
    }

    private void Die()
    {
        StopCoroutine(_growthTimer);
        _spriteRenderer.color = new Color(70/255f, 70/255f, 70/255f);
        _ded = true;
        Debug.Log("ded");
    }

    private void OnDestroy()
    {
        GrowingPlants.Remove(this.transform);
        OnPlantDestroyed?.Invoke(this.transform);
    }
}
