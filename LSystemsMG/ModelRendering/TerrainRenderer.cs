using System.Diagnostics;
using LSystemsMG.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LSystemsMG.ModelRendering
{
    class TerrainRenderer
    {
        private const int TERRAIN_TILES = 5;
        private int TERRAIN_SIDE = 50;
        private Model[] terrainModels = new Model[TERRAIN_TILES];
        CameraTransforms cameraTransforms;

        public TerrainRenderer(ContentManager Content, CameraTransforms cameraTransforms) {
            this.cameraTransforms= cameraTransforms;
            for (int i = 0; i < TERRAIN_TILES; i++)
            {
                terrainModels[i] = Content.Load<Model>($"terrain-tiles/terrain{i:000}");
            }
        }


        bool[] tileAssigned = new bool[10201];
        int[] randomSelected = new int[10201];

        public void DrawRandom(int tX, int tY)
        {
            int ordinal = 101 * (tX + 50) + (tY + 50);
            if (!tileAssigned[ordinal])
            {
                int roll = RandomNum.GetRandomInt(0, 100);
                if (tX*tX+tY*tY>=6 && roll > 90)
                {
                    randomSelected[ordinal] = RandomNum.GetRandomInt(1, 4);
                } else
                {
                    randomSelected[ordinal] = 0;
                }
                tileAssigned[ordinal] = true;
            }
            Draw(randomSelected[ordinal], tX, tY);
        }

        public void Draw(int terrainId, int tX, int tY)
        {
            foreach (ModelMesh mesh in terrainModels[terrainId].Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    BasicEffect basicEffect = (BasicEffect) effect;
                    basicEffect.EnableDefaultLighting();
                    basicEffect.AmbientLightColor = new Vector3(0.3f, 0.4f, 0.4f);
                    basicEffect.DirectionalLight0.Enabled = true;
                    basicEffect.DirectionalLight0.Direction = new Vector3(0.8f, 0.8f, -1);
                    basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.4f, 0.4f);
                    basicEffect.World = cameraTransforms.worldMatrix;
                    basicEffect.View = cameraTransforms.viewMatrix;
                    basicEffect.Projection = cameraTransforms.projectionMatrix;
                    Matrix transform = Matrix.CreateTranslation(tX * TERRAIN_SIDE, tY * TERRAIN_SIDE, 0);
                    basicEffect.World = Matrix.Multiply(transform, basicEffect.World);
                }
                mesh.Draw();
            }
        }
    }
}

