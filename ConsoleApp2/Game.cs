using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace Snake
{
    public class Game
    {
        const int WindowSize = 32;
        const int InitialLength = 5;
        const int GameSpeed = 150;

        Random rand = new Random();
        Pixel head;
        Pixel berry;
        List<Pixel> body = new List<Pixel>();
        Direction currentMovement = Direction.Right;
        int score = InitialLength;
        bool isGameOver = false;

        public void Start()
        {
            InitializeGame();
            RunGame();
            ShowGameOver();
        }

        void InitializeGame()
        {
            WindowHeight = WindowSize;
            WindowWidth = WindowSize;
            CursorVisible = false;

            head = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            berry = CreateBerry();
        }

        void RunGame()
        {
            while (!isGameOver)
            {
                Render();
                HandleInput();
                Update();
            }
        }

        void Update()
        {
            MoveSnake();
            HandleWallCollision();
            HandleSelfCollision();
            HandleBerryCollision();
            UpdateBody();
        }

        void MoveSnake()
        {
            body.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));

            switch (currentMovement)
            {
                case Direction.Up: head.YPos--; break;
                case Direction.Down: head.YPos++; break;
                case Direction.Left: head.XPos--; break;
                case Direction.Right: head.XPos++; break;
            }
        }

        void HandleWallCollision()
        {
            if (head.XPos == 0 || head.XPos == WindowWidth - 1 ||
                head.YPos == 0 || head.YPos == WindowHeight - 1)
            {
                isGameOver = true;
            }
        }

        void HandleSelfCollision()
        {
            foreach (var segment in body)
            {
                if (segment.XPos == head.XPos && segment.YPos == head.YPos)
                {
                    isGameOver = true;
                }
            }
        }

        void HandleBerryCollision()
        {
            if (head.XPos == berry.XPos && head.YPos == berry.YPos)
            {
                score++;
                berry = CreateBerry();
            }
        }

        void UpdateBody()
        {
            if (body.Count > score)
            {
                body.RemoveAt(0);
            }
        }

        void HandleInput()
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds <= GameSpeed)
            {
                currentMovement = ReadMovement(currentMovement);
            }
        }

        void Render()
        {
            Clear();
            DrawBorder();
            DrawPixel(berry);

            foreach (var segment in body)
                DrawPixel(segment);

            DrawPixel(head);
        }

        Pixel CreateBerry()
        {
            return new Pixel(
                rand.Next(1, WindowWidth - 2),
                rand.Next(1, WindowHeight - 2),
                ConsoleColor.Cyan);
        }

        void ShowGameOver()
        {
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            ForegroundColor = ConsoleColor.White;
            WriteLine($"Game over, Score: {score - InitialLength}");
            ReadKey();
        }

        Direction ReadMovement(Direction movement)
        {
            if (!KeyAvailable) return movement;

            var key = ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && movement != Direction.Down)
                return Direction.Up;

            if (key == ConsoleKey.DownArrow && movement != Direction.Up)
                return Direction.Down;

            if (key == ConsoleKey.LeftArrow && movement != Direction.Right)
                return Direction.Left;

            if (key == ConsoleKey.RightArrow && movement != Direction.Left)
                return Direction.Right;

            return movement;
        }

        void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Write("■");
            SetCursorPosition(0, 0);
        }

        void DrawBorder()
        {
            ForegroundColor = ConsoleColor.Cyan;

            for (int i = 0; i < WindowWidth; i++)
            {
                SetCursorPosition(i, 0);
                Write("■");
                SetCursorPosition(i, WindowHeight - 1);
                Write("■");
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("■");
                SetCursorPosition(WindowWidth - 1, i);
                Write("■");
            }
        }
    }
}