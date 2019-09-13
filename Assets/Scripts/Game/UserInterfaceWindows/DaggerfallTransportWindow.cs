// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2019 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Hazelnut

using UnityEngine;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.Items;

namespace DaggerfallWorkshop.Game.UserInterfaceWindows
{
    public class DaggerfallTransportWindow : DaggerfallPopupWindow
    {
        #region UI Rects

        Rect footButtonRect = new Rect(5, 5, 120, 7);
        Rect horseButtonRect = new Rect(5, 14, 120, 7);
        Rect cartButtonRect = new Rect(5, 23, 120, 7);
        Rect shipButtonRect = new Rect(5, 32, 120, 7);
        Rect exitButtonRect = new Rect(44, 42, 43, 15);

//		Rect footDisabledRect = new Rect(1, 1, 120, 7);     // Can foot option ever be disabled?
        Rect horseDisabledRect = new Rect(1, 10, 120, 7);
        Rect cartDisabledRect = new Rect(1, 19, 120, 7);
        Rect shipDisabledRect = new Rect(1, 28, 120, 7);

        #endregion

        #region UI Controls

        Panel mainPanel = new Panel();
        Button footButton;
        Button horseButton;
        Button cartButton;
        Button shipButton;
        Button exitButton;

        #endregion

        #region UI Textures

        Texture2D baseTexture;
        Texture2D disabledTexture;

        #endregion

        #region Fields

        const string baseTextureName = "MOVE00I0.IMG";
        const string disabledTextureName = "MOVE01I0.IMG";

        Vector2 baseSize;

        KeyCode toggleClosedBinding;

        #endregion

        #region Constructors

        public DaggerfallTransportWindow(IUserInterfaceManager uiManager)
            : base(uiManager)
        {
            // Clear background
            ParentPanel.BackgroundColor = Color.clear;
        }

        #endregion

        #region Setup Methods

        protected override void Setup()
        {
            // What transport options does the player have?
            ItemCollection inventory = GameManager.Instance.PlayerEntity.Items;
            bool hasHorse = GameManager.Instance.TransportManager.HasHorse();
            bool hasCart = GameManager.Instance.TransportManager.HasCart();
            bool hasShip = GameManager.Instance.TransportManager.HasShip();

            // Load all textures
            LoadTextures();

            // Create interface panel
            mainPanel.HorizontalAlignment = HorizontalAlignment.Center;
            mainPanel.VerticalAlignment = VerticalAlignment.Middle;
            mainPanel.BackgroundTexture = baseTexture;
            mainPanel.Position = new Vector2(0, 50);
            mainPanel.Size = baseSize;
            DFSize disabledTextureSize = new DFSize(122, 36);

            // Foot button
            footButton = DaggerfallUI.AddButton(footButtonRect, mainPanel);
            footButton.OnMouseClick += FootButton_OnMouseClick;
            footButton.ShortcutKey = DaggerfallShortcut.Keys[DaggerfallMessageBox.MessageBoxButtons.TransportFoot];

            // Horse button
            horseButton = DaggerfallUI.AddButton(horseButtonRect, mainPanel);
            if (hasHorse) {
                horseButton.OnMouseClick += HorseButton_OnMouseClick;
                horseButton.ShortcutKey = DaggerfallShortcut.Keys[DaggerfallMessageBox.MessageBoxButtons.TransportHorse];
            }
            else {
                horseButton.BackgroundTexture = ImageReader.GetSubTexture(disabledTexture, horseDisabledRect, disabledTextureSize);
            }
            // Cart button
            cartButton = DaggerfallUI.AddButton(cartButtonRect, mainPanel);
            if (hasCart) {
                cartButton.OnMouseClick += CartButton_OnMouseClick;
                cartButton.ShortcutKey = DaggerfallShortcut.Keys[DaggerfallMessageBox.MessageBoxButtons.TransportCart];
            }
            else {
                cartButton.BackgroundTexture = ImageReader.GetSubTexture(disabledTexture, cartDisabledRect, disabledTextureSize);
            }
            // Ship button
            shipButton = DaggerfallUI.AddButton(shipButtonRect, mainPanel);
            if (hasShip) {
                shipButton.OnMouseClick += ShipButton_OnMouseClick;
                shipButton.ShortcutKey = DaggerfallShortcut.Keys[DaggerfallMessageBox.MessageBoxButtons.TransportShip];
            }
            else {
                shipButton.BackgroundTexture = ImageReader.GetSubTexture(disabledTexture, shipDisabledRect, disabledTextureSize);
            }

            // Exit button
            exitButton = DaggerfallUI.AddButton(exitButtonRect, mainPanel);
            exitButton.OnMouseClick += ExitButton_OnMouseClick;
            exitButton.ShortcutKey = DaggerfallShortcut.Keys[DaggerfallMessageBox.MessageBoxButtons.TransportExit];

            NativePanel.Components.Add(mainPanel);

            // Store toggle closed binding for this window
            toggleClosedBinding = InputManager.Instance.GetBinding(InputManager.Actions.Transport);
        }

        #endregion

        #region Overrides

        public override void Update()
        {
            base.Update();

            // Toggle window closed with same hotkey used to open it
            if (Input.GetKeyUp(toggleClosedBinding))
                CloseWindow();

            mainPanel.KeyboardActivation();
        }

        #endregion

        #region Private Methods

        void LoadTextures()
        {
            ImageData baseData = ImageReader.GetImageData(baseTextureName);
            baseTexture = baseData.texture;
            baseSize = new Vector2(baseData.width, baseData.height);
            disabledTexture = ImageReader.GetTexture(disabledTextureName);
        }

        #endregion

        #region Event Handlers

        private void ExitButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            CloseWindow();
        }

        private void FootButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            // Reset to normal on foot walking.
            GameManager.Instance.TransportManager.TransportMode = TransportModes.Foot;
            CloseWindow();
        }

        private void HorseButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            // Change to riding a horse.
            GameManager.Instance.TransportManager.TransportMode = TransportModes.Horse;
            CloseWindow();
        }

        private void CartButton_OnMouseClick(BaseScreenComponent sender, Vector2 position) {
            // Change to riding a cart.
            GameManager.Instance.TransportManager.TransportMode = TransportModes.Cart;
            CloseWindow();
        }

        private void ShipButton_OnMouseClick(BaseScreenComponent sender, Vector2 position) {
            // Teleport to your ship, or back.
            GameManager.Instance.TransportManager.TransportMode = TransportModes.Ship;
            CloseWindow();
        }

        #endregion
    }
}