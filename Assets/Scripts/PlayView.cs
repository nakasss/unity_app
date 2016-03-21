using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayView : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Play UI Group
	 */
	#region Play UI Group
	[Header ("UI Group")]


	[SerializeField] private Animator playUiGroupAnimator;
	private static readonly string PLAYUI_SHOW_KEY_NAME = "IsShowGroup";

	public void ShowUIGroup () {
		playUiGroupAnimator.SetBool (PLAYUI_SHOW_KEY_NAME, true);		
	}

	public void HideUIGroup () {
		playUiGroupAnimator.SetBool (PLAYUI_SHOW_KEY_NAME, false);
	}

	public bool IsShowUIGroup () {
		return playUiGroupAnimator.GetBool (PLAYUI_SHOW_KEY_NAME);
	}

	public void ToggleUIGroup () {
		if (IsShowUIGroup ()) {
			HideUIGroup ();
		} else {
			ShowUIGroup ();
		}
	}


	#endregion Play UI Group


	/*
	 * Top Area
	 */
	#region Top Area
	[Header ("Top Area")]


	// Title
	[SerializeField] private Text videoTitle;

	public void SetVideoTitle (string title) {
		videoTitle.text = title;
	}

	public string GetVideoTitle () {
		return videoTitle.text;
	}


	// Viewer Icon
	[SerializeField] private Toggle viewrIcon;

	public bool IsViewerIconOn () {
		return viewrIcon.isOn;
	}


	#endregion End : Top Area


	/*
	 * Middle Area
	 */
	#region Middle Area
	[Header ("Middle Area")]


	// Play Button Wrapper
	[SerializeField] private CanvasGroup playButtonWrapper;
	[SerializeField] private Animator playButtonAnimator;
	private static readonly string PLAY_BUTTON_ANIMATOR_KEY_NAME = "IsAppear";

	public void ShowPlayBtn () {
		playButtonAnimator.SetBool (PLAY_BUTTON_ANIMATOR_KEY_NAME, true);
	}

	public void HidePlayBtn () {
		playButtonAnimator.SetBool (PLAY_BUTTON_ANIMATOR_KEY_NAME, false);
	}

	public bool IsShowPlayBtn () {
		return playButtonAnimator.GetBool (PLAY_BUTTON_ANIMATOR_KEY_NAME);
	}


	// After Play Buttons Wrapper
	[SerializeField] private CanvasGroup videoEndButtonsWrapper;
	[SerializeField] private Animator videoEndButtonsAnimator;
	private static readonly string VIDEOEND_BUTTONS_ANIMATOR_KEY_NAME = "IsShow";

	public void ShowVideoEndBtns () {
		videoEndButtonsAnimator.SetBool (VIDEOEND_BUTTONS_ANIMATOR_KEY_NAME, true);
	}

	public void HideVideoEndBtns () {
		videoEndButtonsAnimator.SetBool (VIDEOEND_BUTTONS_ANIMATOR_KEY_NAME, false);
	}

	public bool IsShowVideoEndBtns () {
		return videoEndButtonsAnimator.GetBool (VIDEOEND_BUTTONS_ANIMATOR_KEY_NAME);
	}


	// Toggle Middle Area Buttons
	public void ToggleMiddleAreaButtons () {
		if (IsShowPlayBtn ()) {
			HidePlayBtn ();
			ShowVideoEndBtns ();
		} else {
			ShowPlayBtn ();
			HideVideoEndBtns ();
		}
	}


	// Play Button
	[SerializeField] private Toggle playButton;

	public void ShowPlayButton () {
		playButton.isOn = false;
		UpdatePlayButtonBg ();
	}

	public void ShowPauseButton () {
		playButton.isOn = true;
		UpdatePlayButtonBg ();
	}

	public void UpdatePlayButtonBg () {
		float alpha = playButton.isOn ? 0.0f : 255.0f;
		Color bgColor = playButton.targetGraphic.color;
		bgColor.a = alpha;
		playButton.targetGraphic.color = bgColor;
	}

	public void EnablePlayButton () {
		playButton.interactable = true;
	}

	public void DisablePlayButton () {
		playButton.interactable = false;
	}

	public bool IsPlayButtonOn () {
		return playButton.isOn;
	}

	public bool IsPlayButtonPlay () {
		return !IsPlayButtonOn();
	}

	public bool IsPlayButtonPause () {
		return IsPlayButtonOn();
	}

	#endregion End : Middle Area


	/*
	 * Bottom Area
	 */
	#region Bottom Area
	[Header ("Bottom Area")]


	// Progress Bar
	[SerializeField] private Slider progressBar;

	public void SetProgress (float progress) {
		progressBar.value = progress;
	}

	public float GetCurrentProgress () {
		return progressBar.value;
	}

	public void EnableProgressBar () {
		progressBar.interactable = true;
	}

	public void DisableProgressBar () {
		progressBar.interactable = false;
	}


	// Progress Timer
	[SerializeField] private Text progressTimer;

	public void SetProgressTimerBySec (long sec) {
		string minutePart = (sec / 60 < 10) ? "0" + (sec / 60).ToString() : (sec / 60).ToString();
		string secPart = (sec % 60 < 10) ? "0" + (sec % 60).ToString() : (sec % 60).ToString();;

		SetProgressTimerByText (minutePart + ":" + secPart);
	}

	public void SetProgressTimerByText (string time) {
		progressTimer.text = time;
	}

	public long GetProgressTimeSec () {
		string currentTimeText = GetProgressTimeText ();

		string[] devidedTimeText = currentTimeText.Split(':');
		long minutePart = long.Parse(devidedTimeText[0]);
		long minutePartSec = 60 * minutePart;
		long secPart = long.Parse(devidedTimeText[1]);

		return minutePartSec + secPart; 
	}

	public string GetProgressTimeText () {
		return progressTimer.text;
	}


	// Total Timer
	[SerializeField] private Text totalTimer;

	public void SetTotalTimerBySec (long sec) {
		string minutePart = (sec / 60 < 10) ? "0" + (sec / 60).ToString() : (sec / 60).ToString();
		string secPart = (sec % 60 < 10) ? "0" + (sec % 60).ToString() : (sec % 60).ToString();;

		SetTotalTimerByText (minutePart + ":" + secPart);
	}

	public void SetTotalTimerByText (string time) {
		totalTimer.text = time;
	}

	public long GetTotalTimeSec () {
		string currentTimeText = GetTotalTimeText ();

		string[] devidedTimeText = currentTimeText.Split(':');
		long minutePart = long.Parse(devidedTimeText[0]);
		long minutePartSec = 60 * minutePart;
		long secPart = long.Parse(devidedTimeText[1]);

		return minutePartSec + secPart; 
	}

	public string GetTotalTimeText () {
		return totalTimer.text;
	}

	#endregion End : Bottom Area
}
