namespace Editor.TextureEditor;

public class TextureRect : SceneCustomObject
{
	public PreviewTools Tools { get; set; }
	public Texture Texture { get; set; }

	public TextureRect( SceneWorld sceneWorld, Texture texture ) : base( sceneWorld )
	{
		Texture = texture;
	}

	public override void RenderSceneObject()
	{
		base.RenderSceneObject();

		if ( Texture == null )
			return;

		var textureSize = Texture.Size;
		var viewportSize = Graphics.Viewport.Size;
		textureSize *= Math.Min( viewportSize.x / textureSize.x, viewportSize.y / textureSize.y );

		//var bitMap = Texture.GetBitmap(0);
		// for ( var x = 0; x < bitMap.Width; x++ )
        // {
        //     for ( var y = 0; y < bitMap.Height; y++ )
        //     {
		// 		var pixColor = bitMap.GetPixel(x, y);
        //         if ( !Tools.showRedChannel ) bitMap.SetPixel(x, y, pixColor.WithRed(0));
		// 		if ( !Tools.showGreenChannel ) bitMap.SetPixel(x, y, pixColor.WithGreen(0));
		// 		if ( !Tools.showBlueChannel ) bitMap.SetPixel(x, y, pixColor.WithBlue(0));
		// 		if ( !Tools.showAlphaChannel ) bitMap.SetPixel(x, y, pixColor.WithAlpha(0));
        //     }
        // }
		var tex = Texture;

		Graphics.Attributes.SetComboEnum( "D_BLENDMODE", BlendMode.Normal );
		Graphics.Attributes.Set( "Texture", tex );
		Graphics.Attributes.Set( "LayerMat", Matrix.Identity );
		Graphics.DrawQuad( new Rect( (viewportSize - textureSize) * 0.5f, textureSize ), Material.UI.Basic, Color.White );
	}
}

public class Preview : Widget
{
	private readonly RenderingWidget Rendering;

	private PreviewTools _tools;
	public PreviewTools Tools
	{
		get => _tools;
		set
		{
			_tools = value;
			Rendering.TextureRect.Tools = value;
		}
	}
	public Texture Texture { set => Rendering.TextureRect.Texture = value; }

	public Preview( Widget parent ) : base( parent )
	{
		Name = "Preview";
		WindowTitle = "Preview";
		SetWindowIcon( "photo" );

		Layout = Layout.Column();

		Tools = new PreviewTools( this );
		Layout.Add( Tools );

		Rendering = new RenderingWidget( this );
		Layout.Add( Rendering );
	}

	private class RenderingWidget : SceneRenderingWidget
	{
		public TextureRect TextureRect { get; private set; }

		public RenderingWidget( Widget parent ) : base( parent )
		{
			MouseTracking = true;
			FocusMode = FocusMode.Click;

			Scene = Scene.CreateEditorScene();

			using ( Scene.Push() )
			{
				{
					Camera = new GameObject( true, "camera" ).GetOrAddComponent<CameraComponent>( false );
					Camera.ZNear = 0.1f;
					Camera.ZFar = 4000;
					Camera.LocalRotation = new Angles( 0, 180, 0 );
					Camera.FieldOfView = 10;
					Camera.BackgroundColor = Color.Transparent;
					Camera.Enabled = true;
				}
			}

			TextureRect = new TextureRect( Scene.SceneWorld, null );
		}

		public override void OnDestroyed()
		{
			base.OnDestroyed();

			Scene?.Destroy();
			Scene = null;
		}
	}
}
