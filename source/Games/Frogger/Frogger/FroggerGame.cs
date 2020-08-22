﻿using System;

namespace Frogger
{
    public partial class FroggerGame
    {
        //each lane has a velocity (float)
        //each lane shows one obstical type
        //safe or not safe

        public float[] LaneSpeeds { get; private set; } = new float[6] { 1.0f, -2.0f, 1.5f, -1.0f, 1.5f, -2.0f };
        public byte[,] LaneData { get; private set; } = new byte[6, 32]
        {
            //docks
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,3,0,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,0,0,1,3,0,0,0,1,3,0,0,1,2,3,0,0,1,3,0,1,3,0,0,1,3,0,0 },//logs
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,2,3,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,1,3,0,0,0,0,0,0,0,0,0,1,3,0,0,0,0,0,0,1,3,0,0,1,3,0,0 },//trucks
            {0,0,1,2,0,0,0,1,2,0,0,0,1,2,0,0,1,2,0,0,0,1,2,0,1,2,0,0,0,0,0,0 },//cars
            {1,2,3,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0 },//trucks
            //start
        };

        public double GameTime { get; private set; }
        public double TimeDelta => GameTime - lastTime;

        public byte LaneLength => 32;
        public byte Columns => 16;
        public byte Rows => 8;

        public byte FrogX { get; set; }
        public byte FrogY { get; private set; }

        public byte Lives { get; private set; }
        public byte FrogsHome { get; private set; }

        public byte CellSize { get; private set; }

        DateTime gameStart;

        public FroggerGame(byte cellsize = 8)
        {
            CellSize = cellsize;
            Reset();
        }

        void Reset()
        { 
            gameStart = DateTime.Now;
            ResetFrog();
            Lives = 3;

            FrogsHome = 0;
        }

        void ResetFrog()
        {
            FrogX = (byte)(Columns * CellSize / 2);
            FrogY = (byte)((Rows - 1) * CellSize);
        }

        double lastTime;
        public void Update()
        {
            lastTime = GameTime;
            GameTime = (DateTime.Now - gameStart).TotalSeconds;
        }

        public void OnUp()
        {
            if(FrogY >= CellSize) { FrogY -= CellSize; }

            if(FrogY == 0)
            {
                FrogsHome++;
                if(FrogsHome >= 5) { Reset(); }
                else { ResetFrog(); }
            }
        }

        public void OnDown()
        {
            if (FrogY < Rows*CellSize - CellSize) { FrogY += CellSize; }
        }

        public void OnLeft()
        {
            if (FrogX > CellSize) { FrogX -= CellSize; }
        }

        public void OnRight()
        {
            if (FrogX <= Columns*CellSize - CellSize) { FrogX += CellSize; }
        }

        void CheckHomeCollision()
        {

        }

        public void KillFrog()
        {
            ResetFrog();
        }
    }
}