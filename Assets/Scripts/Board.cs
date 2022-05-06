using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


public sealed class Board : MonoBehaviour
{
	public static Board Instance { get; private set; }

	// Audio
	[SerializeField]
	private AudioClip popSFX;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField] private Row[] rows;

	public Tile[,] Tiles { get; private set; }

	public int Width => Tiles.GetLength(0);
	public int Height => Tiles.GetLength(1);

	private readonly List<Tile> _selection = new List<Tile>();

	// Animation Duration
	private const float TweenDuration = 0.25f;

	private void Awake() => Instance = this;

    private void Start()
    {
		Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
		
		for(var y = 0; y < Height; y++)
        {
			for(var x = 0; x < Width; x++)
            {
				var tile = rows[y].tiles[x];

				tile.x = x;
				tile.y = y;

				tile.item = ItemDatabase.items[Random.Range(0, ItemDatabase.items.Length)]; //TODO? Look at this

				Tiles[x, y] = tile;

			}
        }

		// Check and Pop when game 1st Starts
		Pop();
	}

    private void Update()
    {
		/*if (!Input.GetKeyDown(KeyCode.A)) return;

		foreach(var connectedTile in Tiles[0, 0].GetConnectedTiles())
        {
			connectedTile.icon.transform.DOScale(1.25f, TweenDuration).Play();
        }*/
    }

    public async void Select(Tile tile)
	{
		if(!_selection.Contains(tile))
        {
			if(_selection.Count > 0)
            {
				if(Array.IndexOf(_selection[0].Neighbours, tile) != -1)
                {
					_selection.Add(tile);
                }
            }
            else
            {
				_selection.Add(tile);
            }
        }

		//if(!_selection.Contains(tile))
        //{
		//	_selection.Add(tile);
		//}

		if (_selection.Count < 2) return;

		Debug.Log($"Selected tiles at ({_selection[0].x}, {_selection[0].y}) and ({_selection[1].x}, {_selection[1].y})");

		await Swap(_selection[0], _selection[1]);

		if(CanPop())
        {
			Pop();
        }
        else
        {
			await Swap(_selection[0], _selection[1]);
		}

		_selection.Clear();
	}

	public async Task Swap(Tile tile1, Tile tile2)
    {
		var icon1 = tile1.icon;
		var icon2 = tile2.icon;

		var icon1Transform = icon1.transform;
		var icon2Transform = icon2.transform;

		icon1Transform.SetAsLastSibling();
		icon2Transform.SetAsLastSibling();

		// Using Dotween Animation
		var sequence = DOTween.Sequence();

		sequence
			.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration).SetEase(Ease.OutBack))
			.Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration).SetEase(Ease.OutBack));

		await sequence.Play().AsyncWaitForCompletion();

		icon1Transform.SetParent(tile2.transform);
		icon2Transform.SetParent(tile1.transform);

		var tileItem = tile1.item; // TODO ! Correct this...

		//var tile1Item = tile1.Type;
		//tile1.Type = tile2.Type;
		//tile2.Type = tile1Item;

		tile1.item = tile2.item;
		tile2.item = tile1.item;
	}

	private bool CanPop()
    {
		for(var y = 0; y < Height; y++)
        {
			for(var x = 0; x < Width; x++)
            {
				if(Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
                {
					return true;
				}
            }
        }

		return false;
    }

	private async void Pop()
	{
		for(var y = 0; y < Height; y++)
        {
			for(var x = 0; x < Width; x++)
            {
				var tile = Tiles[x, y];

				var connectedTiles = tile.GetConnectedTiles();

				if (connectedTiles.Skip(1).Count() < 2) continue;

				var deflateSequence = DOTween.Sequence();

				foreach(var connectedTile in connectedTiles)
                {
					deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                }

				await deflateSequence.Play().AsyncWaitForCompletion();

				// Play pop sound
				audioSource.PlayOneShot(popSFX);

				// Give Score
				Score.Instance.ScorePoints += tile.item.value * connectedTiles.Count;

				var inflateSequence = DOTween.Sequence();

				foreach(var connectedTile in connectedTiles)
                {
					connectedTile.item = ItemDatabase.items[Random.Range(0, ItemDatabase.items.Length)];

					inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

				await inflateSequence.Play().AsyncWaitForCompletion();

				x = 0;
				y = 0;
			}
		}
	}
}

