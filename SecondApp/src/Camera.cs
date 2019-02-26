using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondApp
{
    class Camera
    {

        public enum CameraMovement
        {
            FORWARD,
            BACKWARD,
            LEFT,
            RIGHT
        }

        Vector3 position;
        Vector3 front;
        Vector3 up;
        Vector3 right;
        Vector3 worldUp;
        
        float yaw;
        float pitch;
        float speed;
        float sensitivity;

        // vector init
        public Camera(Vector3 position, Vector3 up, Vector3 front, float yaw, float pitch, float speed, float sensitivity)
        {
            this.position = position;
            this.worldUp = up;
            this.front = front;
            this.yaw = yaw;
            this.pitch = pitch;
            this.speed = speed;
            this.sensitivity = sensitivity;
            updateCameraVectors();
        }

        // scalar init
        public Camera(float posX, float posY, float posZ, float upX, float upY, float upZ, float frontX, float frontY, float frontZ, float yaw, float pitch, float speed, float sensitivity)
        {
            this.position = new Vector3(posX, posY, posZ);
            this.worldUp = new Vector3(upX, upY, upZ);
            this.front = new Vector3(frontX, frontY, frontZ);
            this.yaw = yaw;
            this.pitch = pitch;
            this.speed = speed;
            this.sensitivity = sensitivity;
            updateCameraVectors();
        }


        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + front, up);
        }

        public void ProcessKeyboard(CameraMovement direction, float dt)
        {
            float velocity = speed * dt;

            if (direction == CameraMovement.FORWARD)
                position += front * velocity;
            if (direction == CameraMovement.BACKWARD)
                position -= front * velocity;
            if (direction == CameraMovement.LEFT)
                position -= right * velocity;
            if (direction == CameraMovement.RIGHT)
                position += right * velocity;
        }

        public void ProcessMouseMovement(float xOffset, float yOffset)
        {
            xOffset *= sensitivity;
            yOffset *= sensitivity;

            yaw += xOffset;
            pitch += yOffset;

            if (pitch > 89.9f)
                pitch = 89.9f;
            if (pitch < -89.9f)
                pitch = -89.9f;

            updateCameraVectors();
        }

        private void updateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 _front = new Vector3();
            _front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            _front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            _front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(_front);
            // Also re-calculate the Right and Up vector
            right = Vector3.Normalize(Vector3.Cross(front, worldUp));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }
    }
}
