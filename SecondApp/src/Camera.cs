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

        public Vector3 position;
        Vector3 front;
        Vector3 up;
        Vector3 right;
        Vector3 worldUp;
        
        float yaw;
        float pitch;
        float speed;
        float sensitivity;
        public float fov;

        // vector init
        public Camera(Vector3 position, Vector3 worldUp, float yaw, float pitch, float fov, float speed, float sensitivity)
        {
            this.position = position;
            this.worldUp = worldUp;
            this.yaw = yaw;
            this.pitch = pitch;
            this.fov = fov;
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

        public void ProcessMouseMovement(float xOffset, float yOffset, float scrollOffset)
        {
            if (fov >= 1.0f && fov <= 45.0f)
                fov -= scrollOffset;
            if (fov <= 1.0f)
                fov = 1.0f;
            if (fov >= 45.0f)
                fov = 45.0f;

            xOffset *= sensitivity;
            yOffset *= sensitivity;

            float fovCompensation = (fov / 45.0f);
            yaw += xOffset * fovCompensation;
            pitch += yOffset * fovCompensation;

            if (pitch > 89.999f)
                pitch = 89.999f;
            if (pitch < -89.999f)
                pitch = -89.999f;

            updateCameraVectors();
        }

        public void updateCameraVectors()
        {
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, worldUp));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }
    }
}
