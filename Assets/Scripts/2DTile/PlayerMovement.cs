using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Stage stage;
    private Animator animator;
    private int currentTileId;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        var findGo = GameObject.FindWithTag("Map");
        stage = findGo.GetComponent<Stage>();
    }

    private void Update()
    {

        var direction = Sides.None;
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Sides.Top;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Sides.Bottom;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Sides.Right;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Sides.Left;
        }

        if (direction != Sides.None)
        {
            var targetTile = stage.Map.tiles[currentTileId].adjacents[(int)direction];
            if (targetTile != null && targetTile.CanMove)
            {
                MoveTo(targetTile.id);
            }

        }

        
    }

    public void MoveTo (int tileId)
    {
        currentTileId = tileId;
        transform.position = stage.GetTilePos(currentTileId);
    }

}
