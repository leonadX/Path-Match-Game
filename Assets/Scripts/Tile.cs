using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
	public int x;
	public int y;

	public Image icon;

	private Item _item;
	
	[SerializeField]
	private AudioClip failSFX;
	
	[SerializeField]
	private AudioSource failAudioSource;

	public Item item
    {
		get => _item;

        set
        {
			if (_item == value) return;

			_item = value;

			icon.sprite = _item.sprite;
        }
    }

	public Button button;

	public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
	public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
	public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
	public Tile Bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;

    public Tile[] Neighbours => new[]
    {
		Left,
		Top,
		Right,
		Bottom,
	};

    private void Start()
    {
		button.onClick.AddListener(() => Board.Instance.Select(this));
    }

	public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
		var result = new List<Tile> { this,};

		if(exclude == null)
        {
			exclude = new List<Tile> { this, };
        }
		else
        {
			exclude.Add(this);
        }

		foreach(var neighbour in Neighbours)
        {
			if(neighbour == null || exclude.Contains(neighbour) || neighbour.item != item) continue;

			// CheckMatchNeighbours();

			// if (neighbour == null || exclude.Contains(neighbour) || neighbour.item != item)
			// {
			// 	// failAudioSource.PlayOneShot(failSFX);
			// 	continue;
			// }
			// else
			// {
			// 	failAudioSource.PlayOneShot(failSFX);
			// }
			
			result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

		return result;
    }

	// Check if neighbours of selected tiles are same, if 3 or more match do nothing, if less than 3 matches, play fail sound
	// private void CheckMatchNeighbours()
	// {
	// 	var connectedTiles = GetConnectedTiles();

	// 	if(connectedTiles.Count < 3)
	// 	{
	// 		failAudioSource.PlayOneShot(failSFX);
	// 		return;
	// 	}
	// 	else
	// 	{
	// 		return;
	// 	}
	// }
}

/**
public void CheckMatchNeighbours()
	{
		if(Board.Instance._selection.Count < 2) return;

		foreach(var neighbour in Neighbours)
		{
			if(neighbour == null || neighbour.item != item) continue;

			failAudioSource.PlayOneShot(failSFX);
		}
	}

*/
