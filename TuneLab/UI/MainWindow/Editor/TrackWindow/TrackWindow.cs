using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneLab.Base.Event;
using TuneLab.GUI.Components;
using TuneLab.GUI.Input;
using TuneLab.Data;
using TuneLab.Base.Science;
using TuneLab.GUI;
using TuneLab.Utils;
using TuneLab.Extensions.Formats.DataInfo;
using TuneLab.I18N;
using TuneLab.UI.Commands;

namespace TuneLab.UI;

internal class TrackWindow : DockPanel, TimelineView.IDependency, TrackScrollView.IDependency, PlayheadLayer.IDependency, TrackVerticalAxis.IDependency, TrackHeadList.IDependency, ICommandContext
{
    public IProvider<IProject> ProjectProvider => mDependency.ProjectProvider;
    public IProvider<ITimeline> TimelineProvider => mDependency.ProjectProvider;
    public IQuantization Quantization => mQuantization;
    public TickAxis TickAxis => mTickAxis;
    public TrackVerticalAxis TrackVerticalAxis => mTrackVerticalAxis;
    public IPlayhead Playhead => mDependency.Playhead;
    public IProvider<IPart> EditingPart => mDependency.EditingPart;
    public IProject? Project => ProjectProvider.Object;
    public TrackScrollView TrackScrollView => mTrackScrollView;
    public INotifiableProperty<PlayScrollTarget> PlayScrollTarget => mDependency.PlayScrollTarget;

    public interface IDependency
    {
        IProvider<IProject> ProjectProvider { get; }
        IPlayhead Playhead { get; }
        IProvider<IPart> EditingPart { get; }
        void SwitchEditingPart(IPart? part);
        INotifiableProperty<PlayScrollTarget> PlayScrollTarget { get; }
    }

    public TrackWindow(IDependency dependency)
    {
        mDependency = dependency;
        mCommands = new CommandMap(new Dictionary<CommandId, Action>
        {
            [CommandId.SelectionDelete] = () => TrackScrollView.DeleteAllSelectedParts(),
            [CommandId.SelectionCopy] = () => TrackScrollView.Copy(),
            [CommandId.SelectionCut] = () => TrackScrollView.Cut(),
            [CommandId.SelectionPaste] = () => TrackScrollView.PasteAt(TrackScrollView.GetQuantizedTick(Playhead.Pos)),
            [CommandId.SelectionSelectAll] = () => Project?.Tracks.SelectMany(track => track.Parts).SelectAllItems(),
        });

        mQuantization = new(MusicTheory.QuantizationBase.Base_1, MusicTheory.QuantizationDivision.Division_8);
        mTickAxis = new();
        mTrackVerticalAxis = new(this);

        mTrackHeadList = new(this) { Width = 232, Margin = new(1, 0, 0, 0) };
        var headArea = new DockPanel();
        {
            var title = new DockPanel() { Height = 48, Background = Style.INTERFACE.ToBrush(), Margin = new(1, 0, 0, 0), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top };
            {
                var icon = new Image() { Source = Assets.Track.GetImage(Style.LIGHT_WHITE), Width = 24, Height = 24, Margin = new(16, 12, 12, 12) };
                title.AddDock(icon, Dock.Left);
                var name = new Label() { Content = "Track".Tr(TC.Dialog), FontSize = 16, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Foreground = Style.TEXT_LIGHT.ToBrush() };
                title.AddDock(name);
            }
            headArea.AddDock(title, Dock.Top);
            headArea.AddDock(new Border() { Height = 1, Background = Style.BACK.ToBrush() }, Dock.Top);
            headArea.AddDock(mTrackHeadList);
        }
        this.AddDock(headArea, Dock.Right);

        var layerPanel = new LayerPanel();
        {
            var layer = new DockPanel();
            {
                mTimelineView = new(this) { VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top };
                layer.AddDock(mTimelineView, Dock.Top);

                layer.AddDock(new Border() { Height = 1, Background = Style.BACK.ToBrush() }, Dock.Top);

                mTrackScrollView = new(this);
                layer.AddDock(mTrackScrollView);
            }
            layerPanel.Children.Add(layer);

            mPlayheadLayer = new(this);
            layerPanel.Children.Add(mPlayheadLayer);
        }
        this.AddDock(layerPanel);

        TickAxis.ScaleLevel = -8;
    }

    public void SwitchEditingPart(IPart part)
    {
        mDependency.SwitchEditingPart(part);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        mTickAxis.ViewLength = e.NewSize.Width - mTrackHeadList.Width;
        mTrackVerticalAxis.ViewLength = e.NewSize.Height - mTimelineView.Height;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (CommandRouter.TryHandle(e, this))
            return;

        if (e.IsHandledByTextBox())
            return;
    }

    ICommandContext? ICommandContext.ParentCommandContext => mDependency as ICommandContext;

    bool ICommandContext.CanExecuteCommand(CommandId command)
    {
        if (TrackScrollView.OperationState != TrackScrollView.State.None)
            return false;

        return mCommands.Contains(command);
    }

    bool ICommandContext.ExecuteCommand(CommandId command)
    {
        return mCommands.TryExecute(command);
    }

    readonly Quantization mQuantization;
    readonly TickAxis mTickAxis;
    readonly TrackVerticalAxis mTrackVerticalAxis;

    readonly TrackHeadList mTrackHeadList;
    readonly TimelineView mTimelineView;
    readonly TrackScrollView mTrackScrollView;
    readonly PlayheadLayer mPlayheadLayer;
    readonly CommandMap mCommands;

    readonly IDependency mDependency;
}
