using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using TuneLab.GUI;
using TuneLab.GUI.Components;
using TuneLab.I18N;
using TuneLab.UI.Commands;
using TuneLab.Utils;
using Button = TuneLab.GUI.Components.Button;

namespace TuneLab.UI;

internal partial class ShortcutEditDialog : Window
{
    public ShortcutEditDialog(CommandMetadata metadata, Shortcut currentShortcut)
    {
        InitializeComponent();
        Focusable = true;
        CanResize = false;
        WindowState = WindowState.Normal;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Topmost = true;

        Background = Style.BACK.ToBrush();
        TitleLabel.Foreground = Style.TEXT_LIGHT.ToBrush();
        Title = "Edit Shortcut".Tr(this) + " - TuneLab";
        TitleLabel.Content = "Edit Shortcut".Tr(this);

        var closeButton = new Button() { Width = 48, Height = 40 }
            .AddContent(new() { Item = new BorderItem() { CornerRadius = 0 }, ColorSet = new() { HoveredColor = Colors.White.Opacity(0.2), PressedColor = Colors.White.Opacity(0.2) } })
            .AddContent(new() { Item = new IconItem() { Icon = Assets.WindowClose }, ColorSet = new() { Color = Style.TEXT_LIGHT.Opacity(0.7) } });
        closeButton.Clicked += () => Close(null);
        WindowControl.Children.Add(closeButton);

        var titleBar = this.FindControl<Grid>("TitleBar") ?? throw new InvalidOperationException("TitleBar not found");
        bool useSystemTitle = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
        if (useSystemTitle)
        {
            titleBar.Height = 0;
            Height -= 40;
        }

        Content.Background = Style.INTERFACE.ToBrush();
        CommandNameLabel.Text = metadata.DisplayName.Tr(this);
        CommandNameLabel.Foreground = Style.WHITE.ToBrush();
        HintLabel.Text = "Press the new shortcut. Press Esc to cancel.".Tr(this);
        HintLabel.Foreground = Style.TEXT_LIGHT.ToBrush();
        ShortcutBorder.Background = Style.BACK.ToBrush();
        ShortcutBorder.BorderBrush = Style.BUTTON_PRIMARY.ToBrush();
        ShortcutBorder.BorderThickness = new Thickness(1);
        ShortcutValueLabel.Foreground = Style.WHITE.ToBrush();
        ShortcutValueLabel.Text = ShortcutDisplayFormatter.Format(currentShortcut);

        Opened += (_, _) => Focus();
        KeyDown += OnDialogKeyDown;
    }

    void OnDialogKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            e.Handled = true;
            Close(null);
            return;
        }

        if (IsModifierKey(e.Key))
            return;

        e.Handled = true;
        Close(new Shortcut(e.Key, e.ModifierKeys()));
    }

    static bool IsModifierKey(Key key)
    {
        return key is Key.LeftCtrl or Key.RightCtrl
            or Key.LeftShift or Key.RightShift
            or Key.LeftAlt or Key.RightAlt;
    }
}
