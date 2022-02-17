using Microsoft.Xna.Framework;
using Spellwright.Spells;
using Spellwright.Spells.Base;
using Spellwright.UI.Components;
using Spellwright.Util;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Spellwright.Menus
{
    internal class SpellInput : UIPanel
    {
        private int previousTextLength;
        public UITextBox textbox;

        public SpellInput()
        {
            previousTextLength = 0;

            Width = 500f;
            Height = 60f;
            BackgroundColor = new Color(60, 60, 60, 255) * 0.685f;

            textbox = new UITextBox();
            textbox.Width = 450;
            textbox.OnKeyPressed += new UITextBox.KeyPressedHandler(OnKeyPressed);
            textbox.OnEnterPresseed += new EventHandler(OnEnterPressed);
            textbox.OnEscPressed += new EventHandler(OnCancel);
            textbox.OnTabPressed += new EventHandler(OnCancel);
            AddChild(textbox);

            CenterToParent();
        }

        public void Show()
        {
            if (Visible)
                return;
            Visible = true;
            textbox.Focus();
        }

        public void Hide()
        {
            Visible = false;
            textbox.Unfocus();
            textbox.Text = "";
        }

        public override void Update()
        {
            float shiftX = (Width - textbox.Width) / 2f;
            float shiftY = (Height - textbox.Height) / 2f;
            textbox.Position = new Vector2(shiftX, shiftY);

            CenterToParent();

            base.Update();
        }

        private void OnKeyPressed(object sender, char key)
        {
            if (textbox.Text.Length <= 0)
                return;

            bool characterIsAdded = textbox.Text.Length > previousTextLength;
            previousTextLength = textbox.Text.Length;
            if (characterIsAdded)
            {
                Vector2 position = Main.LocalPlayer.position;
                //SpawnSparkles(DustID.BoneTorch, position, 5);
                //SpawnSparkles(DustID.GoldFlame, position, 5);
                //SpawnSparkles(DustID.Firefly, position, 5);
                SpawnSparkles(DustID.SilverCoin, position, 5);
                //SpawnSparkles(ModContent.DustType<Sparkle>(), position, 5);
                SoundEngine.PlaySound(SoundID.Item19, position);
            }

            if (textbox.Text.Length > 50)
                textbox.Text = textbox.Text.Substring(0, textbox.Text.Length - 1);
        }

        private static void SpawnSparkles(int dustType, Vector2 position, int dustCount)
        {
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dustPosition = position + UtilRandom.RandomVector(15, 300);
                Vector2 velocity = UtilRandom.RandomVector(1, 5);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, dustType, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
        private static void SpawnCircle(int dustType, Vector2 position, int dustCount, int minRadius, int maxRadius, int direction = 1)
        {
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dustPosition = position + UtilRandom.RandomVector(minRadius, maxRadius);
                //Vector2 velocity = UtilRandom.RandomVector(.1f, 2.5f);
                float scale = UtilRandom.NextFloat(.1f, 2.5f);
                Vector2 velocity = dustPosition - position;
                velocity.Normalize();
                Vector2 velocityChange = UtilRandom.RandomVector(.05f, 0.1f);
                velocity += velocityChange;
                velocity *= direction;


                var dust = Dust.NewDustDirect(dustPosition, 22, 22, dustType, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }

        private void OnEnterPressed(object sender, EventArgs e)
        {
            string spellText = textbox.Text;
            SpellCastResult castResult = SpellProcessor.ProcessCast(spellText);
            Main.ClosePlayerChat();

            Vector2 position = Main.LocalPlayer.position;

            if (castResult == SpellCastResult.Success)
            {
                SpawnCircle(DustID.GoldCoin, position, 75, 20, 80, 1);
                SpawnCircle(DustID.GoldCoin, position, 15, 20, 80, -1);
                SpawnCircle(DustID.GoldCoin, position, 15, 90, 130);
                SoundEngine.PlaySound(SoundID.Item4, position);
                //SoundEngine.PlaySound(SoundID.Item30, position);
            }
            else if (castResult == SpellCastResult.IncantationInvalid)
            {
                SpawnCircle(DustID.SilverCoin, position, 40, 20, 80);
                SpawnCircle(DustID.SilverCoin, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);
            }
            else
            {
                SpawnCircle(DustID.CopperCoin, position, 40, 20, 80);
                SpawnCircle(DustID.CopperCoin, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);
            }

            Hide();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Hide();
        }
    }
}