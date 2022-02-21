using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Spells;
using Spellwright.Spells.Base;
using Spellwright.UI.Components;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace Spellwright.UI
{
    internal class SpellInput : UIPanel
    {
        private int previousTextLength;
        public UITextBox textbox;

        public SpellInput()
        {
            previousTextLength = 0;

            HAlign = .5f;
            VAlign = .5f;

            //Left = new StyleDimension(0, 0.5f);
            //Top = new StyleDimension(0, 0.5f);

            Width = new StyleDimension(500, 0);
            Height = new StyleDimension(60, 0);

            BackgroundColor = new Color(60, 60, 60, 255) * 0.685f;

            textbox = new UITextBox();
            textbox.Width = new StyleDimension(450, 0);
            textbox.OnKeyPressed += new UITextBox.TextChangeHandler(OnKeyPressed);
            textbox.OnEnterPresseed += new EventHandler(OnEnterPressed);
            textbox.OnEscPressed += new EventHandler(OnCancel);
            textbox.OnTabPressed += new EventHandler(OnCancel);
            textbox.HAlign = .5f;
            textbox.VAlign = .5f;

            Append(textbox);

            //CenterToParent();
        }
        public override void OnActivate()
        {
            base.OnActivate();
            Show();
        }
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            Hide();
        }
        public void Show()
        {
            Visible = true;
            textbox.Focus();
        }

        public void Hide()
        {
            Visible = false;
            textbox.Unfocus();
            textbox.Text = "";
            //Spellwright.instance.spellInputState.Deactivate();
            //Spellwright.instance.userInterface.IsVisible = false;
        }

        public override void Update()
        {

            //HAlign = .5f;
            //VAlign = .5f;


            //Left = new StyleDimension(0, 0.0f);
            //Top = new StyleDimension(0, 0.0f);
            //float shiftX = (ElementWidth - textbox.ElementWidth) / 2f;
            //float shiftY = (ElementHeight - textbox.ElementHeight) / 2f;
            //textbox.Position = new Vector2(shiftX, shiftY);

            //CenterToParent();

            base.Update();
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
                Vector2 position = Main.LocalPlayer.position;
                //SpawnSparkles(DustID.BoneTorch, position, 5);
                //SpawnSparkles(DustID.GoldFlame, position, 5);
                //SpawnSparkles(DustID.Firefly, position, 5);
                SpawnSparkles(DustID.SilverCoin, position, 5);
                //SpawnSparkles(ModContent.DustType<Sparkle>(), position, 5);
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
            //for (int i = 0; i < dustCount; i++)
            //{
            //    Vector2 dustPosition = UtilVector2.GetPointOnRing(position, minRadius, maxRadius);
            //    Vector2 velocity = UtilVector2.RandomVector(position, dustPosition, .1f, 2.5f, -60, 60);
            //    velocity *= direction;

            //    var dust = Dust.NewDustDirect(dustPosition, 22, 22, dustType, 0f, 0f, 100, default, 2.5f);
            //    dust.velocity = velocity;
            //    dust.noLightEmittence = true;
            //}
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
            else if (castResult == SpellCastResult.ModifiersInvalid)
            {
                SpawnCircle(DustID.IceTorch, position, 40, 20, 80);
                SpawnCircle(DustID.IceTorch, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);
            }
            else
            {
                SpawnCircle(DustID.Torch, position, 40, 20, 80);
                SpawnCircle(DustID.Torch, position, 15, 90, 130);
                //SoundEngine.PlaySound(SoundID.Item20, position);
                SoundEngine.PlaySound(SoundID.Item45, position);
            }

            Deactivate();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Hide();
        }
    }
}