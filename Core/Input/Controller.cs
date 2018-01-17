using OpenTK.Input;
using System;
using OpenTK;
using System.Drawing;

namespace UMLProgram.Core.Input {
    public class Controller {
        public ControllerData Data;
        private bool[] pressed; 
        private Key[] controlledKeys = new Key[] { Key.Up, Key.Down, Key.Left, Key.Right, Key.Space };
        float speed = 3;
        float mouseSpeed = 0.035f;
        private double horizontalAngle = 3.14f;
        private double verticalAngle = 0;

        public Controller(int totalKeys) {
            pressed = new bool[totalKeys];
            Data = new ControllerData();
        }
        public Controller(int totalKeys,Vector3 position) {
            pressed = new bool[totalKeys];
            Data = new ControllerData();
            Data.Position = position;
        }
        //TODO: should be independant of controller (Demeter)
        // As this data is significant for Renderer's but the controller is not.
        public class ControllerData {
            public Vector3 Direction,Right;
            public int FOV=45;
            public Vector3 Position = new Vector3(0, 0, 0);
        }

        public void CalculateChanges(double deltaTime, Point mouse, int wheel, KeyboardState keys) {
            GetPressedKeys(keys);
            CalculateFieldOfView(wheel);
            CalculateAngles(deltaTime,mouse);
            Data.Direction = CalculateDirection();
            Data.Right = CalculateRight();
            CalculatePositionChanges(deltaTime);
        }
        private void CalculatePositionChanges(double deltaTime) {
            if (pressed[(int)Key.Up]) Data.Position += Vector3.Multiply(Data.Direction,   (float) deltaTime* speed);
            if (pressed[(int)Key.Down]) Data.Position -= Vector3.Multiply(Data.Direction, (float) deltaTime* speed);
            if (pressed[(int)Key.Right]) Data.Position += Vector3.Multiply(Data.Right,    (float) deltaTime* speed);
            if (pressed[(int)Key.Left]) Data.Position -= Vector3.Multiply(Data.Right,     (float) deltaTime* speed);
        }
        private void CalculateFieldOfView(int wheel) {
            Data.FOV = Data.FOV - 5 * wheel;
            if (Data.FOV < 30) Data.FOV = 30;
            if (Data.FOV > 120) Data.FOV = 120;
        }
        private void CalculateAngles(double deltaTime, Point mouse) {
            horizontalAngle += mouseSpeed * deltaTime * (1024 / 2 - mouse.X);
            verticalAngle += mouseSpeed * deltaTime * (768 / 2 - mouse.Y);
        }
        private Vector3 CalculateDirection() {
            return new Vector3((float)(Math.Cos(verticalAngle) * Math.Sin(horizontalAngle)),
                               (float)Math.Sin(verticalAngle),
                               (float)(Math.Cos(verticalAngle) * Math.Cos(horizontalAngle))); 
        }
        private Vector3 CalculateRight() {
            return new Vector3(
                (float)Math.Sin(horizontalAngle - 3.14f / 2),
                0,
                (float)Math.Cos(horizontalAngle - 3.14f / 2));
        }

        public bool[] GetPressedKeys(KeyboardState keys) {
            RegisterKeys(keys);
            return pressed;
        }
        private void RegisterKeys(KeyboardState keys) {
            foreach (Key k in controlledKeys) {
                RegisterKeyUp(keys, k);
                RegisterKeyDown(keys, k);
            }
        }
        private void RegisterKeyUp(KeyboardState keys, Key key) {
            if (keys.IsKeyUp(key)) pressed[(int)key] = false;
        }
        private void RegisterKeyDown(KeyboardState keys, Key key) {
            if (keys.IsKeyDown(key)) pressed[(int)key] = true;
        }

        
    }
}
