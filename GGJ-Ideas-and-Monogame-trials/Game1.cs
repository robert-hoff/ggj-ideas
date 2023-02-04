using System.Diagnostics;
using GGJ_Ideas_and_Monogame_trials.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/**
 *
 * <remarks>constructor</remarks>
 * - `new GraphicsDeviceManager(this)` attaches itself to this.GraphicsDevice
 * - removed 'spritebatch' (refer to template)
 *
 * <remarks>Initialize()</remarks>
 * - called once after constructor
 * - GraphicsDevice will be ready
 *
 * <remarks>method Update(GameTime gameTime)</remarks>
 * Called on each game-loop
 * - Target FPS (default) = 60
 * - gameTime.TotalGameTime = global clock
 * - gameTime.ElapsedTime, time since last update (1/60 seconds)
 *
 * <remarks>method Draw(GameTime gameTime)</remarks>
 * Same as Update. Called immediately after (on the execution thread)
 * - attaching an object of type `DrawableGameComponent`
 *   will have its Draw(GameTime gametime) method implicitly called at this step (needs to be registered)
 *
 *
 *
 */
namespace GGJ_Ideas_and_Monogame_trials
{
    public class Game1 : Game
    {
        private const int DEFAULT_VIEWPORT_WIDTH = 800;
        private const int DEFAULT_VIEWPORT_HEIGHT = 600;
        private CameraTransforms cameraTransforms;

        private Model spaceshipModel;
        private DrawTriangle drawTriangle;
        private DrawLine drawLine;

        public Game1()
        {
            Content.RootDirectory = "Content";
            Window.Title = "Monogame Trials";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            _ = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = DEFAULT_VIEWPORT_WIDTH,
                PreferredBackBufferHeight = DEFAULT_VIEWPORT_HEIGHT,
                // PreferredBackBufferFormat = SurfaceFormat.Color,
                PreferMultiSampling = true,
                PreferredDepthStencilFormat = DepthFormat.Depth24,
                SynchronizeWithVerticalRetrace = true,
                // SynchronizeWithVerticalRetrace = false,
            };

            int screenWidth = Window.ClientBounds.Width;
            int screenHeight = Window.ClientBounds.Height;
            cameraTransforms = new CameraTransforms(screenWidth, screenHeight);
        }

        protected override void Initialize()
        {
            // enable these lines if using SpriteBatch
            // GraphicsDevice.BlendState = BlendState.Opaque;
            // GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // don't cull polygon backsides (probably set this on a per-model bases)
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            drawTriangle = new DrawTriangle(GraphicsDevice, cameraTransforms);
            drawLine = new DrawLine(GraphicsDevice, cameraTransforms);

            Content = new ContentManager(this.Services, "Content");
            // spaceshipModel = Content.Load<Model>("ship-no-texture");
            spaceshipModel = Content.Load<Model>("ship-with-texture");
        }

        private int previousMouseScroll = 0;
        private int mouseDragX = 0;
        private int mouseDragY = 0;
        private bool leftMouseIsReleased = true;

        protected override void Update(GameTime gameTime)
        {
            cameraTransforms.UpdateViewportDimensions(Window.ClientBounds.Width, Window.ClientBounds.Height);

            // TESTING ONLY, print camera position
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Debug.WriteLine($"camera position {cameraTransforms.cameraPosition}");
                Debug.WriteLine($"rotation {MathHelper.ToDegrees(cameraTransforms.cameraRotation)}");
                Debug.WriteLine($"distance from origin {Vector3.Distance(cameraTransforms.cameraPosition, new Vector3(0,0,0))}");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (leftMouseIsReleased)
                {
                    leftMouseIsReleased = false;
                    mouseDragX = Mouse.GetState().X;
                    mouseDragY = Mouse.GetState().Y;
                } else
                {
                    float diffX = Mouse.GetState().X - mouseDragX;
                    float diffY = Mouse.GetState().Y - mouseDragY;
                    cameraTransforms.IncrementCameraOrbitDegrees(diffX/4);
                    cameraTransforms.OrbitUpDown(diffY / 20);
                    mouseDragX = Mouse.GetState().X;
                    mouseDragY = Mouse.GetState().Y;
                }
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                leftMouseIsReleased = true;
            }

            int currentMouseScroll = Mouse.GetState().ScrollWheelValue;
            if (previousMouseScroll > currentMouseScroll)
            {
                cameraTransforms.ZoomOut();
                previousMouseScroll = currentMouseScroll;
            }
            if (previousMouseScroll < currentMouseScroll)
            {
                cameraTransforms.ZoomIn();
                previousMouseScroll = currentMouseScroll;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Matrix world = cameraTransforms.worldMatrix;
            Matrix view = cameraTransforms.viewMatrix;
            Matrix projection = cameraTransforms.projectionMatrix;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            // GraphicsDevice.Clear(Color.Black);
            // GraphicsDevice.Clear(Color.White);

            // -- render game components
            DrawModel(spaceshipModel, world, view, projection);
            // DrawModelTranslationAndColor(spaceshipModel, world, view, projection, 0, 1, 2, Color.Tomato.ToVector3());
            // DrawModelTranslationAndColor(spaceshipModel, world, view, projection, 0, 1, 1, Color.Plum.ToVector3());

            // drawTriangle.DrawTestTriangle(GraphicsDevice);

            drawTriangle.DrawTestSquare(GraphicsDevice);
            drawTriangle.DrawTestSquare(GraphicsDevice, -1, 0);
            drawTriangle.DrawTestSquare(GraphicsDevice, -1, -1);
            drawTriangle.DrawTestSquare(GraphicsDevice, 0, -1);
            drawLine.DrawAxis(GraphicsDevice);
            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    BasicEffect basicEffect = (BasicEffect) effect;
                    basicEffect.World = world;
                    basicEffect.View = view;
                    basicEffect.Projection = projection;
                    Matrix.CreateTranslation(0, 0, 0, out Matrix translation);
                    basicEffect.World = Matrix.Multiply(translation, basicEffect.World);

                }
                mesh.Draw();
            }
        }

        private void DrawModelTranslationAndColor(Model model, Matrix world, Matrix view, Matrix projection, float tX, float tY, float tZ, Vector3 color)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    BasicEffect basicEffect = (BasicEffect) effect;
                    basicEffect.World = world;
                    basicEffect.View = view;
                    basicEffect.Projection = projection;
                    basicEffect.DiffuseColor = color;
                    Matrix.CreateTranslation(tX, tY, tZ, out Matrix translation);
                    basicEffect.World = Matrix.Multiply(translation, basicEffect.World);

                }
                mesh.Draw();
            }
        }
    }
}
