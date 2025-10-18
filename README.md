<div align="center">

# FLFloppa Editor Helpers

_Reusable UI Toolkit utilities for polished Unity editor experiences_

[![Unity 2022.3+](https://img.shields.io/badge/unity-2022.3%2B-black.svg?logo=unity)](#requirements)
[![License: MIT](https://img.shields.io/badge/license-MIT-green.svg)](#license)

</div>

The FLFloppa Editor Helpers package ships shared UI Toolkit building blocks used across FLFloppa products. It focuses on consistent card layouts, property controls, and expandable summary lists that keep inspectors lightweight and designer-friendly.

---

## Table of contents

* [Highlights](#highlights)
* [Requirements](#requirements)
* [Installation](#installation)
* [Usage](#usage)
* [Utilities](#utilities)
* [Samples](#samples)
* [Documentation](#documentation)
* [Contributing](#contributing)
* [Support](#support)
* [License](#license)

---

## Highlights

* __UI Toolkit first__ – Components are implemented with pure UI Toolkit for instant compatibility with editor themes.
* __Consistent theming__ – Card helpers adapt automatically to light/dark palettes.
* __Composable helpers__ – Foundational API surface for layouts, cards, controls, and expandable lists.
* __Zero runtime baggage__ – Editor-only assembly definition keeps runtime assemblies lean.

---

## Requirements

* Unity **2022.3 LTS** or newer.

---

## Installation

### Via Git URL

1. Open **Window → Package Manager**.
2. Select the **+** button → **Add package from git URL…**
3. Paste the repository URL with the package path:
   ```
   https://github.com/FLFloppa/events-system.git
   ```
4. Unity will import the editor helpers assembly.

### Local copy

1. Clone/download the repository.
2. Copy `Packages/FLFloppa Editor Helpers/` into your project `Packages/` directory.
3. Restart the editor to refresh assembly definitions.

---

## Usage

Reference the helpers from any custom inspector or editor window:

```csharp
using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(MyAsset))]
public sealed class MyAssetInspector : UnityEditor.Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = InspectorUi.Layout.CreateRoot();

        root.Add(InspectorUi.Layout.CreateHeader(
            "My Asset",
            "Configure behaviour and review key settings."));

        root.Add(InspectorUi.Cards.Create(
            "Settings",
            out var settings,
            "Adjust serialized properties with built-in styling."));
        settings.Add(InspectorUi.Controls.CreatePropertyField(serializedObject.FindProperty("myProperty"), "My Property"));

        var summary = InspectorUi.ExpandableLists.Create(
            "Summary",
            "Show computed values or validation output.",
            expanded: true);
        summary.AddItem("Resolved Path", "/Config/game.json");
        root.Add(summary.Root);

        return root;
    }
}
```

---

## Utilities

The core API lives in `Editor/Utilities/InspectorUi.cs` and is split into namespaces:

* __`InspectorUi.Layout`__ – Scroll view roots and headers.
* __`InspectorUi.Cards`__ – Card containers with optional descriptions.
* __`InspectorUi.Controls`__ – Property fields, action buttons, help boxes, and summary labels.
* __`InspectorUi.ExpandableLists`__ – Stylised collapsible lists for summaries or diagnostics.

Each helper returns ready-to-use `VisualElement` instances so you can mix and match with bespoke UI Toolkit content.

---

## Samples

This package does not bundle a sample scene. See the FLFloppa Events System package for real-world inspector implementations using these helpers.

---

## Documentation

* [`Docs/manual.md`](Docs/manual.md) – Detailed API reference, integration patterns, and troubleshooting tips.

---

## Contributing

Issues and pull requests are welcome. Ensure stylistic changes follow existing conventions and include screenshots when adjusting visual behaviour.

---

## Support

* Email: `flfloppa@yandex.ru`

---

## License

Released under the [MIT License](LICENSE). Feel free to reuse helpers in commercial and open-source projects with attribution.
