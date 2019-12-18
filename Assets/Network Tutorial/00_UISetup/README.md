# UI Setup Project
This is the initial setup of the UI that will be used throughout this project.
I've decided to use an ECS like system to not only familiarize yourself with the ECS concept, but to be able to convert this to ECS code later.
It's not pure ECS, because I wanted to build to WebGL, Windows, Mac, Linux, IOS, Android without any hassle.
This seemed to be the best way as of 2019.1.14f1.

## Entities and Components
An entity is a named container that contains components.  
A component is a small data object that has no functions/methods.

### HighlightPanel Entity
The yellow border around a focused panel
- **SplitScreenHighlightPanel** - UI position/size of Yellow Highlight Box

### Panel Entity
A UI Panel
- **UIText** - A UI Text object
- **SplitScreenPanel** - Panel Data storing if isFocused and position/size of the panel
- **SplitScreenInput** - User input variables on specific panel

## Systems
During each frame, a system performs functions on a component or coupling of components.
- **FocusOnFirstIfNoPanelsAreFocused** - If no panels are focused, focused on first found panel
- **FocusOnSplitScreenFocusedPanel** - Always focus on panel specified by Panel.SplitScreenFocusedPlanel
- **FocusSplitScreenPanelOnClick** - Change focused panel on click
- **HighlightFocusedPanel** - Resize highlight panel to the panel which it is focusing
- **KeepOnlyOneDirectionalLight** - Only keep one directional light in all active scenes
- **LimitAudioListeners** - Only keep one audio listener active in all active scenes
- **ResizeSplitScreenCamera** - Resize Camera viewport depending how many SplitScreens there are
- **ResizeSplitScreenPanels** - Resize Panel depending how many SplitScreens there are
- **HandlSplitScreenInput** - Update data on Panel.SplitScreenInput
- **ShowSplitScreenInputOnTextUI** - Shows input on Panel.UIText
- **GetApplicationInformationSystem** - Populates ApplicationInformation components

-------------------------------------

# SplitScreen Project
This is the scene in almost every following project that loads multiple scenes in a split screen fashion.

## Entities and Components

### SplitScreen Entity
Stores scenes that are loaded, which scene is in focus, and how many scenes are there
- **OnEnableListener** - A special MonoBehaviour that creates a OnEnableEvent Component when the entity is enabled
- **LoadScene** - Scenes that should be loaded (and split)
- **SplitScreenFocusedPanel** - Stores which panel is focused and number of panels on screen

## Systems
- **LoadSceneSystem** - Loads the defined scenes OnEnable
