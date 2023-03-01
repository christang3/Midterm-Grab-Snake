/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using DG.Tweening;

public class Snake : MonoBehaviour {

    private enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    private enum State { 
        Alive,
        Dead
    }

    private State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private Direction CurrentDirection;
    private SpriteRenderer SnakeHead;

    private bool TimeUp;

    public bool IsEating;

    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
    }

    private void Awake() {
        SnakeHead = gameObject.GetComponent<SpriteRenderer>();
        gridPosition = new Vector2Int(0, 8);
        gridMoveTimerMax = .2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        CurrentDirection = gridMoveDirection;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;
        snakeBodySize++;
        CreateSnakeBodyPart();
    }

    private void Update() {
        switch (state) {
        case State.Alive:
            if (IsEating)
            {
                SnakeHead.sprite = GameAssets.i.SnakeEatSprite;
            } else
            {
                SnakeHead.sprite = GameAssets.i.snakeHeadSprite;
            }
            HandleInput();
            HandleGridMovement();
            break;
        case State.Dead:
            break;
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (CurrentDirection != Direction.Down) {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (CurrentDirection != Direction.Up) {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (CurrentDirection != Direction.Right) {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (CurrentDirection != Direction.Left) {
                gridMoveDirection = Direction.Right;
            }
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            //SoundManager.PlaySound(SoundManager.Sound.SnakeMove);

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0) {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            CurrentDirection = gridMoveDirection;
            switch (gridMoveDirection) {
            default:
            case Direction.Right:   gridMoveDirectionVector = new Vector2Int(+1, 0); break;
            case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1, 0); break;
            case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0, +1); break;
            case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

            bool hit = levelGrid.ValidateGridPosition(gridPosition); //gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            bool Delivered = levelGrid.TrySnakeDeliverFood(gridPosition);
            if (Delivered) {
                SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
            }
            if (snakeAteFood) {
                // Snake ate food, grow body
                snakeBodySize++;
                CreateSnakeBodyPart();
                SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1) {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition || hit || TimeUp) {
                    // Game Over!
                    //CMDebug.TextPopup("DEAD!", transform.position);
                    state = State.Dead;
                    GameHandler.SnakeDied();
                    SoundManager.PlaySound(SoundManager.Sound.SnakeDie);
                }
            }
            
            transform.DOMove(new Vector3(gridPosition.x, gridPosition.y),.1f).SetEase(Ease.OutQuad);
            //transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }

    private void CreateSnakeBodyPart() {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts() {
        for (int i = 0; i < snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].UpdateSnakeBod(snakeBodyPartList.Count);
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    public void TimeToDie()
    {
        TimeUp = true;
    }


    private float GetAngleFromVector(Vector2Int dir) {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition() {
        return gridPosition;
    }

    // Return the full list of positions occupied by the snake: Head + Body
    public List<Vector2Int> GetFullSnakeGridPositionList() {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList) {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }




    /*
     * Handles a Single Snake Body Part
     * */
    private class SnakeBodyPart {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        private GameObject obj;
        private SpriteRenderer CurSprite;
        private int PositionInList;

        public SnakeBodyPart(int bodyIndex) {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            transform = snakeBodyGameObject.transform;
            obj = snakeBodyGameObject;
            CurSprite = snakeBodyGameObject.GetComponent<SpriteRenderer>();
            switch (bodyIndex) {
                default: 
                    snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
                    break;
                case 0: // Previously was going Left
                    snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.SnakeNeckSprite;
                    break;
            }
            CurSprite.sortingOrder = -1 - bodyIndex;
            PositionInList = bodyIndex;
        }

        public void UpdateSnakeBod(int Max)
        {
            if (Max-1==PositionInList)
            {
                CurSprite.sprite = GameAssets.i.SnakeTailSprite;
            } else if (PositionInList == 0)
            {
                CurSprite.sprite = GameAssets.i.SnakeNeckSprite;
            } else
            {
                CurSprite.sprite = GameAssets.i.snakeBodySprite;
            }
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition) {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            //transform.DOMove(new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y),.1f);

            float angle;
            switch (snakeMovePosition.GetDirection()) {
            default:
            case Direction.Up: // Currently going Up
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 0; 
                    break;
                case Direction.Left: // Previously was going Left
                    angle = 0 + 45; 
                    //transform.DOMove(transform.position+new Vector3(.2f, .2f),.1f);
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Right: // Previously was going Right
                    angle = 0 - 45; 
                    //transform.DOMove(transform.position+new Vector3(-.2f, .2f),.1f);
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                }
                break;
            case Direction.Down: // Currently going Down
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 180; 
                    break;
                case Direction.Left: // Previously was going Left
                    angle = 180 - 45;
                    //transform.DOMove(transform.position+new Vector3(.2f, -.2f),.1f);
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                case Direction.Right: // Previously was going Right
                    angle = 180 + 45;
                    //transform.DOMove(transform.position+new Vector3(-.2f, -.2f),.1f);
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Left: // Currently going to the Left
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = +90; 
                    break;
                case Direction.Down: // Previously was going Down
                    angle = 180 - 45; 
                    //transform.DOMove(transform.position+new Vector3(-.2f, .2f),.1f);
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                case Direction.Up: // Previously was going Up
                    angle = 45; 
                    //transform.DOMove(transform.position+new Vector3(-.2f, -.2f),.1f);
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Right: // Currently going to the Right
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = -90; 
                    break;
                case Direction.Down: // Previously was going Down
                    angle = 180 + 45; 
                    //transform.DOMove(transform.position+new Vector3(.2f, .2f),.1f);
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Up: // Previously was going Up
                    angle = -45; 
                    //transform.DOMove(transform.position+new Vector3(.2f, -.2f),.1f);
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                }
                break;
            }

            //Debug.Log(angle);
            /*
            if (Mathf.Abs(((angle-1)%45)+1) == 45 && CurSprite.sprite != GameAssets.i.SnakeTailSprite)
                {
                    CurSprite.sprite = GameAssets.i.SnakeTurnSprite;
                }
            */
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition() {
            return snakeMovePosition.GetGridPosition();
        }
    }



    /*
     * Handles one Move Position from the Snake
     * */
    private class SnakeMovePosition {

        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction) {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition() {
            return gridPosition;
        }

        public Direction GetDirection() {
            return direction;
        }

        public Direction GetPreviousDirection() {
            if (previousSnakeMovePosition == null) {
                return Direction.Right;
            } else {
                return previousSnakeMovePosition.direction;
            }
        }

    }

}
