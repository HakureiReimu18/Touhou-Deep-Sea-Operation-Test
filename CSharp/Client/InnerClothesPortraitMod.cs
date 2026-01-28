using System;
using Barotrauma;
using Barotrauma.Extensions;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

[assembly: IgnoreAccessChecksTo("Barotrauma")]
[assembly: IgnoreAccessChecksTo("BarotraumaCore")]
[assembly: IgnoreAccessChecksTo("DedicatedServer")]

namespace Touhou.InnerClothesPortrait;

public sealed class InnerClothesPortraitPlugin : IAssemblyPlugin
{
    public void Initialize()
    {
        var harmony = new Harmony("touhou.innerclothesportrait");
        harmony.PatchAll();
    }

    public void OnLoadCompleted()
    {
    }

    public void PreInitPatching()
    {
    }

    public void Dispose()
    {
    }
}

[HarmonyPatch(typeof(CharacterHUD), "Draw")]
internal static class InnerClothesPortraitHudPatch
{
    private const float IconScaleRatio = 0.45f;

    private static void Postfix(SpriteBatch spriteBatch, Character character, Camera cam)
    {
        if (spriteBatch == null || character == null)
        {
            return;
        }

        if (GUI.DisableHUD)
        {
            return;
        }

        if (character != Character.Controlled || character.IsDead || character.Inventory == null)
        {
            return;
        }

        if (CharacterHealth.OpenHealthWindow != null || character.SelectedCharacter != null || character.ShouldLockHud())
        {
            return;
        }

        Item inner = character.Inventory.GetItemInLimbSlot(InvSlotType.InnerClothes);
        if (inner == null)
        {
            return;
        }

        ItemPrefab prefab = inner.Prefab;
        if (!prefab.Tags.Contains("Touhou_Clothes".ToIdentifier()))
        {
            return;
        }

        Sprite iconSprite = prefab.InventoryIcon ?? prefab.Sprite;
        if (iconSprite == null)
        {
            return;
        }

        if (iconSprite.SourceRect.Width <= 0 || iconSprite.SourceRect.Height <= 0)
        {
            return;
        }

        Rectangle portraitArea = HUDLayoutSettings.PortraitArea;
        int iconSize = (int)(Math.Min(portraitArea.Width, portraitArea.Height) * IconScaleRatio);
        if (iconSize <= 0)
        {
            return;
        }

        var drawPos = new Vector2(
            portraitArea.Right - iconSize - HUDLayoutSettings.Padding / 2f,
            portraitArea.Bottom - iconSize - HUDLayoutSettings.Padding / 2f);

        float scaleX = iconSize / (float)iconSprite.SourceRect.Width;
        float scaleY = iconSize / (float)iconSprite.SourceRect.Height;
        float scale = MathF.Min(scaleX, scaleY);

        Color color = iconSprite == inner.Sprite ? inner.GetSpriteColor() : inner.GetInventoryIconColor();
        iconSprite.Draw(spriteBatch, drawPos, color, 0f, scale);
    }
}
