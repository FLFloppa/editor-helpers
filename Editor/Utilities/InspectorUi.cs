using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FLFloppa.EditorHelpers
{
    /// <summary>
    /// Provides composable UI Toolkit helpers for building consistent editor inspectors.
    /// </summary>
    public static class InspectorUi
    {
        public static class Layout
        {
            public const float SectionSpacing = 10f;
            public const float ButtonSpacing = 6f;

            /// <summary>
            /// Creates the standard scrollable root container used by FLFloppa inspectors.
            /// </summary>
            public static ScrollView CreateRoot()
            {
                var root = new ScrollView
                {
                    style =
                    {
                        paddingTop = 6,
                        paddingBottom = 6,
                        paddingLeft = 8,
                        paddingRight = 8
                    }
                };

                root.contentContainer.style.flexDirection = FlexDirection.Column;
                return root;
            }

            /// <summary>
            /// Creates a header block with title and optional description.
            /// </summary>
            public static VisualElement CreateHeader(string title, string? description = null)
            {
                var container = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column,
                        marginBottom = SectionSpacing
                    }
                };

                container.Add(new Label(title)
                {
                    style =
                    {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        fontSize = 14
                    }
                });

                if (!string.IsNullOrEmpty(description))
                {
                    container.Add(new Label(description)
                    {
                        style =
                        {
                            whiteSpace = WhiteSpace.Normal,
                            marginTop = 2
                        }
                    });
                }

                return container;
            }
        }

        public static class Cards
        {
            private static readonly Color CardColorPro = new(0.19f, 0.19f, 0.19f, 1f);
            private static readonly Color CardColorLight = new(0.87f, 0.87f, 0.87f, 1f);
            private static readonly Color BorderColor = new(0f, 0f, 0f, 0.35f);

            /// <summary>
            /// Creates a card container with optional description and exposes a content area.
            /// </summary>
            public static VisualElement Create(string title, out VisualElement content, string? description = null)
            {
                var card = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column,
                        paddingTop = 8,
                        paddingBottom = 8,
                        paddingLeft = 10,
                        paddingRight = 10,
                        marginBottom = Layout.SectionSpacing,
                        backgroundColor = EditorGUIUtility.isProSkin ? CardColorPro : CardColorLight,
                        borderTopWidth = 1,
                        borderBottomWidth = 1,
                        borderLeftWidth = 1,
                        borderRightWidth = 1,
                        borderTopColor = BorderColor,
                        borderBottomColor = BorderColor,
                        borderLeftColor = BorderColor,
                        borderRightColor = BorderColor
                    }
                };

                card.Add(new Label(title)
                {
                    style =
                    {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        fontSize = 12,
                        marginBottom = 2
                    }
                });

                if (!string.IsNullOrEmpty(description))
                {
                    card.Add(new Label(description)
                    {
                        style =
                        {
                            whiteSpace = WhiteSpace.Normal,
                            color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.8f) : new Color(0f, 0f, 0f, 0.8f),
                            marginBottom = 6
                        }
                    });
                }

                content = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column
                    }
                };

                card.Add(content);

                return card;
            }
        }

        public static class Controls
        {
            /// <summary>
            /// Creates a consistently styled action button.
            /// </summary>
            public static Button CreateActionButton(string label, Action onClick)
            {
                var button = new Button(onClick)
                {
                    text = label,
                    style =
                    {
                        minWidth = 150,
                        marginRight = Layout.ButtonSpacing,
                        marginBottom = Layout.ButtonSpacing,
                        paddingLeft = 10,
                        paddingRight = 10
                    }
                };

                return button;
            }

            /// <summary>
            /// Creates a property field bound to the supplied property with optional change callback.
            /// </summary>
            public static PropertyField CreatePropertyField(SerializedProperty property, string label, EventCallback<SerializedPropertyChangeEvent>? onValueChanged = null)
            {
                var field = new PropertyField(property, label)
                {
                    style = { marginBottom = 4 }
                };

                field.BindProperty(property);

                if (onValueChanged != null)
                {
                    field.RegisterValueChangeCallback(onValueChanged);
                }

                return field;
            }

            /// <summary>
            /// Builds a column container for grouping related buttons.
            /// </summary>
            public static VisualElement CreateButtonColumn(string title, params Button[] buttons)
            {
                var column = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column,
                        flexGrow = 1,
                        marginRight = Layout.SectionSpacing
                    }
                };

                column.Add(new Label(title)
                {
                    style =
                    {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        marginBottom = 4
                    }
                });

                foreach (var button in buttons)
                {
                    column.Add(button);
                }

                return column;
            }

            /// <summary>
            /// Adds a styled help box to the provided parent container.
            /// </summary>
            public static HelpBox AddHelpBox(VisualElement parent, string message, HelpBoxMessageType type)
            {
                var helpBox = new HelpBox(message, type)
                {
                    style =
                    {
                        whiteSpace = WhiteSpace.Normal,
                        marginBottom = 4
                    }
                };

                parent.Add(helpBox);
                return helpBox;
            }

            /// <summary>
            /// Creates a summary label entry.
            /// </summary>
            public static Label CreateSummaryLabel(string text)
            {
                return new Label(text)
                {
                    style = { marginBottom = 2 }
                };
            }
        }

        public static class ExpandableLists
        {
            private static readonly Color ContainerBackgroundPro = new(0.16f, 0.16f, 0.16f, 1f);
            private static readonly Color ContainerBackgroundLight = new(0.94f, 0.94f, 0.94f, 1f);
            private static readonly Color ItemBackgroundPro = new(0.23f, 0.23f, 0.23f, 1f);
            private static readonly Color ItemBackgroundLight = new(0.98f, 0.98f, 0.98f, 1f);
            private static readonly Color BorderColor = new(0f, 0f, 0f, 0.25f);

            public sealed class ListControl
            {
                internal ListControl(VisualElement root, Foldout foldout, VisualElement itemsContainer)
                {
                    Root = root;
                    Foldout = foldout;
                    _itemsContainer = itemsContainer;
                }

                private readonly VisualElement _itemsContainer;

                public VisualElement Root { get; }
                public Foldout Foldout { get; }

                public void ClearItems()
                {
                    _itemsContainer.Clear();
                }

                public void AddItem(string primaryText, string? secondaryText = null)
                {
                    var row = CreateRow(primaryText, secondaryText);
                    _itemsContainer.Add(row);
                }

                public void ShowEmptyState(string message)
                {
                    var label = new Label(message)
                    {
                        style =
                        {
                            color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.6f) : new Color(0f, 0f, 0f, 0.6f),
                            marginBottom = 2
                        }
                    };
                    label.style.unityFontStyleAndWeight = FontStyle.Italic;

                    _itemsContainer.Add(label);
                }

                private static VisualElement CreateRow(string primaryText, string? secondaryText)
                {
                    var row = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Column,
                            backgroundColor = EditorGUIUtility.isProSkin ? ItemBackgroundPro : ItemBackgroundLight,
                            borderTopColor = BorderColor,
                            borderBottomColor = BorderColor,
                            borderLeftColor = BorderColor,
                            borderRightColor = BorderColor,
                            borderTopWidth = 1,
                            borderBottomWidth = 1,
                            borderLeftWidth = 1,
                            borderRightWidth = 1,
                            marginBottom = 4,
                            paddingTop = 4,
                            paddingBottom = 4,
                            paddingLeft = 6,
                            paddingRight = 6,
                        }
                    };

                    row.style.borderBottomLeftRadius = 4;
                    row.style.borderBottomRightRadius = 4;
                    row.style.borderTopLeftRadius = 4;
                    row.style.borderTopRightRadius = 4;

                    var primary = new Label(primaryText)
                    {
                        style =
                        {
                            unityFontStyleAndWeight = FontStyle.Bold,
                            marginBottom = string.IsNullOrEmpty(secondaryText) ? 0 : 2
                        }
                    };
                    row.Add(primary);

                    if (!string.IsNullOrEmpty(secondaryText))
                    {
                        row.Add(new Label(secondaryText)
                        {
                            style =
                            {
                                fontSize = 11,
                                color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f)
                            }
                        });
                    }

                    return row;
                }
            }

            public static ListControl Create(string title, string? description = null, bool expanded = false)
            {
                var root = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column,
                        paddingTop = 6,
                        paddingBottom = 6,
                        paddingLeft = 8,
                        paddingRight = 8,
                        marginBottom = Layout.SectionSpacing,
                        backgroundColor = EditorGUIUtility.isProSkin ? ContainerBackgroundPro : ContainerBackgroundLight,
                        borderTopColor = BorderColor,
                        borderBottomColor = BorderColor,
                        borderLeftColor = BorderColor,
                        borderRightColor = BorderColor,
                        borderTopWidth = 1,
                        borderBottomWidth = 1,
                        borderLeftWidth = 1,
                        borderRightWidth = 1,
                    }
                };

                root.style.borderBottomLeftRadius = 6;
                root.style.borderBottomRightRadius = 6;
                root.style.borderTopLeftRadius = 6;
                root.style.borderTopRightRadius = 6;

                var foldout = new Foldout
                {
                    text = title,
                    value = expanded
                };
                foldout.style.marginBottom = 0;
                foldout.style.paddingLeft = 0;

                var toggle = foldout.Q<Toggle>();
                if (toggle != null)
                {
                    toggle.style.unityFontStyleAndWeight = FontStyle.Bold;
                    toggle.style.marginLeft = 0;
                    toggle.style.paddingLeft = 12;
                    toggle.style.paddingRight = 4;

                    var arrow = toggle.Q(className: "unity-foldout__arrow");
                    if (arrow != null)
                    {
                        arrow.style.marginLeft = 0;
                        arrow.style.left = 0;
                    }
                }

                var contentContainer = foldout.contentContainer;
                contentContainer.style.flexDirection = FlexDirection.Column;
                contentContainer.style.marginTop = 6;
                // use child margin for spacing instead of row gap for broader compatibility

                if (!string.IsNullOrEmpty(description))
                {
                    contentContainer.Add(new Label(description)
                    {
                        style =
                        {
                            whiteSpace = WhiteSpace.Normal,
                            color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.75f) : new Color(0f, 0f, 0f, 0.75f),
                            marginBottom = 6
                        }
                    });
                }

                var itemsContainer = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column
                    }
                };

                contentContainer.Add(itemsContainer);
                root.Add(foldout);

                return new ListControl(root, foldout, itemsContainer);
            }
        }
    }
}
