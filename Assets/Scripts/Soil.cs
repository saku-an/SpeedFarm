using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Soil : MonoBehaviour
{
    private Tilemap _tilemap;

    private void Awake()
    {
        Plant.OnWatered += OnPlantWatered;
        Plant.OnNotWatered += OnPlantNotWatered;
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnDestroy()
    {
        Plant.OnWatered -= OnPlantWatered;
        Plant.OnNotWatered -= OnPlantNotWatered;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlantWatered(Vector3 plantPos)
    {
        Vector3Int tilePos = _tilemap.LocalToCell(plantPos);
        _tilemap.SetTileFlags(tilePos, TileFlags.None);
        _tilemap.SetColor(tilePos, new Color(200/255f, 200/255f, 200/255f));
    }

    private void OnPlantNotWatered(Vector3 plantPos)
    {
        Vector3Int tilePos = _tilemap.LocalToCell(plantPos);
        _tilemap.SetTileFlags(tilePos, TileFlags.None);
        _tilemap.SetColor(tilePos, Color.white);
    }
}
