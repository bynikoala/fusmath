using System;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using Fusee.Engine;
using Microsoft.VisualBasic;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.Core.GUI;

namespace FuseeApp
{

   
    public class MatheGeoBahnen : RenderCanvas
    {   
        // Timefactor
        private float tf = 0.5f;
        // Flying range
        private float range = 1.25f;

        // Object Transform (single movements)
        private TransformComponent _satelliteMovement;
        // private TransformComponent _earthMovement;

        

        // Horizontal and vertical rotation Angles for the displayed object
        private static float _angleHorz = M.PiOver4, _angleVert;
        
        // Horizontal and vertical angular speed
        private static float _angleVelHorz, _angleVelVert;

        // Overall speed factor. Change this to adjust how fast the rotation reacts to input
        private const float RotationSpeed = 7;

        // Damping factor
        private const float Damping = 0.8f;

        private SceneContainer _worldScene;
        private SceneRenderer _sceneRenderer;

        private bool _keys;
        

        // Init is called on startup. 
        public override void Init()        
        {

            float3 test1 = FuseeApp.Grosskreis.getPolCoord(new float3(1,1,1));
            float3 test2 = FuseeApp.Grosskreis.getKartCoord(test1);

            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.2f, 0.2f, 0.2f, 1);

            // Load the World model
            _worldScene = AssetStorage.Get<SceneContainer>("World.fus");

            // Find objects to move
            _satelliteMovement = _worldScene.Children.FindNodes(node => node.Name == "Satellite")?.FirstOrDefault()?.GetTransform();
            // _earthMovement = _worldScene.Children.FindNodes(node => node.Name == "Earth")?.FirstOrDefault()?.GetTransform();

            // Wrap a SceneRenderer around the model.
            _sceneRenderer = new SceneRenderer(_worldScene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);


            // Mouse and keyboard movement
            if (Keyboard.LeftRightAxis != 0 || Keyboard.UpDownAxis != 0)
            {
                _keys = true;
            }

            if (Mouse.LeftButton)
            {
                _keys = false;
                _angleVelHorz = -RotationSpeed * Mouse.XVel * DeltaTime * 0.0005f;
                _angleVelVert = -RotationSpeed * Mouse.YVel * DeltaTime * 0.0005f;
            }
            else if (Touch.GetTouchActive(TouchPoints.Touchpoint_0))
            {
                _keys = false;
                var touchVel = Touch.GetVelocity(TouchPoints.Touchpoint_0);
                _angleVelHorz = -RotationSpeed * touchVel.x * DeltaTime * 0.0005f;
                _angleVelVert = -RotationSpeed * touchVel.y * DeltaTime * 0.0005f;
            }
            else
            {
                if (_keys)
                {
                    _angleVelHorz = -RotationSpeed * Keyboard.LeftRightAxis * DeltaTime;
                    _angleVelVert = -RotationSpeed * Keyboard.UpDownAxis * DeltaTime;
                }
                else
                {
                    var curDamp = (float)System.Math.Exp(-Damping * DeltaTime);
                    _angleVelHorz *= curDamp;
                    _angleVelVert *= curDamp;
                }
            }


            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;

            // Create the camera matrix and set it as the current ModelView transformation
            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            var mtxCam = float4x4.LookAt(0, 20, Mouse.Wheel * 5 - 300, 0, 0, 0, 0, 1, 0);
            RC.ModelView = mtxCam * mtxRot;

            // Move above defined objects with respect to several factors as the timefactor tf or the range factor rf
            // _earthMovement.Rotation = new float3(0, -0.1f * M.Pi * TimeSinceStart * tf, 0);
            // _satelliteMovement.Rotation = new float3(0, 0.15f * M.Pi * TimeSinceStart * tf, 0);

            _satelliteMovement.Translation = new float3(range * M.Sin(tf * TimeSinceStart), -1.5f, range * M.Cos(tf * TimeSinceStart));
            

            // Render the scene loaded in Init()
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

        private InputDevice Creator(IInputDeviceImp device)
        {
            throw new NotImplementedException();
        }

        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width/(float) Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}