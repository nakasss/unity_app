using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VideoCellBase : MonoBehaviour {
    [SerializeField] private Text titleLabel;
	[SerializeField] public RawImage thumbnailImg;
	[SerializeField] private AspectRatioFitter thumbAspectFitter;
    private long id;
    public long ID {
        get { return id; }
        set { id = value; }
    }
    
    
    private RectTransform cachedRectTransform;
	public RectTransform CachedRectTransform {
		get {
			if (cachedRectTransform == null) {
                cachedRectTransform = GetComponent<RectTransform>();
            }
			return cachedRectTransform;
		}
	}
    
    public float Height {
		get { return CachedRectTransform.sizeDelta.y; }
		set {
			Vector2 sizeDelta = CachedRectTransform.sizeDelta;
			sizeDelta.y = value;
			CachedRectTransform.sizeDelta = sizeDelta;
		}
	}
	
	public Vector2 Top {
		get {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			return CachedRectTransform.anchoredPosition + 
				new Vector2(0.0f, corners[1].y);
		}
		set {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			CachedRectTransform.anchoredPosition = 
				value - new Vector2(0.0f, corners[1].y);
		}
	}
	
	public Vector2 Bottom {
		get {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			return CachedRectTransform.anchoredPosition + 
				new Vector2(0.0f, corners[3].y);
		}
		set {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			CachedRectTransform.anchoredPosition = 
				value - new Vector2(0.0f, corners[3].y);
		}
	}
    
    public string Title {
        get {
            return titleLabel.text;
        }
        set {
            titleLabel.text = value;
        }
    }
    
    public Texture Thumbnail {
        get {
            return thumbnailImg.texture; 
        }
        set {
            thumbnailImg.texture = value;
            // Set Aspect
            float aspect = (float)value.width / (float)value.height;;
            thumbAspectFitter.aspectRatio = aspect;
        }
    }
    
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    public void SetCellParent (Transform parent) {
        Vector3 scale = transform.localScale;
        Vector3 localPosition = transform.localPosition;
		Vector2 sizeDelta = CachedRectTransform.sizeDelta;
		Vector2 offsetMax = CachedRectTransform.offsetMax;
		Vector2 offsetMin = CachedRectTransform.offsetMin;
        
        transform.SetParent(parent);
        
        transform.localScale = scale;
        transform.localPosition = localPosition;
		CachedRectTransform.sizeDelta = sizeDelta;
		CachedRectTransform.offsetMin = offsetMin;
		CachedRectTransform.offsetMax = offsetMax;
    }
    
    
    private void setTitle (string title) {
        titleLabel.text = title;
    }
    
    private void setThumbnail (Texture img) {
        thumbnailImg.texture = img;
        
        // Set Aspect
        float aspect = (float)img.width / (float)img.height;;
		thumbAspectFitter.aspectRatio = aspect;
    }
}
