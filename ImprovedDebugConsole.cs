using ModAPI.Attributes;
using TheForest;
using UnityEngine;
using System.Collections.Generic;
using System;
using TheForest.Utils;
using UnityEngine.VR;
using UnityEngine.Profiling;
using System.Collections;

namespace ConsoleAlwaysOn
{
    internal class ImprovedDebugConsole : MonoBehaviour
    {
        private int winWidth;
        private int winHeight;

        private string labelContent;

		private GUIStyle labelStyle, labelStyleStatus, labelStyleTitle, buttonStyle, toggleStyle;

		public static Color errorColor, warningColor, exceptionColor, logColor;
		public static int textSize, maxLogs;
		public static bool menuVisible, closeOnInput, toggleOnStartup, awaitingSettingsUpdate;

		[ExecuteOnGameStart]
        private static void AddMeToScene()
        {
            new GameObject("__Main__").AddComponent<ImprovedDebugConsole>();
        }

        private void Start()
        {
			defaultSettings();

			awaitingSettingsUpdate = true;
			menuVisible = false;

			winWidth = Screen.width;
            winHeight = Screen.height;

            labelContent = "Console is enabled";
		}
        
		private void Update()
        {
			if (ModAPI.Input.GetButtonDown("openSettings"))
            {
				menuVisible = !menuVisible;
            }
        }

        private void OnGUI()
        {
			// Settings labels
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontSize = 18;
				labelStyle.fixedWidth = 200;
			}
			// Top-right corner label
			if (labelStyleStatus == null)
            {
				labelStyleStatus = new GUIStyle(labelStyle);
				labelStyleStatus.normal.textColor = Color.green;
			}
			// Settings menu title
			if (labelStyleTitle == null)
			{
				labelStyleTitle = new GUIStyle(GUI.skin.label);
				labelStyleTitle.fontSize = 32;
				labelStyleTitle.alignment = TextAnchor.MiddleCenter;
			}
			// Settings buttons
			if (buttonStyle == null)
            {
				buttonStyle = new GUIStyle(GUI.skin.button);
				buttonStyle.fontSize = 22;
				buttonStyle.fixedHeight = 30f;
				buttonStyle.fixedWidth = 200f;
				buttonStyle.margin = new RectOffset(70, 30, 0, 0);
            }
			// Toggle buttons
			if (toggleStyle == null)
            {
				toggleStyle = new GUIStyle(GUI.skin.toggle);
				toggleStyle.fontSize = 22;
            }

			// Top-right corner label
			Vector2 labelSize = labelStyle.CalcSize(new GUIContent(labelContent));
            GUI.Label(new Rect(winWidth - labelSize.x - 30f, 30f, labelSize.x, labelSize.y), labelContent, labelStyleStatus);

			if (menuVisible)
			{
				GUILayout.BeginArea(new Rect(winWidth - 610, (winHeight - 500) / 2, 600f, 500f), GUI.skin.box);

				// Title line
				GUILayout.Label("Improved Debug Console Settings", labelStyleTitle, new GUILayoutOption[0]);
				GUILayout.Space(50f);

				// Error color
				GUILayout.BeginHorizontal();
				GUILayout.Label("Error color (R-G-B): ", labelStyle, new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				});
				errorColor.r = GUILayout.HorizontalSlider(errorColor.r, 0f, 1f);
				errorColor.g = GUILayout.HorizontalSlider(errorColor.g, 0f, 1f);
				errorColor.b = GUILayout.HorizontalSlider(errorColor.b, 0f, 1f);
				GUILayout.EndHorizontal();

				// Warning color
				GUILayout.BeginHorizontal();
				GUILayout.Label("Warning color (R-G-B): ", labelStyle, new GUILayoutOption[0]);
				warningColor.r = GUILayout.HorizontalSlider(warningColor.r, 0f, 1f);
				warningColor.g = GUILayout.HorizontalSlider(warningColor.g, 0f, 1f);
				warningColor.b = GUILayout.HorizontalSlider(warningColor.b, 0f, 1f);
				GUILayout.EndHorizontal();

				// Exception color
				GUILayout.BeginHorizontal();
				GUILayout.Label("Exception color (R-G-B): ", labelStyle, new GUILayoutOption[0]);
				exceptionColor.r = GUILayout.HorizontalSlider(exceptionColor.r, 0f, 1f);
				exceptionColor.g = GUILayout.HorizontalSlider(exceptionColor.g, 0f, 1f);
				exceptionColor.b = GUILayout.HorizontalSlider(exceptionColor.b, 0f, 1f);
				GUILayout.EndHorizontal();

