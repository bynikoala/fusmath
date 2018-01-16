using Fusee.Engine;
using Fusee.Math;

namespace FuseeApp.Handler
{
    public class Handler : RenderCanvas
    {
        private GUIHandler _guiHandler;

        private IFont _guiFontCabin18;
        private IFont _guiFontCabin24;
        private GUIText _guiText;

        // private GUIImage _guiImage;

        private GUIButton[] _guiDiffs;

        private GUIPanel _guiPanel;

        private GUIButton _guiResetButton;
        private GUIButton _guiSolveButton;

        public override void Init()
        {
            Width = 616;
            Height = 688;

            // GUIHandler
            _guiHandler = new GUIHandler();
            _guiHandler.AttachToContext(RC);

            // font + text
            _guiFontCabin18 = RC.LoadFont("Assets/calibri.ttf", 18);
            _guiFontCabin24 = RC.LoadFont("Assets/calibri.ttf", 24);

            _guiText = new GUIText("Spot all seven differences!", _guiFontCabin24, 310, 35);
            _guiText.TextColor = new float4(1, 1, 1, 1);

            _guiHandler.Add(_guiText);

            // // image
            // _guiImage = new GUIImage("Assets/spot_the_diff.png", 0, 0, -5, 600, 650);
            // _guiHandler.Add(_guiImage);

            // buttons / rectangles
            _guiDiffs = new GUIButton[7];

            _guiDiffs[0] = new GUIButton(240, 328, 40, 40);
            _guiDiffs[1] = new GUIButton(3, 595, 40, 40);
            _guiDiffs[2] = new GUIButton(220, 580, 40, 40);
            _guiDiffs[3] = new GUIButton(325, 495, 40, 40);
            _guiDiffs[4] = new GUIButton(265, 435, 40, 40);
            _guiDiffs[5] = new GUIButton(490, 540, 40, 40);
            _guiDiffs[6] = new GUIButton(495, 605, 40, 40);

            for (int i = 0; i < 7; i++)
            {
                _guiDiffs[i].ButtonColor = new float4(0, 0, 0, 0);

                _guiDiffs[i].BorderColor = new float4(0, 0, 0, 1);
                _guiDiffs[i].BorderWidth = 0;

                _guiDiffs[i].OnGUIButtonDown += OnDiffButtonDown;

                _guiHandler.Add(_guiDiffs[i]);
            }

            // panel
            _guiPanel = new GUIPanel("Menu", _guiFontCabin18, 10, 10, 150, 110);
            _guiHandler.Add(_guiPanel);

            // reset button
            _guiResetButton = new GUIButton("Reset", _guiFontCabin18, 25, 40, 100, 25);

            _guiResetButton.OnGUIButtonDown += OnMenuButtonDown;
            _guiResetButton.OnGUIButtonUp += OnMenuButtonUp;
            _guiResetButton.OnGUIButtonEnter += OnMenuButtonEnter;
            _guiResetButton.OnGUIButtonLeave += OnMenuButtonLeave;

            _guiPanel.ChildElements.Add(_guiResetButton);

            // solve button
            _guiSolveButton = new GUIButton("Solve", _guiFontCabin18, 25, 70, 100, 25);

            _guiSolveButton.OnGUIButtonDown += OnMenuButtonDown;
            _guiSolveButton.OnGUIButtonUp += OnMenuButtonUp;
            _guiSolveButton.OnGUIButtonEnter += OnMenuButtonEnter;
            _guiSolveButton.OnGUIButtonLeave += OnMenuButtonLeave;

            _guiPanel.ChildElements.Add(_guiSolveButton);
        }

        private void OnDiffButtonDown(GUIButton sender, MouseEventArgs mea)
        {
            sender.BorderWidth = 2;
        }

        private void OnMenuButtonDown(GUIButton sender, MouseEventArgs mea)
        {
            sender.BorderWidth = 2;
        }

        private void OnMenuButtonUp(GUIButton sender, MouseEventArgs mea)
        {
            sender.BorderWidth = 1;

            // make them visible or hide them?
            var bWidth = (sender == _guiSolveButton) ? 2 : 0;

            foreach (var guiButton in _guiDiffs)
                guiButton.BorderWidth = bWidth;
        }

        private static void OnMenuButtonEnter(GUIButton sender, MouseEventArgs mea)
        {
            // if left mouse button is pressed
            if (Input.Instance.IsButton(MouseButtons.Left))
                sender.BorderWidth = 2;

            sender.TextColor = new float4(0.8f, 0.1f, 0.1f, 1);
        }

        private static void OnMenuButtonLeave(GUIButton sender, MouseEventArgs mea)
        {
            sender.BorderWidth = 1;
            sender.TextColor = new float4(0f, 0f, 0f, 1);
        }

        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            _guiHandler.RenderGUI();
            Present();
        }

        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width/(float) Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);

            // refresh all elements
            _guiHandler.Refresh();
        }

        public static void Main()
        {
            var app = new Handler();
            app.Run();
        }
    }
}