using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneLab.Base.Utils;
using TuneLab.Configs;
using TuneLab.GUI;
using TuneLab.GUI.Components;
using TuneLab.I18N;
using TuneLab.Utils;
using TuneLab.Base.Properties;
using TuneLab.GUI.Controllers;
using Avalonia.Platform.Storage;
using TuneLab.Base.Event;
using TuneLab.Audio;
using TuneLab.Base.Structures;
using TuneLab.UI.Commands;
using Button = TuneLab.GUI.Components.Button;

namespace TuneLab.UI;

internal partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        Focusable = true;
        CanResize = false;
        WindowState = WindowState.Normal;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Topmost = true;

        TitleLabel.Content = "Settings".Tr(this);

        this.Background = Style.BACK.ToBrush();
        TitleLabel.Foreground = Style.TEXT_LIGHT.ToBrush();

        var closeButton = new GUI.Components.Button() { Width = 48, Height = 40 }
                .AddContent(new() { Item = new BorderItem() { CornerRadius = 0 }, ColorSet = new() { HoveredColor = Colors.White.Opacity(0.2), PressedColor = Colors.White.Opacity(0.2) } })
                .AddContent(new() { Item = new IconItem() { Icon = Assets.WindowClose }, ColorSet = new() { Color = Style.TEXT_LIGHT.Opacity(0.7) } });
        closeButton.Clicked += () =>
        {
            CloseSettingsWindow();
        };

        WindowControl.Children.Add(closeButton);
	
        var titleBar = this.FindControl<Grid>("TitleBar") ?? throw new System.InvalidOperationException("TitleBar not found");
        bool UseSystemTitle = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
	    if(UseSystemTitle){
		    titleBar.Height = 0;
	    }

        Content.Background = Style.INTERFACE.ToBrush();

        Settings.Language.Modified.Subscribe(async () => await this.ShowMessage("Tips".Tr(TC.Dialog), "Please restart to apply settings.".Tr(this)), s);

        var listView = new ListView() { Orientation = Avalonia.Layout.Orientation.Vertical, FitWidth = true };
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var comboBox = new ComboBoxController() { Width = 180 };
                comboBox.SetConfig(new(TranslationManager.Languages));
                comboBox.Bind(Settings.Language, false, s);
                
                panel.AddDock(comboBox, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Language".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var slider = new SliderController() { Width = 180, IsInterger = false };
                slider.SetRange(-24, 24);
                slider.SetDefaultValue(Settings.DefaultSettings.MasterGain);
                slider.Bind(Settings.MasterGain, true, s);
                panel.AddDock(slider, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Master Gain (dB)".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var comboBox = new ComboBoxController() { Width = 300 };
                comboBox.SetConfig(new(AudioEngine.GetAllDrivers()));
                comboBox.Bind(Settings.AudioDriver, false, s);
                comboBox.Display(AudioEngine.CurrentDriver.Value);

                panel.AddDock(comboBox, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Audio Driver".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var comboBox = new ComboBoxController() { Width = 300 };
                comboBox.SetConfig(new(AudioEngine.GetAllDevices()));
                comboBox.Bind(Settings.AudioDevice, false, s);
                comboBox.Display(AudioEngine.CurrentDevice.Value);
                
                panel.AddDock(comboBox, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Audio Device".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var comboBox = new ComboBoxController() { Width = 180 };
                comboBox.SetConfig(new(["32000", "44100", "48000", "96000", "192000"]));
                comboBox.Select(int.Parse, (int value) => { return value.ToString(); }).Bind(Settings.SampleRate, false, s);
                comboBox.Display(AudioEngine.SampleRate.Value.ToString());

                panel.AddDock(comboBox, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Sample Rate".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var comboBox = new ComboBoxController() { Width = 180 };
                comboBox.SetConfig(new(["64", "128", "256", "512", "1024", "2048", "4096", "8192"]));
                comboBox.Select(int.Parse, (int value) => { return value.ToString(); }).Bind(Settings.BufferSize, false, s);
                comboBox.Display(AudioEngine.BufferSize.Value.ToString());

                panel.AddDock(comboBox, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Buffer Size".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var slider = new SliderController() { Width = 180, IsInterger = false };
                slider.SetRange(0, 1);
                slider.SetDefaultValue(Settings.DefaultSettings.BackgroundImageOpacity);
                slider.Bind(Settings.BackgroundImageOpacity, true, s);
                panel.AddDock(slider, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Opacity".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new(0, 0, 12, 0) };
                panel.AddDock(name, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Custom Background Image".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var controller = new PathInput() { Margin = new(24, 12), Options = new FilePickerOpenOptions() { FileTypeFilter = [FilePickerFileTypes.ImageAll] } };
            controller.Bind(Settings.BackgroundImagePath, false, s);
            listView.Content.Children.Add(controller);
        }
        {
            var name = new TextBlock() { Margin = new(24, 12), Text = "Piano Key Samples".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
            listView.Content.Children.Add(name);
        }
        {
            var controller = new PathInput() { Margin = new(24, 12), Options = new FolderPickerOpenOptions() };
            controller.Bind(Settings.PianoKeySamplesPath, false, s);
            listView.Content.Children.Add(controller);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var slider = new SliderController() { Width = 180, IsInterger = true };
                slider.SetRange(10, 60);
                slider.SetDefaultValue(Settings.DefaultSettings.AutoSaveInterval);
                slider.Bind(Settings.AutoSaveInterval, false, s);
                panel.AddDock(slider, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Auto Save Interval (second)".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var slider = new SliderController() { Width = 180, IsInterger = true };
                slider.SetRange(1, 60);
                slider.SetDefaultValue(Settings.DefaultSettings.ParameterBoundaryExtension);
                slider.Bind(Settings.ParameterBoundaryExtension, false, s);
                panel.AddDock(slider, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Parameter Boundary Extension (tick)".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var checkBox = new GUI.Components.CheckBox();
                checkBox.Bind(Settings.ParameterSyncMode, false, s);
                panel.AddDock(checkBox, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Parameter Sync Mode".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12) };
            {
                var slider = new SliderController() { Width = 180, IsInterger = true };
                slider.SetRange(-720, 720);
                slider.SetDefaultValue(Settings.DefaultSettings.TrackHueChangeRate);
                slider.Bind(Settings.TrackHueChangeRate, true, s);
                panel.AddDock(slider, Dock.Right);
            }
            {
                var name = new TextBlock() { Text = "Track Hue Change Rate (degree/second)".Tr(this) + ": ", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                panel.AddDock(name);
            }
            listView.Content.Children.Add(panel);
        }
        {
            var divider = new Border() { Height = 1, Margin = new(24, 16), Background = Style.BACK.ToBrush() };
            listView.Content.Children.Add(divider);
        }
        {
            var panel = new DockPanel() { Margin = new(24, 12, 24, 4) };
            var title = new TextBlock()
            {
                Text = "Keyboard Shortcuts".Tr(this),
                FontSize = 18,
                FontWeight = FontWeight.Bold,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };
            panel.AddDock(title);

            var resetAllButton = CreateActionButton("Reset All".Tr(this), 96);
            resetAllButton.Clicked += () =>
            {
                ShortcutRegistry.ResetAll();
                RefreshShortcutRows();
            };
            panel.AddDock(resetAllButton, Dock.Right);
            listView.Content.Children.Add(panel);
        }
        {
            var description = new TextBlock()
            {
                Margin = new(24, 0, 24, 12),
                Text = "Changes take effect immediately and will be saved when this window closes.".Tr(this),
                Foreground = Style.TEXT_LIGHT.ToBrush(),
                TextWrapping = TextWrapping.Wrap,
            };
            listView.Content.Children.Add(description);
        }
        listView.Content.Children.Add(mShortcutRowsHost);
        RefreshShortcutRows();

        Content.AddDock(listView);
    }

    void CloseSettingsWindow()
    {
        Settings.Save(PathManager.SettingsFilePath);
        s.DisposeAll();
        Close();
    }

    void RefreshShortcutRows()
    {
        mShortcutRowsHost.Children.Clear();
        var groups = ShortcutHelpRegistry
            .GetAll()
            .GroupBy(item => item.Category)
            .OrderBy(group => group.Key);

        foreach (var group in groups)
        {
            mShortcutRowsHost.Children.Add(new TextBlock()
            {
                Text = GetCategoryName(group.Key),
                Margin = new(24, 12, 24, 4),
                FontSize = 16,
                FontWeight = FontWeight.Bold,
                Foreground = Style.TEXT_LIGHT.ToBrush(),
            });

            foreach (var item in group)
            {
                mShortcutRowsHost.Children.Add(BuildShortcutRow(item));
            }
        }
    }

    Control BuildShortcutRow(ShortcutHelpItem item)
    {
        var panel = new DockPanel() { Margin = new(24, 8) };

        var resetButton = CreateActionButton("Reset".Tr(TC.Dialog));
        resetButton.Clicked += () =>
        {
            ShortcutRegistry.ResetShortcut(item.Command);
            RefreshShortcutRows();
        };
        panel.AddDock(resetButton, Dock.Right);

        var editButton = CreateActionButton("Edit".Tr(TC.Dialog));
        editButton.Clicked += async () =>
        {
            if (!CommandMetadataRegistry.TryGet(item.Command, out var metadata))
                return;

            var dialog = new ShortcutEditDialog(metadata, item.Shortcut);
            var shortcut = await dialog.ShowDialog<Shortcut?>(this);
            if (!shortcut.HasValue)
                return;

            if (ShortcutRegistry.TryFindCommand(shortcut.Value, out var conflictCommand, item.Command) &&
                CommandMetadataRegistry.TryGet(conflictCommand, out var conflictMetadata))
            {
                await this.ShowMessage("Shortcut Conflict".Tr(this), "This shortcut is already assigned to".Tr(this) + ": " + conflictMetadata.DisplayName.Tr(this));
                return;
            }

            ShortcutRegistry.SetShortcut(item.Command, shortcut.Value);
            RefreshShortcutRows();
        };
        panel.AddDock(editButton, Dock.Right);

        panel.AddDock(new TextBlock()
        {
            Text = ShortcutDisplayFormatter.Format(item.Shortcut),
            FontFamily = Assets.NotoMono,
            Width = 96,
            Margin = new(0, 0, 12, 0),
            TextAlignment = TextAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Foreground = Style.LIGHT_WHITE.ToBrush(),
        }, Dock.Right);

        panel.AddDock(new TextBlock()
        {
            Text = item.DisplayName.Tr(this),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Foreground = Style.WHITE.ToBrush(),
        });

        return panel;
    }

    Button CreateActionButton(string text, double width = 80)
    {
        var button = new Button() { Width = width, Height = 28, Margin = new(8, 0, 0, 0) };
        button.AddContent(new() { Item = new BorderItem() { CornerRadius = 6 }, ColorSet = new() { Color = Style.BUTTON_PRIMARY, HoveredColor = Style.BUTTON_PRIMARY_HOVER } });
        button.AddContent(new() { Item = new TextItem() { Text = text }, ColorSet = new() { Color = Colors.White } });
        return button;
    }

    static string GetCategoryName(CommandCategory category)
    {
        return category switch
        {
            CommandCategory.App => "App".Tr(TC.Menu),
            CommandCategory.File => "File".Tr(TC.Menu),
            CommandCategory.Edit => "Edit".Tr(TC.Menu),
            CommandCategory.Transport => "Transport".Tr(TC.Menu),
            CommandCategory.Selection => "Selection".Tr(TC.Menu),
            CommandCategory.Piano => "Piano".Tr(TC.Menu),
            _ => category.ToString(),
        };
    }

    readonly DisposableManager s = new();
    readonly StackPanel mShortcutRowsHost = new() { Spacing = 0 };
}
