using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_Ideas_and_Monogame_trials.Primitives
{
    class DrawLine
    {
        // private GraphicsDevice graphicsDevice;
        private CameraTransforms cameraTransforms;
        private BasicEffect basicEffect;

        public DrawLine(GraphicsDevice graphicsDevice, CameraTransforms cameraTransforms)
        {
            // this.graphicsDevice = graphicsDevice;
            this.cameraTransforms = cameraTransforms;
            this.basicEffect = new BasicEffect(graphicsDevice);

            // -- enable per-polygon vertex colors
            basicEffect.VertexColorEnabled = true;
        }

        public void DrawLinePrimitive(GraphicsDevice graphicsDevice, Vector3[] vertices, Color color)
        {
            VertexPositionColor[] vertexList = new VertexPositionColor[2];
            vertexList[0] = new VertexPositionColor(vertices[0], color);
            vertexList[1] = new VertexPositionColor(vertices[1], color);

            //VertexPositionColor[] vertexList = new VertexPositionColor[3];
            //vertexList[0] = new VertexPositionColor(vertices[0], color);
            //vertexList[1] = new VertexPositionColor(vertices[1], color);
            //vertexList[2] = new VertexPositionColor(vertices[2], color);
            basicEffect.CurrentTechnique.Passes[0].Apply();
            ApplyCameraTransform();
            //graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertexList, 0, 1);

            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertexList, 0, 1);

            // GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 8, 0, 7);
        }

        private void ApplyCameraTransform()
        {
            basicEffect.World = cameraTransforms.GetWorldMatrix();
            basicEffect.View = cameraTransforms.GetViewMatrix();
            basicEffect.Projection = cameraTransforms.GetProjectionMatrix();
        }

        public void DrawTestLine(GraphicsDevice graphicsDevice)
        {
            Vector3[] vertices = new Vector3[2];
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(0, 2f, 0);
            DrawLinePrimitive(graphicsDevice, vertices, Color.Blue);
        }

        public void DrawAxis(GraphicsDevice graphicsDevice)
        {
            Vector3[] positiveX = new Vector3[2];
            positiveX[0] = new Vector3(0, 0, 0);
            positiveX[1] = new Vector3(2f, 0, 0);
            DrawLinePrimitive(graphicsDevice, positiveX, Color.Red);
            Vector3[] negativeX = new Vector3[2];
            negativeX[0] = new Vector3(0, 0, 0);
            negativeX[1] = new Vector3(-2f, 0, 0);
            DrawLinePrimitive(graphicsDevice, negativeX, Color.Black);

            Vector3[] positiveY = new Vector3[2];
            positiveY[0] = new Vector3(0, 0, 0);
            positiveY[1] = new Vector3(0, 2f, 0);
            DrawLinePrimitive(graphicsDevice, positiveY, Color.Green);
            Vector3[] negativeY = new Vector3[2];
            negativeY[0] = new Vector3(0, 0, 0);
            negativeY[1] = new Vector3(0, -2f, 0);
            DrawLinePrimitive(graphicsDevice, negativeY, Color.Black);

            Vector3[] positiveZ = new Vector3[2];
            positiveZ[0] = new Vector3(0, 0, 0);
            positiveZ[1] = new Vector3(0, 0, 2f);
            DrawLinePrimitive(graphicsDevice, positiveZ, Color.Blue);
            Vector3[] negativeZ = new Vector3[2];
            negativeZ[0] = new Vector3(0, 0, 0);
            negativeZ[1] = new Vector3(0, 0, -2f);
            DrawLinePrimitive(graphicsDevice, negativeZ, Color.Black);

        }




    }
}

