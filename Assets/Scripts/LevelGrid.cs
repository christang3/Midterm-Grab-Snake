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
using TMPro;

public class LevelGrid {

    private Vector2Int foodGridPosition;
    private Vector2Int DeliverGridPosition;
    private GameObject foodGameObject;
    private GameObject DeliveryObject;
    private int width;
    private int height;
    private Snake snake;
    private int FoodCount;
    private Timer timer;

    private int DeliverEveryAmountsFood = 10;

    public LevelGrid(int width, int height) {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake) {
        this.snake = snake;

        FoodCount = -1;
        SpawnFood();
    }

    public void Setup2(Timer timer) {
        this.timer = timer;
        timer.Setup(snake);
    }

    private void StartDelivery()
    {
        Debug.Log("Delivery Time");
        timer.StartTimer();
        DeliveryObject = new GameObject("Delivery", typeof(SpriteRenderer));
        DeliveryObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.DeliverySprite;
        DeliveryObject.transform.position = new Vector3(DeliverGridPosition.x, DeliverGridPosition.y);
    }


    private void SpawnFood() {
        do {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1 && DeliverGridPosition != foodGridPosition);
        FoodCount += 1;
        Debug.Log(FoodCount);
        if (FoodCount > 0 && FoodCount%DeliverEveryAmountsFood==0 && DeliveryObject == null)
        {
            do {
                DeliverGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            } while (snake.GetFullSnakeGridPositionList().IndexOf(DeliverGridPosition) != -1 && DeliverGridPosition != foodGridPosition);
            StartDelivery();
        }
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        Sprite RandomFood = GameAssets.i.FoodList[Random.Range(0,GameAssets.i.FoodList.Count)];
        foodGameObject.GetComponent<SpriteRenderer>().sprite = RandomFood;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    private bool CheckIfFoodNear(Vector2Int Pos)
    {
        float x = Mathf.Abs((Pos.x)-(foodGridPosition.x));
        float y = Mathf.Abs((Pos.y)-(foodGridPosition.y));
        if (x <= 1 && y <= 1)
        {
            return true;
        }
        return false;
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition) {
        if (snakeGridPosition == foodGridPosition) {
            Object.Destroy(foodGameObject);
            SpawnFood();
            timer.ReduceTimer(1f);
            Score.AddScore();
            return true;
        } else if (CheckIfFoodNear(snakeGridPosition))
        {
            snake.IsEating = true;
        }
        else {
            snake.IsEating = false;
        }
        return false;
    }

    public bool TrySnakeDeliverFood(Vector2Int snakeGridPosition) {
        if (snakeGridPosition == DeliverGridPosition) {
            Object.Destroy(DeliveryObject);
            timer.StopTimer();
            Score.AddScore();
            return true;
        } else {
            return false;
        }
    }

    public bool ValidateGridPosition(Vector2Int gridPosition) {
        bool Hit = false;
        if (gridPosition.x < 0) {
            gridPosition.x = width - 1;
            Hit = true;
        }
        if (gridPosition.x > width - 1) {
            gridPosition.x = 0;
            Hit = true;
        }
        if (gridPosition.y < 0) {
            gridPosition.y = height - 1;
            Hit = true;
        }
        if (gridPosition.y > height - 1) {
            gridPosition.y = 0;
            Hit = true;
        }
        return Hit;
        //return gridPosition;
    }
}
