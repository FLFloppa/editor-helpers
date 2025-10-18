# FLFloppa Editor Helpers Manual

This manual covers the UI Toolkit helper API provided by the FLFloppa Editor Helpers package.

---

## 1. Overview

The package offers opinionated helpers under the `FLFloppa.EditorHelpers.InspectorUi` static class. Utilities are split into four namespaces:

* `InspectorUi.Layout`
* `InspectorUi.Cards`
* `InspectorUi.Controls`
* `InspectorUi.ExpandableLists`

Use them together to assemble inspectors that match FLFloppa styling without duplicating code.

---

## 2. Getting started

```csharp
using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(MySettings))]
public sealed class MySettingsInspector : UnityEditor.Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = InspectorUi.Layout.CreateRoot();

        root.Add(InspectorUi.Layout.CreateHeader(
            "My Settings",
            "Configure application behaviour."));

        root.Add(InspectorUi.Cards.Create(
            "Configuration",
            out var content,
            "All serialized fields are surfaced with standardised styling."));

        var property = serializedObject.FindProperty("configurationPath");
        content.Add(InspectorUi.Controls.CreatePropertyField(property, "Configuration Path"));

        var summary = InspectorUi.ExpandableLists.Create(
            "Summary",
            "Runtime data mapped from the selected configuration.",
            expanded: true);
        summary.AddItem("Resolved Path", "/Configs/app.json");
        root.Add(summary.Root);

        return root;
    }
}
```

---

## 3. Layout helpers

### 3.1 `InspectorUi.Layout.CreateRoot()`

Creates a scroll view root configured for column layout. Wrap your entire inspector with this container to ensure consistent padding.

### 3.2 `InspectorUi.Layout.CreateHeader(title, description)`

Returns a vertically stacked header with bold title and optional description.

---

## 4. Card helpers

`InspectorUi.Cards.Create(title, out content, description)` builds a card container with title and optional description. The returned `content` element is where controls should be appended.

Cards automatically adjust background colours and borders based on the active Unity editor theme.

---

## 5. Control helpers

| Helper | Purpose |
|--------|---------|
| `CreateActionButton(label, onClick)` | Consistent button spacing and minimum width. |
| `CreatePropertyField(property, label, onValueChanged)` | Creates a `PropertyField` bound to the property and optionally applies a callback. |
| `CreateButtonColumn(title, params Button[])` | Organises action buttons in labeled columns.| 
| `AddHelpBox(parent, message, type)` | Adds styled `HelpBox` with line wrapping. |
| `CreateSummaryLabel(text)` | Generates a simple label with common spacing for summary sections. |

---

## 6. Expandable lists

`InspectorUi.ExpandableLists.Create(title, description, expanded)` returns a `ListControl` struct:

```csharp
var list = InspectorUi.ExpandableLists.Create("Diagnostics", "Important details.");
list.AddItem("Result", "Ok");
list.ShowEmptyState("No entries recorded.");
inspectorRoot.Add(list.Root);
```

ListControl members:

* `Root` – The root `VisualElement` to add to the inspector.
* `Foldout` – Access to the underlying `Foldout` if custom behaviour is required.
* `ClearItems()` – Remove all rows.
* `AddItem(primary, secondary)` – Add a styled row with optional secondary text.
* `ShowEmptyState(message)` – Display italic placeholder text.

---

## 7. Tips

* Combine these helpers with native UI Toolkit controls for bespoke layouts.
* Use `Foldout` callbacks from `ListControl.Foldout` when you need to refresh items lazily.
* The helpers are editor-only; avoid referencing them from runtime assemblies.

---

## 8. Troubleshooting

* **Arrow misaligned** – Ensure the project uses the latest package version; older implementations may not set toggle padding.
* **Missing styles** – Verify the inspector inherits from `UnityEditor.Editor` and uses `CreateInspectorGUI()` with UI Toolkit.
* **No card background** – UI Toolkit requires `EditorGUIUtility.isProSkin`; backgrounds adapt automatically based on theme.

---

## 9. Additional resources

* Review `Packages/FLFloppa Events System/Editor/Inspectors/` for real-world usage examples.
* Visit the FLFloppa Events System README for guidance on Inspector composition.
