namespace Editor.TextureEditor;

public partial class PreviewTools : Widget
{
	private float Margin => 6;
	private float Spacing => 0;

	public bool showRedChannel { get; set; } = true;
	public bool showGreenChannel { get; set; } = true;
	public bool showBlueChannel { get; set; } = true;
	public bool showAlphaChannel { get; set; } = true;

	public PreviewTools( Widget parent ) : base( parent )
    {
		Layout = Layout.Row();
		Layout.Margin = new( Margin );
		Layout.Spacing = Spacing;
        MaximumHeight = Theme.ControlHeight + Margin * 2;

		// Rendering options
		{
			var group = Layout.Add( AddGroup() );

			AddColorChannelButton(
				group.Layout,
				"Toggle Red Channel",
				"R",
				() => showRedChannel,
				( v ) => showRedChannel = v,
				Color.Red,
				new Color( 0.4f, 0.1f, 0.1f )
			);

			AddColorChannelButton(
				group.Layout,
				"Toggle Green Channel",
				"G",
				() => showGreenChannel,
				( v ) => showGreenChannel = v,
				Color.Green,
				new Color( 0.1f, 0.4f, 0.1f )
			);

			AddColorChannelButton(
				group.Layout,
				"Toggle Blue Channel",
				"B",
				() => showBlueChannel,
				( v ) => showBlueChannel = v,
				Color.Blue,
				new Color( 0.1f, 0.1f, 0.4f )
			);

			AddColorChannelButton(
				group.Layout,
				"Toggle Alpha Channel",
				"A",
				() => showAlphaChannel,
				( v ) => showAlphaChannel = v,
				Color.White,
				Color.Gray
			);
		}

		Layout.AddStretchCell();
    }

	private EditorToolButton AddToggleButton( Layout layout, string tooltip, Func<string> getIcon, Func<bool> getVal, Action<bool> setVal )
	{
		var __getVal = () => { try { return getVal(); } catch ( System.Exception ) { return false; } };
		var __setVal = ( bool b ) => { try { setVal( b ); } catch ( System.Exception ) { } };

		var b = new EditorToolButton();
		b.GetIcon = getIcon;
		b.ToolTip = tooltip;
		b.Action = () => __setVal( !__getVal() );
		b.IsActive = () => __getVal();

		layout.Add( b );
		return b;
	}

	private EditorToolButton AddColorChannelButton( Layout layout, string tooltip, string letter, Func<bool> getVal, Action<bool> setVal, Color enabledColor, Color disabledColor )
	{
		var __getVal = () => { try { return getVal(); } catch ( Exception ) { return false; } };
		var __setVal = ( bool b ) => { try { setVal( b ); } catch ( Exception ) { } };

		var b = new EditorToolButton();
		b.ToolTip = tooltip;
		b.Action = () => __setVal( !__getVal() );
		b.IsActive = () => __getVal();
		
		b.OnPaintOverride = () =>
		{
			var isActive = __getVal();
			var color = isActive ? enabledColor : disabledColor;
			
			Paint.ClearPen();
			
			Paint.SetPen( color );
			Paint.SetBrush( color );
			Paint.SetFont( "Arial", 14, 500 );
			Paint.DrawText( b.LocalRect, letter, TextFlag.Center );
			
			return true;
		};

		layout.Add( b );
		return b;
	}

	private EditorToolButton AddButton( Layout layout, string tooltip, Func<string> getIcon, Action action )
	{
		var b = new EditorToolButton();
		b.GetIcon = getIcon;
		b.ToolTip = tooltip;
		b.Action = action;

		layout.Add( b );
		return b;
	}

	private Widget AddGroup()
	{
		var w = new Widget();
		w.OnPaintOverride = () =>
		{
			Paint.ClearPen();
			Paint.SetBrush( Theme.ControlBackground );
			Paint.DrawRect( w.LocalRect, Theme.ControlRadius );

			return true;
		};

		w.FixedHeight = Theme.RowHeight;
		w.Layout = Layout.Row();
		w.Layout.Spacing = Spacing;
		w.Layout.Margin = new( 2, 0 );
		return w;
	}
}