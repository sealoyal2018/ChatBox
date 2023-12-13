using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using ChatBox.Models;
using ChatBox.Modules.Chats.Models;

namespace ChatBox.Components;

public class ChatTemplateSelector : IDataTemplate
{
    // This Dictionary should store our shapes. We mark this as [Content], so we can directly add elements to it later.
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();

    // Build the DataTemplate here
    public Control Build(object? param)
    {
        var key = param?.GetType().Name;
        if (key is null) // If the key is null, we throw an ArgumentNullException
        {
            throw new ArgumentNullException(nameof(param));
        }

        if (!AvailableTemplates.ContainsKey(key))
        {
            throw new KeyNotFoundException(key);
        }
        
        
        return AvailableTemplates[key]
                .Build(param); // finally we look up the provided key and let the System build the DataTemplate for us
    }

    // Check if we can accept the provided data
    public bool Match(object? data)
    {
        // Our Keys in the dictionary are strings, so we call .ToString() to get the key to look up
        var key = data?.GetType().Name;

        return data is Chat // the provided data needs to be our enum type
               && !string.IsNullOrEmpty(key) // and the key must not be null or empty
               && AvailableTemplates.ContainsKey(key); // and the key must be found in our Dictionary
    }
}