				// Log color
				GUILayout.BeginHorizontal();
				GUILayout.Label("Log color (R-G-B): ", labelStyle, new GUILayoutOption[0]);
				logColor.r = GUILayout.HorizontalSlider(logColor.r, 0f, 1f);
				logColor.g = GUILayout.HorizontalSlider(logColor.g, 0f, 1f);
				logColor.b = GUILayout.HorizontalSlider(logColor.b, 0f, 1f);
				GUILayout.EndHorizontal();

				GUILayout.Space(20f);

				// Text size
				GUILayout.BeginHorizontal();
				GUILayout.Label("Text size: ", labelStyle, new GUILayoutOption[0]);
				if (int.TryParse(GUILayout.TextField(textSize.ToString(), new GUILayoutOption[0]), out int newTextSize))
                {
					textSize = newTextSize;
                }
				GUILayout.EndHorizontal();

				// Max logs
				GUILayout.BeginHorizontal();
				GUILayout.Label("Max logs shown: ", labelStyle, new GUILayoutOption[0]);
				if (int.TryParse(GUILayout.TextField(maxLogs.ToString(), new GUILayoutOption[0]), out int newMaxLogs))
				{
					maxLogs = newMaxLogs;
				}
				GUILayout.EndHorizontal();

				GUILayout.Space(20f);

				// Close on input
				GUILayout.BeginHorizontal();
				closeOnInput = GUILayout.Toggle(closeOnInput, new GUIContent("Close console after commands"), toggleStyle, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();

				// Toggle on startup
				GUILayout.BeginHorizontal();
				toggleOnStartup = GUILayout.Toggle(toggleOnStartup, new GUIContent("Open log on startup"), toggleStyle, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();

				GUILayout.Space(50f);

				// Last buttons line
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Apply settings", buttonStyle, new GUILayoutOption[0]))
                {
					awaitingSettingsUpdate = true;
				}
				if (GUILayout.Button("Reset settings", buttonStyle, new GUILayoutOption[0]))
                {
					defaultSettings();
					awaitingSettingsUpdate = true;
                }
				GUILayout.EndHorizontal();

				GUILayout.EndArea();
            }
        }

		private void defaultSettings()
        {
			errorColor = Color.red;
			warningColor = Color.yellow;
			exceptionColor = Color.magenta;
			logColor = Color.white;

			textSize = 18;
			maxLogs = 20;

			closeOnInput = true;
			toggleOnStartup = true;
		}
	}

    internal class CheatsEx : Cheats
    {
		protected override void Start()
        {
			base.Start();
			// Simulates the player having written developermodeon right after the script is started to directly enable it
            this.DebugConsoleIndex = this.debugConsoleCode.Length;
        }
    }

    internal class DebugConsoleEx : DebugConsole
    {
		public Color errorColor, warningColor, exceptionColor, logColor;
		public int textSize;
		public bool closeOnInput, toggleOnStartup;

		private Vector2 scrollPosition;
		private float contentHeight, lastKeyTime, inputDelay;
		private int consecutiveInputs;

		protected override void Awake()
        {
            base.Awake();

			this.scrollPosition = new Vector2(0, 0);

			this.inputDelay = 0.35f;
			this.consecutiveInputs = 0;

			if (this.toggleOnStartup)
            {
				this.ToggleOverlay();
				this.ToggleOverlay();
			}
		}

