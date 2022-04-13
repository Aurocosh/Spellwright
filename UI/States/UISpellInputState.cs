using Microsoft.Xna.Framework;
using Spellwright.Content.Spells;
using Spellwright.Content.Spells.Base;
using Spellwright.Extensions;
using Spellwright.UI.Components;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace Spellwright.UI.States
{
    internal class UISpellInputState : UIState
    {
        private int previousTextLength;

        private UIPanel mainPanel;
        public UITextBox textbox;

        public override void OnInitialize()
        {
            mainPanel = new UIPanel
            {
                Width = new StyleDimension(500, 0),
                Height = new StyleDimension(60, 0),
                HAlign = .5f,
                VAlign = .5f,
                BackgroundColor = new Color(60, 60, 60, 255) * 0.685f
            };

            textbox = new UITextBox
            {
                Width = new StyleDimension(450, 0),
                HAlign = .5f,
                VAlign = .5f
            };

            textbox.OnKeyPressed += new UITextBox.TextChangeHandler(OnKeyPressed);
            textbox.OnEnterPresseed += new EventHandler(OnEnterPressed);
            textbox.OnEscPressed += new EventHandler(OnCancel);
            textbox.OnTabPressed += new EventHandler(OnCancel);

            mainPanel.Append(textbox);

            Append(mainPanel);
        }

        public override void OnActivate()
        {
            Show();
        }
        public override void OnDeactivate()
        {
            textbox.Unfocus();
            textbox.Text = "";
        }
        public void Show()
        {
            textbox.Focus();
        }

        private void OnKeyPressed(object sender, string text)
        {
            if (textbox.Text.Length <= 0)
                return;

            if (textbox.Text.Length > 50)
            {
                textbox.Text = textbox.Text.Substring(0, 50);
                return;
            }

            bool characterIsAdded = textbox.Text.Length > previousTextLength;
            previousTextLength = textbox.Text.Length;
            if (characterIsAdded)
            {
                Vector2 position = Main.LocalPlayer.Center;
                //SpawnSparkles(DustID.Firefly, position, 5);
                SpawnSparkles(DustID.SilverCoin, position, 5);
                SoundEngine.PlaySound(SoundID.Item19, position);
            }
        }

        private static void SpawnSparkles(int dustType, Vector2 position, int dustCount)
        {
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(15, 300);
                Vector2 velocity = Main.rand.NextVector2Unit().ScaleRandom(1, 5);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, dustType, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
        private static void SpawnCircle(int dustType, Vector2 position, int dustCount, int minRadius, int maxRadius, int direction = 1)
        {
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2Unit().ScaleRandom(minRadius, maxRadius);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, 2.5f);
                velocity *= direction;

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, dustType, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }

        private void OnEnterPressed(object sender, EventArgs e)
        {
            string spellText = textbox.Text;
            Spellwright.Instance.userInterface.SetState(null);

            SpellCastResult castResult = SpellProcessor.ProcessCast(spellText);
            Main.ClosePlayerChat();

            Vector2 position = Main.LocalPlayer.position;

            if (castResult == SpellCastResult.Success)
            {
                //SpawnCircle(DustID.GoldCoin, position, 75, 20, 80, 1);
                //SpawnCircle(DustID.GoldCoin, position, 15, 20, 80, -1);
                //SpawnCircle(DustID.GoldCoin, position, 15, 90, 130);
                SoundEngine.PlaySound(SoundID.Item4, position);
            }
            else if (castResult == SpellCastResult.IncantationInvalid)
            {
                SpawnCircle(DustID.SilverCoin, position, 40, 20, 80);
                SpawnCircle(DustID.SilverCoin, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);

                var message = Spellwright.GetTranslation("CastResult", "IncantationInvalid");
                Main.NewText(message, Color.OrangeRed);
            }
            else if (castResult == SpellCastResult.ModifiersInvalid)
            {
                SpawnCircle(DustID.IceTorch, position, 40, 20, 80);
                SpawnCircle(DustID.IceTorch, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);

                var message = Spellwright.GetTranslation("CastResult", "ModifiersInvalid");
                Main.NewText(message, Color.OrangeRed);
            }
            else if (castResult == SpellCastResult.LevelTooLow)
            {
                SpawnCircle(DustID.DemonTorch, position, 40, 20, 80);
                SpawnCircle(DustID.DemonTorch, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);

                var message = Spellwright.GetTranslation("CastResult", "LevelTooLow");
                Main.NewText(message, Color.OrangeRed);
            }
            else
            {
                SpawnCircle(DustID.Torch, position, 40, 20, 80);
                SpawnCircle(DustID.Torch, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);

                var message = Spellwright.GetTranslation("CastResult", "ArgumentInvalid");
                Main.NewText(message, Color.OrangeRed);
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Spellwright.Instance.userInterface.SetState(null);
        }
    }
}
