# 00 UI Setup Project
This is the initial setup of the UI that will be used throughout this project.
I've decided to use an ECS like system to not only familiarize yourself with the ECS concept, but to be able to convert this to ECS code later.
It's not pure ECS, because I wanted to build to WebGL, Windows, Mac, Linux, IOS, Android without any hassle.
This seemed to be the best wa as of 2019.1.14f1.

## Entities

### SplitScreen

Stores scenes that are loaded, which scene is in focus, and how many scenes are there

#### Components

- **OnEnableListener** - A special MonoBehaviour that creates a OnEnableEvent Component when the entity is enabled
- **LoadScene** - Scenes that should be loaded (and split)
- **FocusedPanel** - Which panel is focused and number of panels on screen

### HighlightPanel
The yellow border around a focused panel

### Panel

- **UIText** - A UI Text object
- **FocusPanel** - Data about isFocus and position/size of the panel
- **FocusInput** - User input (if panel is focused)

#### Components

- **HighlightPanel** - The position/size of the panel

## Local Systems

### 0 SplitScreen Systems

- **LoadSceneSystem** - Loads the defined scenes OnEnable

### 0 Test UI

- **FocusInputSystem** - If a panel/scene is focused, updates FocusInput(s) in same scene with user input
- **FocusInputTestUISystem** - Update the UIText that has FocusInput on it

## Global Systems

- **SoloFocusPanelFocus** - If there is only one panel, update that FocusPanel.isFocused = true
- **FocusPanelResize** - Change size and position of panels depending on count of FocusPanels
- **DefaultFocusPanel** - If there is > 0 panels, and no panel is focused, focus first panel
- **HighlightPanelSystem** - Resize highlight panel to the panel which it is focusing
- **FocusOnClick** - Change focus depending where mouse was last clicked
- **FocusPanelSystem** - Set FocusPanel.IsFocus to only the index selected in FocusedPanel. Remove rest.