        protected override void OnGUI()
		{
			Color color = GUI.color;
			if (this.HandleInput())
			{
				return;
			}
			if (this._showConsole)
			{
				GUI.skin.label.fontSize = 12;
				GUILayout.BeginHorizontal(this._consoleRowStyle, new GUILayoutOption[]
				{
					GUILayout.Width((float)Screen.width),
					GUILayout.Height(20f)
				});
				GUILayout.Label("$> ", GUI.skin.label, new GUILayoutOption[0]);
				GUI.color = Color.gray;
				GUILayout.Label(this._autocomplete, new GUILayoutOption[]
				{
					GUILayout.MinWidth((float)(Screen.width * 3 / 4))
				});
				GUI.color = color;
				GUI.SetNextControlName("debugConsole");
				this._consoleInput = GUI.TextField(GUILayoutUtility.GetLastRect(), this._consoleInput, GUI.skin.label);
				if (this._focusConsoleField)
				{
					this._focusConsoleField = false;
					GUI.FocusControl("debugConsole");
				}
				if (this._selectConsoleText)
				{
					GUI.FocusControl("debugConsole");
					this._selectConsoleText = false;
					this._focusConsoleField = true;
					TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
					textEditor.MoveLineEnd();
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			if (this._showOverlay)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[]
				{
					GUILayout.Width((float)Screen.width),
					GUILayout.Height(24f)
				});
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Label("IsGPad: " + TheForest.Utils.Input.IsGamePad, GUI.skin.button, new GUILayoutOption[0]);
				GUILayout.Label("WasGPad: " + TheForest.Utils.Input.WasGamePad, GUI.skin.button, new GUILayoutOption[0]);
				GUILayout.Label("AnyKey: " + TheForest.Utils.Input.anyKeyDown, GUI.skin.button, new GUILayoutOption[0]);
				GUILayout.Label("MouseLoc: " + TheForest.Utils.Input.IsMouseLocked, GUI.skin.button, new GUILayoutOption[0]);
				GUILayout.EndVertical();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Label("FPS: " + (int)this._fps, GUI.skin.button, new GUILayoutOption[0]);
				GUILayout.Label("DXType: " + SystemInfo.graphicsDeviceType, GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.Label("DX11: " + (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.Label("GSL: " + SystemInfo.graphicsShaderLevel, GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.Label("CompSh: " + SystemInfo.supportsComputeShaders, GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				if (ForestVR.Enabled)
				{
					GUILayout.Label("VRRenderScale: " + VRSettings.renderScale, GUI.skin.button, new GUILayoutOption[]
					{
						GUILayout.MinWidth(100f)
					});
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Total Alloc: {0}", DebugConsole.ToMbString(Profiler.GetTotalAllocatedMemoryLong())), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				GUILayout.Label(string.Format("Total Reserved: {0}", DebugConsole.ToMbString(Profiler.GetTotalReservedMemoryLong())), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				GUILayout.Label(string.Format("Heap Size: {0}", DebugConsole.ToMbString(Profiler.GetMonoHeapSizeLong())), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				GUILayout.Label(string.Format("Used Size: {0}", DebugConsole.ToMbString(Profiler.GetMonoUsedSizeLong())), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				GUILayout.Label(string.Format("GC: {0}", DebugConsole.ToMbString(GC.GetTotalMemory(false))), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.EndVertical();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Label(string.Concat(new object[]
				{
					(int)FMOD_StudioEventEmitter.HoursSinceMidnight,
					"h",
					(int)((FMOD_StudioEventEmitter.HoursSinceMidnight - (float)((int)FMOD_StudioEventEmitter.HoursSinceMidnight)) * 60f),
					(!Clock.Dark) ? " (d)" : " (n)"
				}), GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.Width(80f)
				});
				GUILayout.Label((!LocalPlayer.IsInCaves) ? "Not in cave" : "In cave", GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.Width(80f)
				});
				if (LocalPlayer.Inventory)
				{
					GUILayout.Label(string.Concat(new object[]
					{
						"x: ",
						LocalPlayer.Transform.position.x,
						"\ny: ",
						LocalPlayer.Transform.position.y,
						"\nz: ",
						LocalPlayer.Transform.position.z
					}), GUI.skin.button, new GUILayoutOption[]
					{
						GUILayout.Width(80f)
					});
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				this._showPlayerStats = GUILayout.Toggle(this._showPlayerStats, "Player Stats", GUI.skin.button, new GUILayoutOption[0]);
				if (TheForest.Utils.Scene.SceneTracker)
				{
					GUILayout.Label("Shadow Distance: " + TheForest.Utils.Scene.Atmosphere.DebugShadowDist.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("Light Forward: " + TheForest.Utils.Scene.Atmosphere.DebugLightForward.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("mod shadow blend: " + TheForest.Utils.Scene.Atmosphere.DebugModShadow.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("Occlusion: " + LOD_Manager.TreeOcclusionBonusRatio.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("shadow resolution: " + QualitySettings.shadowResolution.ToString(), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("Total Entities: " + this._totalEntities.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("Total Active Entities: " + this._activeEntities.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("Total Active Trees: " + this._activeTrees.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
					GUILayout.Label("Total Frozen Trees: " + this._frozenTrees.ToString("0.00"), GUI.skin.button, new GUILayoutOption[0]);
				}
				GUILayout.EndVertical();
				foreach (KeyValuePair<Type, int> keyValuePair in DebugConsole.Counters)
				{
					if (GUILayout.Button(keyValuePair.Key.Name + ": " + keyValuePair.Value, new GUILayoutOption[0]))
					{
						this.CheckAmount(keyValuePair.Key, keyValuePair.Value);
					}
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			if (this._showPlayerStats && LocalPlayer.Stats)
			{
				GUILayout.BeginArea(new Rect((float)(Screen.width - 250), (float)(Screen.height / 2 - 200), 250f, 400f), GUI.skin.textArea);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Label(string.Concat(new object[]
				{
					"+ Athleticism real:",
					LocalPlayer.Stats.Skills.AthleticismSkillLevel,
					", display:",
					LocalPlayer.Stats.Skills.AthleticismSkillLevelProgressApprox
				}), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Label(string.Format("|- Sprint: {0:F0} / {1:F0} = {2:F0} ", LocalPlayer.Stats.Skills.TotalRunDuration, LocalPlayer.Stats.Skills.RunSkillLevelDuration, LocalPlayer.Stats.Skills.TotalRunDuration / LocalPlayer.Stats.Skills.RunSkillLevelDuration), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Label(string.Format("|- Diving: {0:F0} / {1:F0} = {2:F0} ", LocalPlayer.Stats.Skills.TotalLungBreathingDuration, LocalPlayer.Stats.Skills.BreathingSkillLevelDuration, LocalPlayer.Stats.Skills.TotalLungBreathingDuration / LocalPlayer.Stats.Skills.BreathingSkillLevelDuration), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Space(20f);
				GUILayout.Label(string.Format("+ Weight {0:F3}lbs", LocalPlayer.Stats.PhysicalStrength.CurrentWeight), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Label(string.Format("|- Current Calories Burnt: {0:F3}", LocalPlayer.Stats.Calories.CurrentCaloriesBurntCount), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Label(string.Format("|- Current Calories Eaten: {0:F3}", LocalPlayer.Stats.Calories.CurrentCaloriesEatenCount), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Label(string.Format("|- Excess Calories Final: {0}", LocalPlayer.Stats.Calories.GetExcessCaloriesFinal()), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Label(string.Format("|- Time to next resolution: {0:F3} Hours (IG)", LocalPlayer.Stats.Calories.TimeToNextResolution()), this._textStyle, new GUILayoutOption[0]);
				int excessCaloriesFinal = LocalPlayer.Stats.Calories.GetExcessCaloriesFinal();
				GUILayout.Label(string.Format("|- Weight change at resolution: {0:F3} lbs", (float)excessCaloriesFinal * ((excessCaloriesFinal <= 0) ? LocalPlayer.Stats.Calories.WeightLossPerMissingCalory : LocalPlayer.Stats.Calories.WeightGainPerExcessCalory)), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Space(20f);
				GUILayout.Space(20f);
				GUILayout.Label(string.Format("+ Strength {0:F4} ({1})", LocalPlayer.Stats.PhysicalStrength.CurrentStrength, (excessCaloriesFinal <= 0) ? "Losing" : "Gaining"), this._textStyle, new GUILayoutOption[0]);
				GUILayout.Space(20f);
				GUILayout.EndVertical();
				GUILayout.EndArea();
			}
			if (this._showLog)
			{
				GUILayout.BeginArea(new Rect(5f, 40f, 600f, 500f));
				GUILayout.BeginScrollView(new Vector2(scrollPosition.x, scrollPosition.y), new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();

				// Counter to track the height of all entries, used for the scrollbar
				this.contentHeight = 0;

				foreach (LogContent logContent in this._logs)
				{
					// Calculates required height for showing the log content and increases the log line height when necessary to display multiline entries
					// Making a new style is not ideal at all, but directly setting _logRowStyle.fixedHeight or using GUILayoutOption doesn't seem to work
					float calcHeight = this._textStyle.CalcHeight(logContent.content, 520f);
					GUIStyle _logRowStyleResized = new GUIStyle(this._logRowStyle);
					_logRowStyleResized.fixedHeight = calcHeight > this._logRowStyle.fixedHeight ? calcHeight : this._logRowStyle.fixedHeight;
					this.contentHeight += _logRowStyleResized.fixedHeight;
					GUILayout.BeginHorizontal(_logRowStyleResized, new GUILayoutOption[0]);

					switch (logContent.type)
					{
						case LogType.Error:
							GUI.color = errorColor;
							break;
						case LogType.Warning:
							GUI.color = warningColor;
							break;
						case LogType.Log:
							GUI.color = logColor;
							break;
						case LogType.Exception:
							GUI.color = exceptionColor;
							break;
					}
					GUILayout.Label(logContent.type + ((logContent.amount <= 1) ? string.Empty : (" x" + logContent.amount)), this._textStyle, new GUILayoutOption[]
					{
						GUILayout.Width(80f)
					});
					GUILayout.Label(logContent.content, this._textStyle, new GUILayoutOption[0]);
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				GUILayout.EndArea();
				GUI.color = color;

				if (!string.IsNullOrEmpty(GUI.tooltip))
				{
					GUIStyle guistyle = new GUIStyle(this._textStyle);
					guistyle.normal.background = this._logRowStyle.normal.background;
					guistyle.wordWrap = true;
					GUI.Label(new Rect(610f, 100f, 600f, 500f), GUI.tooltip, guistyle);
				}
			}
			if (this._showGamePadWheel)
			{
				this.ShowGamepadWheel();
			}
		}

        protected override void Update()
        {
			base.Update();
			// If pressing PageDown or holding it and time since last input larger than the input delay
			if (UnityEngine.Input.GetKeyDown(KeyCode.PageDown) || (UnityEngine.Input.GetKey(KeyCode.PageDown) && (Time.time - lastKeyTime) > this.inputDelay))
            {
				consecutiveInputs++;
				this.scrollPosition.y = Mathf.Clamp(this.scrollPosition.y + 100, 0, contentHeight - 500);
				lastKeyTime = Time.time;
				// When scroll keys are held, the scrolling speed becomes increasingly quick
				// sqrt(exp(x)) was chosen as an arbitrary function, sqrt(x) grew too slowly and exp(x) grew too quickly so this was a good compromise
				inputDelay = 0.35f / Mathf.Sqrt(Mathf.Exp(consecutiveInputs));
			}
			// If pressing PageUp or holding it and time since last input larger than the input delay
			if (UnityEngine.Input.GetKeyDown(KeyCode.PageUp) || (UnityEngine.Input.GetKey(KeyCode.PageUp) && (Time.time - lastKeyTime) > this.inputDelay))
			{
				consecutiveInputs++;
				this.scrollPosition.y = Mathf.Clamp(this.scrollPosition.y - 100, 0, contentHeight - 500);
				lastKeyTime = Time.time;
				inputDelay = 0.35f / Mathf.Sqrt(Mathf.Exp(consecutiveInputs));
			}
			// If neither of the log scrolling keys are held, inputs and consecutive inputs counter are reset
            if (! (UnityEngine.Input.GetKey(KeyCode.PageDown) || UnityEngine.Input.GetKey(KeyCode.PageUp)))
            {
				consecutiveInputs = 0;
				inputDelay = 0.35f;
            }
			if (ImprovedDebugConsole.awaitingSettingsUpdate)
            {
				applySettings();
            }
		}

        protected override void FinalizeConsoleInput()
        {
			this.HandleConsoleInput(this._consoleInput);
			if (this.closeOnInput)
            {
				this.ShowConsole(false);
            }
            else
            {
				this._consoleInput = "";
            }
			this.scrollToBottom();
        }

        private void scrollToBottom()
        {
			this.scrollPosition.y = contentHeight - 500;
        }

		public void applySettings()
        {
			ImprovedDebugConsole.awaitingSettingsUpdate = false;

			this.errorColor = ImprovedDebugConsole.errorColor;
			this.warningColor = ImprovedDebugConsole.warningColor;
			this.exceptionColor = ImprovedDebugConsole.exceptionColor;
			this.logColor = ImprovedDebugConsole.logColor;

			this.textSize = ImprovedDebugConsole.textSize;
			this._maxLogs = ImprovedDebugConsole.maxLogs;
			this.closeOnInput = ImprovedDebugConsole.closeOnInput;
			this.toggleOnStartup = ImprovedDebugConsole.toggleOnStartup;

			this._logRowStyle.fixedHeight = textSize + 2f;
			this._logRowStyle.fontSize = ImprovedDebugConsole.textSize;
			this._textStyle.fontSize = ImprovedDebugConsole.textSize;

			while (this._logs.Count > this._maxLogs)
            {
				this._logs.Dequeue();
            }
		}
    }
}
